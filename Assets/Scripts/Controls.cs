using UnityEngine;
using UnityEngine.InputSystem;

public class Controls : MonoBehaviour
{
    public static Controls current { get { return m_current; } }

    private static Controls m_current;

    #region[Purple] Settings
    public GameObject Indicator;
    #endregion Settings

    #region[Green] Input Actions
    public InputAction Move;
    public InputAction LookX;
    public InputAction LookY;
    public InputAction Melee;
    public InputAction ToggleConsole;
    public InputAction Enter;
    public InputAction Up;
    public InputAction Down;
    #endregion Input Actions

    #region[Blue] Private Members
    private int halfScreenWidth = Screen.width / 2;
    private int halfScreenHeight = Screen.height / 2;
    private GameObject m_characterGameObject;
    private GameObject m_gun;
    private GameObject m_axe;
    private GameObject m_sheathedAxe;
    private GameObject m_sheathedGun;
    private Character m_character;
    private CharacterController m_characterController;
    private Animator m_animator;
    #endregion Private Members

    void Awake()
    {
        m_current = this;
    }

    // Start is called before the first frame update
    void Start()
    {
        AddEvents();
        TakeControlAt("Player 1");
    }

    void AddEvents()
    {
        // Console events:
        ToggleConsole.performed += OnToggleConsolePerformed;

        Enter.canceled += OnEnterCanceled;
        Up.canceled += OnUpCanceled;
        Down.canceled += OnDownCanceled;
    }

    // Update is called once per frame
    void Update()
    {
        UpdateMove();
        UpdateLook();
    }

    public void TakeControlAtCurrentPlayer()
    {
        var character = GameObject.FindWithTag("Player");

        if (character) TakeControlAt(character);
    }

    public void TakeControlAt(string name)
    {
        var character = GameObject.Find(name);

        if (character) TakeControlAt(character);
    }

    public void TakeControlAt(GameObject character)
    {
        character.tag = "Player";
        if (m_characterGameObject) m_characterGameObject.tag = "Untagged";

        m_characterGameObject = character;

        m_gun = character.transform.Find("Armature/Torso/R_Arm/R_Hand/Gun").gameObject;
        m_axe = character.transform.Find("Armature/Torso/R_Arm/R_Hand/Axe").gameObject;
        m_sheathedAxe = character.transform.Find("Body/Sheathed Axe").gameObject;
        m_sheathedGun = character.transform.Find("Body/Sheathed Gun").gameObject;

        m_character = character.GetComponent<Character>();
        m_characterController = character.GetComponent<CharacterController>();
        m_animator = character.GetComponent<Animator>();

        Follower.current.Target = character;
    }

    // Updates the player's character movement based on the move controls input
    private void UpdateMove()
    {
        var inputVector = Move.ReadValue<Vector2>();
        float meleeing = Melee.ReadValue<float>();
        var finalVector = new Vector3();

        finalVector.x = inputVector.x;
        finalVector.z = inputVector.y;

        m_characterController.Move(-finalVector * Time.deltaTime * m_character.Speed);

        // Check if character is meleeing:
        if (meleeing == 0.0f)
        {
            m_character.Meleeing = false;

            // Cancel the melee:
            m_axe.SetActive(false);
            m_sheathedGun.SetActive(false);
            m_sheathedAxe.SetActive(true);
            m_gun.SetActive(true);

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
            m_character.Meleeing = true;

            // Melee:
            m_gun.SetActive(false);
            m_sheathedAxe.SetActive(false);
            m_sheathedGun.SetActive(true);
            m_axe.SetActive(true);

            m_animator.Play("Melee");
        }
    }

    // Updates the player's character look based on the look controls input
    private void UpdateLook()
    {
        float mouseX = LookX.ReadValue<float>();
        float mouseY = LookY.ReadValue<float>();

        var target = new Vector3(
            -mouseX + halfScreenWidth,
            0.0f,
            -mouseY + halfScreenHeight
        );

        m_characterGameObject.transform.LookAt(m_characterGameObject.transform.position + target);

        // Updates the indicator's position:
        var ray = Camera.main.ScreenPointToRay(new Vector3(mouseX, mouseY, 0.0f));
        RaycastHit hit;

        if (Physics.Raycast(ray, out hit, Mathf.Infinity, TerrainPlane.LAYER_MASK))
        {
            float indicatorX = Mathf.Floor(hit.point.x / 10.0f) * 10.0f + 5.0f;
            float indicatorZ = Mathf.Floor(hit.point.z / 10.0f) * 10.0f + 5.0f;

            Indicator.transform.position = new Vector3(
                indicatorX,
                Indicator.transform.position.y,
                indicatorZ
            );
        }
    }

    void OnToggleConsolePerformed(InputAction.CallbackContext ctx)
    {
        GameConsole.current.Toggle();

        if (GameConsole.current.Shown)
        {
            Move.Disable();
            Melee.Disable();
        }
        else
        {
            Move.Enable();
            Melee.Enable();
        }
    }

    void OnEnterCanceled(InputAction.CallbackContext ctx)
    {
        if (GameConsole.current.Shown)
        {
            GameConsole.current.ExecInputField();
            GameConsole.current.Hide();

            Move.Enable();
            Melee.Enable();
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
        Move.Enable();
        LookX.Enable();
        LookY.Enable();
        Melee.Enable();
        ToggleConsole.Enable();
        Enter.Enable();
        Up.Enable();
        Down.Enable();
    }

    void OnDisable()
    {
        Move.Disable();
        LookX.Disable();
        LookY.Disable();
        Melee.Disable();
        ToggleConsole.Disable();
        Enter.Disable();
        Up.Disable();
        Down.Disable();
    }
}
