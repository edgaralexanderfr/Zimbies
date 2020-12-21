using UnityEngine;

public class TerrainPlane : MonoBehaviour
{
    public const int LAYER_MASK = 1 << 8;

    public static TerrainPlane current;

    public GameObject tree1Static;
    public GameObject tree1;
    public GameObject woodenWall;

    void Awake()
    {
        current = this;
    }

    public void PlantStaticTree(float x, float z)
    {
        Instantiate(tree1Static, new Vector3(x, tree1Static.transform.position.y, z), Quaternion.Euler(-90.0f, Random.Range(0.0f, 360.0f), 0.0f));
    }

    public void PlantTree(float x, float z)
    {
        Instantiate(tree1, new Vector3(x, tree1.transform.position.y, z), Quaternion.Euler(-90.0f, Random.Range(0.0f, 360.0f), 0.0f));
    }

    public void CraftWoodenWall(float x, float z)
    {
        Instantiate(woodenWall, new Vector3(x, woodenWall.transform.position.y, z), Quaternion.identity);
    }
}
