using UnityEngine;

public class DestroyAfter : MonoBehaviour
{
    #region[Purple] Settings
    public float Time = 3.0f;
    public float FadeSpeed = 2.0f;
    #endregion Settings

    #region[Blue] Private Members
    private Renderer[] m_renderers;
    private Color[] m_targetColors;
    private float m_time = 0;
    #endregion Private Members

    // Start is called before the first frame update
    void Start()
    {
        StartColors();
    }

    private void StartColors()
    {
        Transform[] transforms = gameObject.GetComponentsInChildren<Transform>();
        m_renderers = new Renderer[transforms.Length];
        m_targetColors = new Color[transforms.Length];
        int i = 0;

        foreach (Transform ltransform in transforms)
        {
            var renderer = ltransform.GetComponent<Renderer>();
            m_renderers[i] = renderer;

            if (renderer)
            {
                m_targetColors[i] = new Color(renderer.material.color.r, renderer.material.color.g, renderer.material.color.b, 0.0f);
            }

            i++;
        }
    }

    void Update()
    {
        UpdateTime();
        UpdateColors();
    }

    private void UpdateTime()
    {
        if (m_time >= Time) Destroy(gameObject);

        m_time += UnityEngine.Time.deltaTime;
    }

    private void UpdateColors()
    {
        int i = 0;

        foreach (Renderer renderer in m_renderers)
        {
            if (renderer)
            {
                renderer.material.color = Color.Lerp(renderer.material.color, m_targetColors[i], FadeSpeed * UnityEngine.Time.deltaTime);
            }

            i++;
        }
    }
}
