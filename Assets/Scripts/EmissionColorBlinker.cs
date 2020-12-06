using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EmissionColorBlinker : MonoBehaviour
{
    public Color color;
    public float speed;

    private Material _material;
    private Color _initialColor;
    private Color _lerpedColor;

    // Start is called before the first frame update
    void Start()
    {
        _material = GetComponent<Renderer>().material;
        _initialColor = _material.GetColor("_EmissionColor");
        _lerpedColor = _initialColor;
    }

    // Update is called once per frame
    void Update()
    {
        _lerpedColor = Color.Lerp(_initialColor, color, Mathf.PingPong(Time.time * speed, 1));
        _material.SetColor("_EmissionColor", _lerpedColor);
    }
}
