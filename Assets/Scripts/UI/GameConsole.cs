using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameConsole : MonoBehaviour
{
    public static GameConsole current
    {
        get
        {
            return m_current;
        }
    }

    private static GameConsole m_current;

    public float DisplayOffset;
    public sbyte MaxCommands;

    public bool Shown
    {
        get
        {
            return m_shown;
        }
    }

    private InputField m_inputField;
    private RectTransform m_rectTransform;
    private bool m_shown;
    private List<string> m_lastCmds = new List<string>();
    private int m_currentCmd = 0;

    void Awake()
    {
        m_current = this;
    }

    // Start is called before the first frame update
    void Start()
    {
        m_inputField = GetComponent<InputField>();
        m_rectTransform = m_inputField.GetComponent<RectTransform>();

        // Set console placeholder:
        m_inputField.placeholder.GetComponent<Text>().text += " v" + Application.version;
    }

    public void Toggle()
    {
        m_shown = !m_shown;

        if (m_shown) Show(); else Hide();
    }

    public void Show()
    {
        m_inputField.ActivateInputField();
        m_rectTransform.localPosition -= Vector3.up * DisplayOffset;
        m_shown = true;
    }

    public void Hide()
    {
        m_inputField.DeactivateInputField();
        m_rectTransform.localPosition += Vector3.up * DisplayOffset;
        m_shown = false;
    }

    public void ExecInputField()
    {
        Exec(m_inputField.text);
        Clear();
    }

    public void Clear()
    {
        m_inputField.text = "";
    }

    public void Exec(string cmd)
    {
        var character = GetPlayerCharacterScript();

        switch (cmd.ToLower())
        {
            case "pp":
            case "plant pine":
                TerrainPlane.current.PlantTree(Controls.current.indicator.transform.position.x, Controls.current.indicator.transform.position.z);
                break;
            case "gw":
            case "give wood":
                if (character != null)
                {
                    character.inventory.wood += 9;
                }
                break;
            case "cww":
            case "craft wooden wall":
                if (character != null && character.inventory.wood >= 3)
                {
                    TerrainPlane.current.CraftWoodenWall(Controls.current.indicator.transform.position.x, Controls.current.indicator.transform.position.z);
                    character.inventory.wood -= 3;
                }
                break;
        }

        PushCmd(cmd);
    }

    public void PreviousCmd()
    {
        if (m_lastCmds.Count != 0)
        {
            m_currentCmd = m_currentCmd == 0 ? m_lastCmds.Count - 1 : m_currentCmd - 1;
            m_inputField.text = m_lastCmds[m_currentCmd];
            m_inputField.caretPosition = m_inputField.text.Length;
        }
    }

    public void NextCmd()
    {
        if (m_lastCmds.Count != 0)
        {
            m_currentCmd = m_currentCmd == m_lastCmds.Count - 1 ? 0 : m_currentCmd + 1;
            m_inputField.text = m_lastCmds[m_currentCmd];
            m_inputField.caretPosition = m_inputField.text.Length;
        }
    }

    private Character GetPlayerCharacterScript()
    {
        var character = GameObject.FindWithTag("Player");

        if (character == null)
        {
            return null;
        }

        return character.GetComponent<Character>();
    }

    private void PushCmd(string cmd)
    {
        m_currentCmd = 0;

        if (cmd == "" || (m_lastCmds.Count > 0 && cmd.ToLower() == m_lastCmds[ m_lastCmds.Count - 1 ].ToLower()))
        {
            return;
        }

        if (m_lastCmds.Count >= MaxCommands) m_lastCmds.RemoveAt(0);

        m_lastCmds.Add(cmd);
    }
}
