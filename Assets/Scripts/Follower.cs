using UnityEngine;

public class Follower : MonoBehaviour
{
    public static Follower current { get { return m_current; } }

    private static Follower m_current;

    #region[Purple] Settings
    public GameObject FollowerObject;
    public GameObject Target;
    public Vector3 Offset;
    #endregion Settings

    void Awake()
    {
        m_current = this;
    }

    // Update is called once per frame
    void Update()
    {
        var newPosition = Target.transform.position + Offset;
        FollowerObject.transform.position = newPosition;
    }
}
