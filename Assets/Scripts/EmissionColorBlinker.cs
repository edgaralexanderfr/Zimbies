using UnityEngine;

public class EmissionColorBlinker : MonoBehaviour
{
    #region[Purple] Settings
    public Color Color;
    public Color TargetColor;
    public float Speed;
    #endregion Settings

    #region[Blue] Private Members
    private Material m_material;
    private Color m_lerpedColor;
    #endregion Private Members

    // Start is called before the first frame update
    void Start()
    {
        m_material = GetComponent<Renderer>().material;
    }

    // Update is called once per frame
    void Update()
    {
        m_lerpedColor = Color.Lerp(Color, TargetColor, Mathf.PingPong(Time.time * Speed, 1));
        m_material.SetColor("_EmissionColor", m_lerpedColor);
    }

    public void SetColors(Color color, Color targetColor)
    {
        Color = color;
        TargetColor = targetColor;
        m_lerpedColor = color;
        m_material?.SetColor("_EmissionColor", color);
    }
}
