using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class Controls : MonoBehaviour
{
    public static Controls current
    {
        get
        {
            return m_current;
        }
    }

    private static Controls m_current;

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
    public InputAction up;
    public InputAction down;

    private int halfScreenWidth = Screen.width / 2;
    private int halfScreenHeight = Screen.height / 2;
    private Character m_character;
    private CharacterController m_characterController;
    private Animator m_animator;

    void Awake()
    {
        m_current = this;
    }

    // Start is called before the first frame update
    void Start()
    {
        m_character = character.GetComponent<Character>();
        m_characterController = character.GetComponent<CharacterController>();
        m_animator = character.GetComponent<Animator>();

        AddEvents();
    }

    void AddEvents()
    {
        // Console events:
        toggleConsole.performed += OnToggleConsolePerformed;

        enter.canceled += OnEnterCanceled;
        up.canceled += OnUpCanceled;
        down.canceled += OnDownCanceled;
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

        m_characterController.Move(-finalVector * Time.deltaTime * 30.0f);

        // Check if character is meleeing:
        if (meleeing == 0.0f)
        {
            m_character.meleeing = false;

            // Cancel the melee:
            axe.SetActive(false);
            sheathedGun.SetActive(false);
            sheathedAxe.SetActive(true);
            gun.SetActive(true);

            // Updates the character's movement animation:
            if (inputVector.x == 0.0f && inputVector.y == 0.0f)
            {
                m_animator.Play("Idle Aiming");
            }
            else
            {
                m_animator.Play("Walk Aiming");
            }
        }
        else
        {
            m_character.meleeing = true;

            // Melee:
            gun.SetActive(false);
            sheathedAxe.SetActive(false);
            sheathedGun.SetActive(true);
            axe.SetActive(true);

            m_animator.Play("Melee");
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

        if (Physics.Raycast(ray, out hit, Mathf.Infinity, TerrainPlane.LAYER_MASK))
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
        GameConsole.current.Toggle();

        if (GameConsole.current.Shown)
        {
            move.Disable();
            melee.Disable();
        }
        else
        {
            move.Enable();
            melee.Enable();
        }
    }

    void OnEnterCanceled(InputAction.CallbackContext ctx)
    {
        if (GameConsole.current.Shown)
        {
            GameConsole.current.ExecInputField();
            GameConsole.current.Hide();

            move.Enable();
            melee.Enable();
        }
    }

    void OnUpCanceled(InputAction.CallbackContext ctx)
    {
        if (GameConsole.current.Shown)
        {
            GameConsole.current.PreviousCmd();
        }
    }

    void OnDownCanceled(InputAction.CallbackContext ctx)
    {
        if (GameConsole.current.Shown)
        {
            GameConsole.current.NextCmd();
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
        up.Enable();
        down.Enable();
    }

    void OnDisable()
    {
        move.Disable();
        lookX.Disable();
        lookY.Disable();
        melee.Disable();
        toggleConsole.Disable();
        enter.Disable();
        up.Disable();
        down.Disable();
    }
}
