using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameTree : MonoBehaviour
{
    public GameObject tree1Destroyed;

    private GameObject _characterGameObject;
    private Character _character;
    private GameObject _tree1Destroyed;

    // Update is called once per frame
    void Update()
    {
        if (_character && _character.meleeing)
        {
            _tree1Destroyed = Instantiate(tree1Destroyed, transform.position, transform.rotation);

            var halfTrunk = _tree1Destroyed.transform.Find("HalfTrunk");
            Physics.IgnoreCollision(_characterGameObject.GetComponent<Collider>(), halfTrunk.GetComponent<Collider>());

            var rigidbody = halfTrunk.GetComponent<Rigidbody>();
            rigidbody.AddForce(_characterGameObject.transform.forward * 25.0f, ForceMode.Impulse);

            // Increase character's wood in inventory:
            _character.inventory.wood++;

            Destroy(gameObject);
        }
    }

    void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            _characterGameObject = collision.gameObject;
            _character = collision.gameObject.GetComponent<Character>();
        }
    }

    void OnCollisionExit(Collision collision)
    {
        _characterGameObject = null;
        _character = null;
    }
}
