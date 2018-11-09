using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnController : MonoBehaviour
{
    [SerializeField]
    GameObject m_playerGameObject;
    [SerializeField]
    GameObject m_startLevel;
    List<GameObject> m_spawnPoints;

    [SerializeField]
    public int playerCount = 2;

    void Start()
    {
        playerCount = NumberOfPlayers.numberOfPlayers;

        for(int i = 0; i < playerCount; i++)
            Instantiate(m_startLevel, new Vector3(i * 100, 0, 0), Quaternion.identity);

        m_spawnPoints = new List<GameObject>();
        // look through all objects and find any that are spawn points and add them to the list of spawn points
        m_spawnPoints.AddRange(GameObject.FindGameObjectsWithTag("SpawnPoint"));

        if(playerCount > 4 || playerCount < 0)
        {
            Debug.LogError("Invalid number of players, must be > 0 and < 5");
            playerCount = 2;
        }

        if(m_spawnPoints.Count < playerCount)
        {
            Debug.LogError("Not enough spawn points for the number of players!");
            playerCount = m_spawnPoints.Count;
        }

        if (m_spawnPoints.Count <= 0)
        {
            Debug.LogError("No player spawn points found");
        }

        for(int i = 0; i < playerCount; ++i)
        {
            spawnPlayer();
        }

        // set the viewports instantly, no smoothing
        gameObject.GetComponent<ScreenController>().setViewports();
        gameObject.GetComponent<UIController>().updateCanvases();


    }

    void Update()
    {

    }

    GameObject spawnPlayer()
    {
        int spawnPointIndex = Random.Range(0, m_spawnPoints.Count - 1);

        GameObject player = Instantiate(m_playerGameObject);

        player.transform.position = m_spawnPoints[spawnPointIndex].transform.position;
        m_spawnPoints.RemoveAt(spawnPointIndex);

        // add a viewport for the player
        player.GetComponent<PlayerController>().m_camera = gameObject.GetComponent<ScreenController>().addViewport();
        // add a canvas for a new viewport
        player.GetComponent<PlayerController>().m_canvas = gameObject.GetComponent<UIController>().addCameraCanvas(player.GetComponent<PlayerController>().m_camera);

        GetComponent<GamePlayController>().registerPlayer(player.GetComponent<PlayerController>());
         
        return player;
    }
}
