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
        if (GameData.g_clusterMode)
        {
            PlayerController[] players = FindObjectsOfType<PlayerController>();
            float lowest = players[0].transform.position.y;
            player = players[0].gameObject;
            foreach (PlayerController p in players)
            {
                if (p.transform.position.y < lowest)
                {
                    lowest = p.transform.position.y;
                    player = p.gameObject;
                }
            }
        }
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
            if(!GameData.g_clusterMode || FindObjectsOfType<PlayerController>().Length <= 1)
                Destroy(gameObject);
        }
    }
}
