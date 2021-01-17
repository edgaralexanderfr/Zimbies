using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameConsole : MonoBehaviour
{
    public static GameConsole current { get { return m_current; } }

    private static GameConsole m_current;

    #region[Purple] Settings
    public Text OutputText;
    public float DisplayOffset;
    public sbyte MaxCommands;
    #endregion Settings

    public bool Shown { get { return m_shown; } }

    #region[Blue] Private Members
    private InputField m_inputField;
    private RectTransform m_rectTransform;
    private bool m_shown;
    private List<string> m_lastCmds = new List<string>();
    private int m_currentCmd = 0;
    #endregion Private Members

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
        m_inputField.placeholder.GetComponent<Text>().text += $" v{Application.version}";

        // Set console initial output:
        OutputText.text = Debug.isDebugBuild ? OutputText.text + $" {Application.version}" : "";
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

        // You can use GameConsole.Exec.code-snippets
        try
        {
            // plant pine:
            if ((shorted = (lowerCaseCmd == "pp")) || lowerCaseCmd == "plant pine")
            {
                TerrainPlane.current.PlantTree(Controls.current.Indicator.transform.position.x, Controls.current.Indicator.transform.position.z, GameTree.TYPE_PINE);
            }
            // plant deciduous tree:
            if ((shorted = (lowerCaseCmd == "pdt")) || lowerCaseCmd == "plant deciduous tree")
            {
                TerrainPlane.current.PlantTree(Controls.current.Indicator.transform.position.x, Controls.current.Indicator.transform.position.z, GameTree.TYPE_DECIDUOUS);
            }
            // craft wooden wall:
            else if ((shorted = (lowerCaseCmd == "cww")) || lowerCaseCmd == "craft wooden wall")
            {
                if (character != null && character.Inventory.Wood >= 3)
                {
                    TerrainPlane.current.CraftWoodenWall(Controls.current.Indicator.transform.position.x, Controls.current.Indicator.transform.position.z);
                    character.Inventory.Wood -= 3;
                }
            }
            // print player.inventory.wood:
            else if ((shorted = (lowerCaseCmd == "ppiw")) || lowerCaseCmd == "print player.inventory.wood")
            {
                if (character != null)
                {
                    OutputText.text = $"Player.Inventory.Wood: {character.Inventory.Wood}";
                }
            }
            // flash:
            else if ((shorted = (lowerCaseCmd == "f")) || lowerCaseCmd == "flash")
            {
                if (character != null)
                {
                    character.Speed = 200.0f;
                }
            }
            // spawn zombie active:
            else if ((shorted = (lowerCaseCmd == "sza")) || lowerCaseCmd == "spawn zombie active")
            {
                God.current.SpawnZombie(Zombie.Modes.Active, Controls.current.Indicator.transform.position.x, Controls.current.Indicator.transform.position.z);
            }
            // spawn zombie passive:
            else if ((shorted = (lowerCaseCmd == "szp")) || lowerCaseCmd == "spawn zombie passive")
            {
                God.current.SpawnZombie(Zombie.Modes.Passive, Controls.current.Indicator.transform.position.x, Controls.current.Indicator.transform.position.z);
            }
            // give wood:
            else if ((shorted = (lowerCaseCmd.StartsWith("gw "))) || lowerCaseCmd.StartsWith("give wood "))
            {
                var instruction = shorted ? "gw " : "give wood ";
                character.Inventory.Wood += ExtractValue<int>(instruction, cmd);
            }
            // take control at:
            else if ((shorted = (lowerCaseCmd.StartsWith("tca "))) || lowerCaseCmd.StartsWith("take control at "))
            {
                var instruction = shorted ? "tca " : "take control at ";
                var value = ExtractValue<string>(instruction, cmd);
                Controls.current.TakeControlAt(value);
            }
            // spawn character:
            else if ((shorted = (lowerCaseCmd.StartsWith("sc "))) || lowerCaseCmd.StartsWith("spawn character "))
            {
                var instruction = shorted ? "sc " : "spawn character ";
                var value = ExtractValue<string>(instruction, cmd);
                God.current.SpawnCharacter(value, Controls.current.Indicator.transform.position.x, Controls.current.Indicator.transform.position.z);
            }
            // print:
            else if ((shorted = (lowerCaseCmd.StartsWith("p "))) || lowerCaseCmd.StartsWith("print "))
            {
                var instruction = shorted ? "p " : "print ";
                var value = ExtractValue<string>(instruction, cmd);
                OutputText.text = value;
            }
        }
        catch (FormatException ex)
        {
            Debug.LogException(ex);
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
