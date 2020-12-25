using UnityEngine;

public class GameWall : MonoBehaviour
{
    #region[Purple] Settings
    public GameObject DestroyedWall;
    #endregion Settings

    #region[Blue] Private Members
    private GameObject m_characterGameObject;
    private Character m_character;
    private GameObject m_woodenWallDestroyed;
    #endregion Private Members

    // Update is called once per frame
    void Update()
    {
        if (m_character && m_character.Meleeing)
        {
            Instantiate(DestroyedWall, transform.position, transform.rotation);
            Destroy(gameObject);
        }
    }

    void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            m_characterGameObject = collision.gameObject;
            m_character = collision.gameObject.GetComponent<Character>();
        }
    }

    void OnCollisionExit(Collision collision)
    {
        m_characterGameObject = null;
        m_character = null;
    }
}
