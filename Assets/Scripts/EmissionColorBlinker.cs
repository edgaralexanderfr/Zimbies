using UnityEngine;

public class EmissionColorBlinker : MonoBehaviour
{
    #region[Purple] Settings
    public Color Color;
    public float Speed;
    #endregion Settings

    #region[Blue] Private Members
    private Material m_material;
    private Color m_initialColor;
    private Color m_lerpedColor;
    #endregion Private Members

    // Start is called before the first frame update
    void Start()
    {
        m_material = GetComponent<Renderer>().material;
        m_initialColor = m_material.GetColor("_EmissionColor");
        m_lerpedColor = m_initialColor;
    }

    // Update is called once per frame
    void Update()
    {
        m_lerpedColor = Color.Lerp(m_initialColor, Color, Mathf.PingPong(Time.time * Speed, 1));
        m_material.SetColor("_EmissionColor", m_lerpedColor);
    }
}
