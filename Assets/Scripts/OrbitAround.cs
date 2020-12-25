using UnityEngine;

public class OrbitAround : MonoBehaviour
{
    #region[Purple] Settings
    public Vector3 Axis;
    public float Speed;
    public GameObject Target;
    #endregion Settings

    // Update is called once per frame
    void Update()
    {
        transform.RotateAround(Target.transform.position, Axis, Speed * Time.deltaTime);
    }
}
