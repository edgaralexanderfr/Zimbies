using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class God : MonoBehaviour
{
    public float forestDensity;
    public GameObject tree1Static;
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
        float terrainSize = 300.0f;
        float treeSize = 20.0f;
        float forestSize = forestDensity * treeSize;
        float x, z;

        for (x = -terrainSize - forestSize; x <= terrainSize + forestSize; x += treeSize)
        {
            for (z = -terrainSize - forestSize; z <= terrainSize + forestSize; z += treeSize)
            {
                if (x <= -terrainSize || x >= terrainSize || z <= -terrainSize || z >= terrainSize)
                {
                    if (Random.Range(0.0f, 1.0f) < 0.8f)
                    {
                        PlantStaticTree(x, z);
                    }
                }
            }
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

    void PlantStaticTree(float x, float z)
    {
        Instantiate(tree1Static, new Vector3(x, tree1Static.transform.position.y, z), Quaternion.Euler(-90.0f, Random.Range(0.0f, 360.0f), 0.0f));
    }

    public void PlantTree(float x, float z)
    {
        Instantiate(tree1, new Vector3(x, tree1.transform.position.y, z), Quaternion.Euler(-90.0f, Random.Range(0.0f, 360.0f), 0.0f));
    }
}
