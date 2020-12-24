using UnityEngine;

public class TerrainPlane : MonoBehaviour
{
    public const int LAYER_MASK = 1 << 8;

    public GameObject Tree1Static;
    public GameObject Tree1;
    public GameObject WoodenWall;

    public static TerrainPlane current
    {
        get
        {
            return m_current;
        }
    }

    private static TerrainPlane m_current;

    void Awake()
    {
        m_current = this;
    }

    public void PlantStaticTree(float x, float z)
    {
        Instantiate(Tree1Static, new Vector3(x, Tree1Static.transform.position.y, z), Quaternion.Euler(-90.0f, Random.Range(0.0f, 360.0f), 0.0f));
    }

    public void PlantTree(float x, float z)
    {
        Instantiate(Tree1, new Vector3(x, Tree1.transform.position.y, z), Quaternion.Euler(-90.0f, Random.Range(0.0f, 360.0f), 0.0f));
    }

    public void CraftWoodenWall(float x, float z)
    {
        Instantiate(WoodenWall, new Vector3(x, WoodenWall.transform.position.y, z), Quaternion.identity);
    }
}
