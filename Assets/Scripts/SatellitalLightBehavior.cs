using UnityEngine;

public class SatellitalLightBehavior : MonoBehaviour
{
    #region[Purple] Settings
    public bool DisableOnGround = true;
    public Light TargetLight;
    #endregion Settings

    // Update is called once per frame
    void Update()
    {
        if (DisableOnGround && transform.position.y < 0.0f) {
            TargetLight.enabled = false;
        } else {
            TargetLight.enabled = true;
        }
    }
}
