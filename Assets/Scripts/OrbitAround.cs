using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OrbitAround : MonoBehaviour
{
    public Vector3 axis;
    public float speed;
    public GameObject target;

    // Update is called once per frame
    void Update()
    {
        transform.RotateAround(target.transform.position, axis, speed * Time.deltaTime);
    }
}
