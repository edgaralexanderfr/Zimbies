using UnityEngine;

public class God : MonoBehaviour
{
    public static God current { get { return m_current; } }

    private static God m_current;

    #region[Purple] Settings
    public GameObject Character;
    public GameObject Zombie1;
    public float ForestDensity;
    #endregion Settings

    void Awake()
    {
        m_current = this;
    }

    // Start is called before the first frame update
    void Start()
    {
        PlantForest();
        PlantInitialTrees();
    }

    public void SpawnCharacter(string name, float x, float z)
    {
        var character = Instantiate(Character, new Vector3(x, Character.transform.position.y, z), Quaternion.identity);
        character.name = name;
    }

    public void SpawnZombie(Zombie.Modes mode, float x, float z)
    {
        var zombieGameObject = Instantiate(Zombie1, new Vector3(x, Character.transform.position.y, z), Quaternion.identity);
        var zombie = zombieGameObject.GetComponent<Zombie>();

        if (zombie) zombie.Mode = mode;
    }

    // Plants the initial surrounding forest
    private void PlantForest()
    {
        float terrainSize = 300.0f;
        float treeSize = 20.0f;
        float forestSize = ForestDensity * treeSize;
        float x, z;

        for (x = -terrainSize - forestSize; x <= terrainSize + forestSize; x += treeSize)
        {
            for (z = -terrainSize - forestSize; z <= terrainSize + forestSize; z += treeSize)
            {
                if (x <= -terrainSize || x >= terrainSize || z <= -terrainSize || z >= terrainSize)
                {
                    if (Random.Range(0.0f, 1.0f) < 0.8f)
                    {
                        TerrainPlane.current.PlantStaticTree(x + 5.0f, z + 5.0f);
                    }
                }
            }
        }
    }

    // Experimental:
    private void PlantInitialTrees()
    {
        int i = 1;
        float x, z;

        while (i <= 64)
        {
            x = Random.Range(-30, 30) * 10.0f + 5.0f;
            z = Random.Range(-30, 30) * 10.0f + 5.0f;

            TerrainPlane.current.PlantTree(x, z);

            i++;
        }
    }
}
