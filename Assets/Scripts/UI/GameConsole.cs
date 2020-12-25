using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameConsole : MonoBehaviour
{
    public static GameConsole current { get { return m_current; } }

    private static GameConsole m_current;

    #region[Purple] Settings
    public float DisplayOffset;
    public sbyte MaxCommands;
    #endregion Settings

    public bool Shown { get { return m_shown; } }

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
        if (!m_shown) Show(); else Hide();
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
        var lowerCaseCmd = cmd.ToLower();
        bool shorted = false;

        try
        {
            if ((shorted = (lowerCaseCmd == "pp")) || lowerCaseCmd == "plant pine")
            {
                TerrainPlane.current.PlantTree(Controls.current.Indicator.transform.position.x, Controls.current.Indicator.transform.position.z);
            }
            else if ((shorted = (lowerCaseCmd == "cww")) || lowerCaseCmd == "craft wooden wall")
            {
                if (character != null && character.Inventory.Wood >= 3)
                {
                    TerrainPlane.current.CraftWoodenWall(Controls.current.Indicator.transform.position.x, Controls.current.Indicator.transform.position.z);
                    character.Inventory.Wood -= 3;
                }
            }
            else if ((shorted = (lowerCaseCmd.StartsWith("gw "))) || lowerCaseCmd.StartsWith("give wood "))
            {
                var instruction = shorted ? "gw " : "give wood ";
                character.Inventory.Wood += ExtractValue<int>(instruction, cmd);
            }
            else if ((shorted = (lowerCaseCmd.StartsWith("tca "))) || lowerCaseCmd.StartsWith("take control at "))
            {
                var instruction = shorted ? "tca " : "take control at ";
                var value = ExtractValue<string>(instruction, cmd);
                Controls.current.TakeControlAt(value);
            }
            else if ((shorted = (lowerCaseCmd.StartsWith("sc "))) || lowerCaseCmd.StartsWith("spawn character "))
            {
                var instruction = shorted ? "sc " : "spawn character ";
                var value = ExtractValue<string>(instruction, cmd);
                God.current.SpawnCharacter(value, Controls.current.Indicator.transform.position.x, Controls.current.Indicator.transform.position.z);
            }
        }
        catch (FormatException e)
        {
            Debug.LogException(e);
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

    private T ExtractValue<T>(string cmd, string input)
    {
        var value = input.Substring(cmd.Length, input.Length - cmd.Length);

        return (T) Convert.ChangeType(value, typeof(T));
    }
}
