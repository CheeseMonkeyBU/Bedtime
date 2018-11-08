﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StairController : MonoBehaviour {

    public GameObject m_stairPrefab;
    public List<GameObject> m_levels;

    public float m_floorCamSize = 18, m_stairCamSize = 8;

    private GameObject m_previous;
    private PlayerController m_player;

	void Start ()
    {
        spawnStair();
	}
	
	void Update ()
    {
        if(!m_player)
        {
            PlayerController[] chars = FindObjectsOfType<PlayerController>();
            if(chars.Length == 0)
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

        float stairHeight = m_previous.transform.position.y, playerHeight = m_player.transform.position.y;
        if(playerHeight > transform.position.y + 5)
        {
            m_player.m_camera.GetComponent<CameraController>().setOrthoSize(m_stairCamSize, 0.2f);
            if (stairHeight - playerHeight < m_player.m_camera.orthographicSize / 2)
                spawnStair();
            if(playerHeight > transform.position.y + 70)
            {
                int level = Random.Range(0, m_levels.Count - 1);
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
                Destroy(gameObject);
            }
        }
        else
        {
            m_player.m_camera.GetComponent<CameraController>().setOrthoSize(m_floorCamSize, 0.2f);
        }
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
    }
}
