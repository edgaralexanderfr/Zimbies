using UnityEngine;

public class Indicator : MonoBehaviour
{
    public static Indicator current { get { return m_current; } }

    private static Indicator m_current;

    #region[Purple] Settings
    public Color ValidColor;
    public Color ValidTargetColor;
    public Color InvalidColor;
    public Color InvalidTargetColor;
    #endregion Settings

    #region[Blue] Private Members
    private EmissionColorBlinker m_emissionColorBlinker;
    private bool m_isValid = true;
    #endregion Private Members

    void Awake()
    {
        m_current = current;
    }

    // Start is called before the first frame update
    void Start()
    {
        m_emissionColorBlinker = GetComponent<EmissionColorBlinker>();

        m_emissionColorBlinker.Color = ValidColor;
        m_emissionColorBlinker.TargetColor = ValidTargetColor;
    }

    // Update is called once per frame
    void Update()
    {
        if (TerrainPlane.current.IsPlaceOccupied(transform.position.x, transform.position.z))
        {
            if (m_isValid)
            {
                m_isValid = false;
                m_emissionColorBlinker.SetColors(InvalidColor, InvalidTargetColor);
            }
        }
        else
        {
            if (!m_isValid)
            {
                m_isValid = true;
                m_emissionColorBlinker.SetColors(ValidColor, ValidTargetColor);
            }
        }
    }
}
