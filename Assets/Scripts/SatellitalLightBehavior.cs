using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SatellitalLightBehavior : MonoBehaviour
{
    public Light targetLight;
    public bool disableOnGround = true;

    // Update is called once per frame
    void Update()
    {
        if (disableOnGround && transform.position.y < 0.0f) {
            targetLight.enabled = false;
        } else {
            targetLight.enabled = true;
        }
    }
}
