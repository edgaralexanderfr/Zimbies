using UnityEngine;

public class DestroyAfter : MonoBehaviour
{
    #region[Purple] Settings
    public int Time;
    #endregion Settings

    // Start is called before the first frame update
    void Start()
    {
        Invoke("Remove", Time);
    }

    private void Remove()
    {
        Destroy(gameObject);
    }
}
