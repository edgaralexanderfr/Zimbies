using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameWall : MonoBehaviour
{
    public GameObject woodenWallDestroyed;

    private GameObject _characterGameObject;
    private Character _character;
    private GameObject _woodenWallDestroyed;

    // Update is called once per frame
    void Update()
    {
        if (_character && _character.meleeing)
        {
            Instantiate(woodenWallDestroyed, transform.position, transform.rotation);
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
