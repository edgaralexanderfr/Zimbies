using UnityEngine;

public class TerrainPlane : MonoBehaviour
{
    public const int LAYER_MASK = 1 << 8;
    public const int TERRAIN_SIZE = 96;
    public const int TERRAIN_HALF_SIZE = TERRAIN_SIZE / 2;

    public static TerrainPlane current { get { return m_current; } }

    private static TerrainPlane m_current;
    private static int[] m_fixedAngles = {0, 90, 180, 270};

    #region[Purple] Settings
    public GameObject Tree1;
    public GameObject Tree1Static;
    public GameObject Tree2;
    public GameObject Tree2Static;
    public GameObject WoodenWall;
    #endregion Settings

    #region[Blue] Private Members
    private GameObject[,] m_grid = new GameObject[TERRAIN_SIZE, TERRAIN_SIZE];
    #endregion Private Members

    void Awake()
    {
        m_current = this;
    }

    public bool IsPlaceOccupied(float x, float z)
    {
        int i = ((int) Mathf.Floor(x / 10.0f)) + TERRAIN_HALF_SIZE;
        int j = ((int) Mathf.Floor(z / 10.0f)) + TERRAIN_HALF_SIZE;

        return m_grid[i, j] != null;
    }

    public GameObject GetGameObjectAt(float x, float z)
    {
        int i = ((int) Mathf.Floor(x / 10.0f)) + TERRAIN_HALF_SIZE;
        int j = ((int) Mathf.Floor(z / 10.0f)) + TERRAIN_HALF_SIZE;

        return m_grid[i, j];
    }

    public void DestroyGameObject(GameObject lgameObject)
    {
        float x = lgameObject.transform.position.x;
        float z = lgameObject.transform.position.z;

        Destroy(lgameObject);
        SetGameObjectAt(null, x, z);
    }

    public GameObject PlantStaticTree(float x, float z, byte type = GameTree.TYPE_ANY)
    {
        GameObject lgameObject = GetTreeGameObjectFromType(type, true);

        return Instantiate(lgameObject, new Vector3(x, lgameObject.transform.position.y, z), Quaternion.Euler(-90.0f, Random.Range(0.0f, 360.0f), 0.0f));
    }

    public GameObject PlantTree(float x, float z, byte type = GameTree.TYPE_ANY)
    {
        GameObject tree = null;

        if (!IsPlaceOccupied(x, z))
        {
            GameObject lgameObject = GetTreeGameObjectFromType(type, false);

            tree = Instantiate(lgameObject, new Vector3(x, lgameObject.transform.position.y, z), Quaternion.Euler(-90.0f, Random.Range(0.0f, 360.0f), 0.0f));
            SetGameObjectAt(tree, x, z);
        }

        return tree;
    }

    public GameObject CraftWoodenWall(float x, float z)
    {
        GameObject woodenWall = null;

        if (!IsPlaceOccupied(x, z))
        {
            woodenWall = Instantiate(WoodenWall, new Vector3(x, WoodenWall.transform.position.y, z), Quaternion.Euler(-90.0f, m_fixedAngles[Random.Range(0, m_fixedAngles.Length - 1)], 0.0f));
            SetGameObjectAt(woodenWall, x, z);
        }

        return woodenWall;
    }

    private GameObject GetTreeGameObjectFromType(byte type, bool isStatic)
    {
        GameObject lgameObject;

        switch (type)
        {
            case GameTree.TYPE_PINE:
                lgameObject = isStatic ? Tree1Static : Tree1;
                break;
            case GameTree.TYPE_DECIDUOUS:
                lgameObject = isStatic ? Tree2Static : Tree2;
                break;
            default:
                if (Random.Range(0.0f, 1.0f) <= 0.65f)
                {
                    lgameObject = isStatic ? Tree1Static : Tree1;
                }
                else
                {
                    lgameObject = isStatic ? Tree2Static : Tree2;
                }
                break;
        }

        return lgameObject;
    }

    private void SetGameObjectAt(GameObject lgameObject, float x, float z)
    {
        int i = ((int) Mathf.Floor(x / 10.0f)) + TERRAIN_HALF_SIZE;
        int j = ((int) Mathf.Floor(z / 10.0f)) + TERRAIN_HALF_SIZE;

        m_grid[i, j] = lgameObject;
    }
}
