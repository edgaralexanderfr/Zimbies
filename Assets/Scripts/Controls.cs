using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class Controls : MonoBehaviour
{
    const int TERRAIN_PLANE_LAYER_MASK = 1 << 8;

    public GameObject character;
    public GameObject indicator;
    public GameObject gun;
    public GameObject axe;
    public GameObject sheathedAxe;
    public GameObject sheathedGun;
    public InputAction move;
    public InputAction lookX;
    public InputAction lookY;
    public InputAction melee;

    private int halfScreenWidth = Screen.width / 2;
    private int halfScreenHeight = Screen.height / 2;

    private Character _character;
    private CharacterController _characterController;
    private Animator _animator;

    // Start is called before the first frame update
    void Start()
    {
        _character = character.GetComponent<Character>();
        _characterController = character.GetComponent<CharacterController>();
        _animator = character.GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        UpdateMove();
        UpdateLook();
    }

    // Updates the player's character movement based on the move controls input
    void UpdateMove()
    {
        var inputVector = move.ReadValue<Vector2>();
        float meleeing = melee.ReadValue<float>();
        var finalVector = new Vector3();

        finalVector.x = inputVector.x;
        finalVector.z = inputVector.y;

        _characterController.Move(-finalVector * Time.deltaTime * 30.0f);

        // Check if character is meleeing:
        if (meleeing == 0.0f)
        {
            _character.meleeing = false;

            // Cancel the melee:
            axe.SetActive(false);
            sheathedGun.SetActive(false);
            sheathedAxe.SetActive(true);
            gun.SetActive(true);

            // Updates the character's movement animation:
            if (inputVector.x == 0.0f && inputVector.y == 0.0f)
            {
                _animator.Play("Idle Aiming");
            }
            else
            {
                _animator.Play("Walk Aiming");
            }
        }
        else
        {
            _character.meleeing = true;

            // Melee:
            gun.SetActive(false);
            sheathedAxe.SetActive(false);
            sheathedGun.SetActive(true);
            axe.SetActive(true);

            _animator.Play("Melee");
        }
    }

    // Updates the player's character look based on the look controls input
    void UpdateLook()
    {
        float mouseX = lookX.ReadValue<float>();
        float mouseY = lookY.ReadValue<float>();

        var target = new Vector3(
            -mouseX + halfScreenWidth,
            0.0f,
            -mouseY + halfScreenHeight
        );

        character.transform.LookAt(character.transform.position + target);

        // Updates the indicator's position:
        var ray = Camera.main.ScreenPointToRay(new Vector3(mouseX, mouseY, 0.0f));
        RaycastHit hit;

        if (Physics.Raycast(ray, out hit, Mathf.Infinity, TERRAIN_PLANE_LAYER_MASK))
        {
            float indicatorX = Mathf.Floor(hit.point.x / 10.0f) * 10.0f + 5.0f;
            float indicatorZ = Mathf.Floor(hit.point.z / 10.0f) * 10.0f + 5.0f;

            indicator.transform.position = new Vector3(
                indicatorX,
                indicator.transform.position.y,
                indicatorZ
            );
        }
    }

    void OnEnable()
    {
        move.Enable();
        lookX.Enable();
        lookY.Enable();
        melee.Enable();
    }

    void OnDisable()
    {
        move.Disable();
        lookX.Disable();
        lookY.Disable();
        melee.Disable();
    }
}
