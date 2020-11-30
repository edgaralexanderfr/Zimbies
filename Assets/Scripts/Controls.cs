using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class Controls : MonoBehaviour
{
    public GameObject character;
    public InputAction move;
    public InputAction lookX;
    public InputAction lookY;

    private int halfScreenWidth = Screen.width / 2;
    private int halfScreenHeight = Screen.height / 2;

    private CharacterController _characterController;
    private Animator _animator;
    private bool _moving = false;

    // Start is called before the first frame update
    void Start()
    {
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
        var finalVector = new Vector3();

        finalVector.x = inputVector.x;
        finalVector.z = inputVector.y;

        _characterController.Move(-finalVector * Time.deltaTime * 30.0f);

        // Updates the character's movement animation:
        if (inputVector.x == 0.0f && inputVector.y == 0.0f)
        {
            if (_moving)
            {
                _moving = false;
                _animator.Play("Idle Aiming");
            }
        }
        else
        {
            if (!_moving)
            {
                _moving = true;
                _animator.Play("Walk Aiming");
            }
        }
    }

    // Updates the player's character look based on the look controls input
    void UpdateLook()
    {
        float mouseX = lookX.ReadValue<float>() - halfScreenWidth;
        float mouseY = lookY.ReadValue<float>() - halfScreenHeight;

        var target = new Vector3(-mouseX, 0.0f, -mouseY);
        character.transform.LookAt(character.transform.position + target);
    }

    void OnEnable()
    {
        move.Enable();
        lookX.Enable();
        lookY.Enable();
    }

    void OnDisable()
    {
        move.Disable();
        lookX.Disable();
        lookY.Disable();
    }
}
