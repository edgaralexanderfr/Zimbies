using UnityEngine;

public class GameTree : MonoBehaviour
{
    #region[Purple] Settings
    public GameObject DestroyedTree;
    #endregion Settings

    #region[Blue] Private Members
    private GameObject m_characterGameObject;
    private GameObject m_zombieGameObject;
    private Character m_character;
    private Zombie m_zombie;
    private GameObject m_destroyedTree;
    #endregion Private Members

    // Update is called once per frame
    void Update()
    {
        if ((m_character && m_character.Meleeing) || (m_zombie && m_zombie.Attacking))
        {
            var lgameObject = m_character ? m_characterGameObject : m_zombieGameObject;

            m_destroyedTree = Instantiate(DestroyedTree, transform.position, transform.rotation);

            var halfTrunk = m_destroyedTree.transform.Find("HalfTrunk");
            Physics.IgnoreCollision(lgameObject.GetComponent<Collider>(), halfTrunk.GetComponent<Collider>());

            var rigidbody = halfTrunk.GetComponent<Rigidbody>();
            rigidbody.AddForce(lgameObject.transform.forward * 25.0f, ForceMode.Impulse);

            // Increase character's wood in inventory:
            if (m_character) m_character.Inventory.Wood++;

            Destroy(gameObject);
        }
    }

    void OnCollisionEnter(Collision collision)
    {
        // FIXME: Handle multiple entities at a time...
        switch (collision.gameObject.tag)
        {
            case "Player":
                m_characterGameObject = collision.gameObject;
                m_character = collision.gameObject.GetComponent<Character>();
                break;
            case "Enemy":
                m_zombieGameObject = collision.gameObject;
                m_zombie = collision.gameObject.GetComponent<Zombie>();
                break;
        }
    }

    void OnCollisionExit(Collision collision)
    {
        // FIXME: Handle multiple entities at a time...
        switch (collision.gameObject.tag)
        {
            case "Player":
                m_characterGameObject = null;
                m_character = null;
                break;
            case "Enemy":
                m_zombieGameObject = null;
                m_zombie = null;
                break;
        }
    }
}
