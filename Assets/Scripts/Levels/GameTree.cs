using UnityEngine;

public class GameTree : MonoBehaviour
{
    #region[Purple] Settings
    public GameObject DestroyedTree;
    #endregion Settings

    #region[Blue] Private Members
    private GameObject m_characterGameObject;
    private Character m_character;
    private GameObject m_destroyedTree;
    #endregion Private Members

    // Update is called once per frame
    void Update()
    {
        if (m_character && m_character.Meleeing)
        {
            m_destroyedTree = Instantiate(DestroyedTree, transform.position, transform.rotation);

            var halfTrunk = m_destroyedTree.transform.Find("HalfTrunk");
            Physics.IgnoreCollision(m_characterGameObject.GetComponent<Collider>(), halfTrunk.GetComponent<Collider>());

            var rigidbody = halfTrunk.GetComponent<Rigidbody>();
            rigidbody.AddForce(m_characterGameObject.transform.forward * 25.0f, ForceMode.Impulse);

            // Increase character's wood in inventory:
            m_character.Inventory.Wood++;

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
