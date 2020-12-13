using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestroyAfter : MonoBehaviour
{
    public int time;

    // Start is called before the first frame update
    void Start()
    {
        Invoke("Remove", time);
    }

    void Remove()
    {
        Destroy(gameObject);
    }
}
