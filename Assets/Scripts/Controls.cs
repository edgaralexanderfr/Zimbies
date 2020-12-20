using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class Controls : MonoBehaviour
{
    const int TERRAIN_PLANE_LAYER_MASK = 1 << 8;

    public GameObject god;
    public GameObject character;
    public GameObject indicator;
    public GameObject gun;
    public GameObject axe;
    public GameObject sheathedAxe;
    public GameObject sheathedGun;
    public InputField console;
    public InputAction move;
    public InputAction lookX;
    public InputAction lookY;
    public InputAction melee;
    public InputAction toggleConsole;
    public InputAction enter;

    private int halfScreenWidth = Screen.width / 2;
    private int halfScreenHeight = Screen.height / 2;

    private God _god;
    private Character _character;
    private CharacterController _characterController;
    private Animator _animator;
    private RectTransform _consoleRectTransform;
    private bool _console = false;

    // Start is called before the first frame update
    void Start()
    {
        _god = god.GetComponent<God>();
        _character = character.GetComponent<Character>();
        _characterController = character.GetComponent<CharacterController>();
        _animator = character.GetComponent<Animator>();
        _consoleRectTransform = console.GetComponent<RectTransform>();

        // Set console placeholder:
        console.placeholder.GetComponent<Text>().text += " v" + Application.version;

        AddEvents();
    }

    void AddEvents()
    {
        // Console events:
        toggleConsole.performed += OnToggleConsolePerformed;

        enter.canceled += OnEnterCanceled;
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

    void OnToggleConsolePerformed(InputAction.CallbackContext ctx)
    {
        _console = !_console;

        if (_console)
        {
            console.ActivateInputField();
            _consoleRectTransform.localPosition -= Vector3.up * 100;

            move.Disable();
            melee.Disable();
        }
        else
        {
            // TODO: refactor repeated code:
            console.DeactivateInputField();
            _consoleRectTransform.localPosition += Vector3.up * 100;

            move.Enable();
            melee.Enable();
        }
    }

    void OnEnterCanceled(InputAction.CallbackContext ctx)
    {
        if (_console)
        {
            // Catch the command:
            string command = console.text.ToLower();
            console.text = "";

            // TODO: refactor repeated code:
            console.DeactivateInputField();
            _consoleRectTransform.localPosition += Vector3.up * 100;
            _console = false;

            move.Enable();
            melee.Enable();

            // Execute the command:
            switch (command)
            {
                case "pp":
                case "plant pine":
                    _god.PlantTree(indicator.transform.position.x, indicator.transform.position.z);
                    break;
                case "gw":
                case "give wood":
                    _character.inventory.wood += 9;
                    break;
                case "cww":
                case "craft wooden wall":
                    if (_character.inventory.wood >= 3)
                    {
                        // _god.CraftWoodenWall(indicator.transform.position.x + -4.0f, indicator.transform.position.z + 4.0f);
                        _god.CraftWoodenWall(indicator.transform.position.x, indicator.transform.position.z);
                        _character.inventory.wood -= 3;
                    }
                    break;
            }
        }
    }

    void OnEnable()
    {
        move.Enable();
        lookX.Enable();
        lookY.Enable();
        melee.Enable();
        toggleConsole.Enable();
        enter.Enable();
    }

    void OnDisable()
    {
        move.Disable();
        lookX.Disable();
        lookY.Disable();
        melee.Disable();
        toggleConsole.Disable();
        enter.Disable();
    }
}
