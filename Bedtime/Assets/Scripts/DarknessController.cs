using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DarknessController : MonoBehaviour
{
    public GameObject player;

    // Use this for initialization
    void Awake()
    {

        transform.position = new Vector3(0, -60, 0);
    }

    // Update is called once per frame
    void Update()
    {
        gameObject.transform.position = new Vector3(player.transform.position.x, transform.position.y, player.transform.position.z);
        gameObject.transform.Translate(new Vector3(0, 5, 0) * Time.deltaTime);
    }

    void OnTriggerEnter(Collider _collision)
    {
        GameObject other = _collision.gameObject;
        if (other.tag == "Player")
        {
            Debug.Log("Killed Player " + other.GetComponent<PlayerController>().m_playerNumber);
            other.GetComponent<PlayerController>().kill();
        }
    }
}
