using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class God : MonoBehaviour
{
    public GameObject tree1;

    // Start is called before the first frame update
    void Start()
    {
        PlantForest();
        PlantInitialTrees();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    // Plants the initial surrounding forest
    void PlantForest()
    {
        float x, z;

        for (x = 320.0f, z = 320.0f; x >= -320.0f; x -= 20.0f)
        {
            PlantTree(x, z);
        }

        for (x = -320.0f, z = -320.0f; x <= 320.0f; x += 20.0f)
        {
            PlantTree(x, z);
        }

        for (x = 320.0f, z = 300.0f; z >= -300.0f; z -= 20.0f)
        {
            PlantTree(x, z);
        }

        for (x = -320.0f, z = 300.0f; z >= -300.0f; z -= 20.0f)
        {
            PlantTree(x, z);
        }
    }

    // Experimental:
    void PlantInitialTrees()
    {
        int i = 1;
        float x, z;

        while (i <= 64)
        {
            x = Random.Range(-30.0f, 30.0f) * 10.0f;
            z = Random.Range(-30.0f, 30.0f) * 10.0f;

            PlantTree(x, z);

            i++;
        }
    }

    void PlantTree(float x, float z)
    {
        Instantiate(tree1, new Vector3(x, 7.98f, z), Quaternion.Euler(-90.0f, Random.Range(0.0f, 360.0f), 0.0f));
    }
}
