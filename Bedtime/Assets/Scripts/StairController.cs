using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StairController : MonoBehaviour {

    public GameObject m_winLevel;
    public GameObject m_stairPrefab;
    public GameObject[] m_levels;

    public float m_floorCamSize = 18, m_stairCamSize = 8;

    public GameObject m_myLevel;
    public int m_lastLevel = -1;

    private List<GameObject> m_stairs;
    private GameObject m_previous;
    private PlayerController m_player;
    private bool m_old = false, m_playerSet = false;

	void Start ()
    {
        m_stairs = new List<GameObject>();
        spawnStair();
	}
	
	void Update ()
    {
        //Find closest player if m_player isn't set
        if(!m_player)
        {
            if (!m_playerSet)
            {
                PlayerController[] chars = FindObjectsOfType<PlayerController>();
                if (chars.Length == 0)
                    Debug.LogError("No players!");
                float distance = 0;
                foreach (PlayerController c in chars)
                {
                    float d = 0;
                    if (!m_player)
                    {
                        m_player = c;
                        distance = Vector3.Distance(c.transform.position, transform.position);
                    }
                    else if ((d = Vector3.Distance(c.transform.position, transform.position)) < distance)
                    {
                        m_player = c;
                        distance = d;
                    }
                }
            }
            else
                return;
        }

        //Based on y difference, spawn stairs or spawn a new level
        float stairHeight = m_previous.transform.position.y, playerHeight = m_player.transform.position.y;
        if(m_old)
        {
            if (playerHeight < stairHeight)
                return;

            foreach (GameObject stair in m_stairs)
                Destroy(stair);

            if (m_myLevel)
                Destroy(m_myLevel);
            else
                Destroy(this);
            return;
        }
        if (playerHeight > transform.position.y + 5)
        {
            m_player.m_camera.GetComponent<CameraController>().setOrthoSize(m_stairCamSize, 0.2f);
            if (stairHeight - playerHeight < m_player.m_camera.orthographicSize / 2)
                spawnStair();
            if (playerHeight > transform.position.y + 70)
            {
                if (m_levels.Length == 0)
                {
                    Debug.LogError("No levels associated with this staircase!");
                    Destroy(this);
                    return;
                }

                //Check win condition
                if (FindObjectOfType<ScreenController>().getCameraCount() == 1)
                {
                    Vector3 p = new Vector3();
                    foreach (Transform t in m_previous.GetComponentsInChildren<Transform>())
                        if (t.CompareTag("EndOfStairs"))
                            p = t.position;

                    GameObject ob = Instantiate(m_winLevel, p, Quaternion.Euler(m_previous.transform.rotation.eulerAngles - new Vector3(0.0f, 90.0f, 0.0f)));
                    foreach (Transform t in ob.GetComponentsInChildren<Transform>())
                        if (t.CompareTag("StartOfStairs"))
                            p = t.position;

                    ob.transform.position = ob.transform.position + (ob.transform.position - p);
                }

                int level = Random.Range(0, m_levels.Length);
                while (level == m_lastLevel) { level = Random.Range(0, m_levels.Length); Debug.Log("Choosing Level"); }
                Debug.Log("New level! Using level " + level + " out of " + m_levels.Length + ", last level was " + m_lastLevel);
                m_lastLevel = level;

                Vector3 pos = new Vector3();
                foreach (Transform t in m_previous.GetComponentsInChildren<Transform>())
                    if (t.CompareTag("EndOfStairs"))
                        pos = t.position;

                GameObject o = Instantiate(m_levels[level], pos, Quaternion.Euler(m_previous.transform.rotation.eulerAngles - new Vector3(0.0f, 90.0f, 0.0f)));
                foreach (Transform t in o.GetComponentsInChildren<Transform>())
                    if (t.CompareTag("StartOfStairs"))
                        pos = t.position;

                o.transform.position = o.transform.position + (o.transform.position - pos);
                o.GetComponentInChildren<StairController>().m_levels = m_levels;
                o.GetComponentInChildren<StairController>().m_myLevel = o;
                o.GetComponentInChildren<StairController>().m_lastLevel = m_lastLevel;
                o.GetComponentInChildren<StairController>().m_winLevel = m_winLevel;

                m_previous = o;
                m_old = true;
            }
        }
        else
            m_player.m_camera.GetComponent<CameraController>().setOrthoSize(m_floorCamSize, 0.2f);
        
	}

    void spawnStair()
    {
        if (!m_previous)
        {
            Vector3 startPos = new Vector3();
            foreach (Transform t in GetComponentsInChildren<Transform>())
                if (t.CompareTag("EndOfStairs"))
                    startPos = t.position;
            m_previous = Instantiate(m_stairPrefab, startPos, transform.rotation);
        }
        else
        {
            Vector3 spawnPos = m_previous.transform.position + new Vector3(10.0f, 10.0f, 10.0f);
            foreach (Transform t in m_previous.GetComponentsInChildren<Transform>())
                if (t.CompareTag("EndOfStairs"))
                    spawnPos = t.position;
            m_previous = Instantiate(m_stairPrefab, spawnPos, m_previous.transform.rotation);
        }
        m_stairs.Add(m_previous);
    }
}
