using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnController : MonoBehaviour
{
    [SerializeField]
    GameObject m_playerGameObject;
    List<GameObject> m_spawnPoints;

    [SerializeField]
    public int playerCount = 2;

    void Start()
    {
        m_spawnPoints = new List<GameObject>();
        // look through all objects and find any that are spawn points and add them to the list of spawn points
        m_spawnPoints.AddRange(GameObject.FindGameObjectsWithTag("SpawnPoint"));

        if (m_spawnPoints.Count <= 0)
        {
            Debug.LogError("No player spawn points found");
        }

        for(int i = 0; i < playerCount; ++i)
        {
            spawnPlayer();
        }

        gameObject.GetComponent<ScreenController>().setViewports();
    }

    void Update()
    {

    }

    GameObject spawnPlayer()
    {
        int spawnPointIndex = Random.Range(0, m_spawnPoints.Count - 1);

        GameObject player = Instantiate(m_playerGameObject);

        player.transform.position = m_spawnPoints[spawnPointIndex].transform.position;
        //m_spawnPoints.RemoveAt(spawnPointIndex);

        player.GetComponent<PlayerController>().m_camera = gameObject.GetComponent<ScreenController>().addViewport();

        return player;
    }
}
