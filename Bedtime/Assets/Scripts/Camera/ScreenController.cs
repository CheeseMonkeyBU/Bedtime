using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScreenController : MonoBehaviour
{
    int m_numPlayers;

    [SerializeField]
    GameObject m_cameraPrefab;
    [SerializeField]
    List<GameObject> m_cameras;
    [SerializeField]
    Canvas m_canvas;


    // Use this for initialization
    void Start ()
    {
        if(m_numPlayers <= 0 || m_numPlayers > 4)
        {
            m_numPlayers = 1;
        }

        m_numPlayers = gameObject.GetComponent<SpawnController>().playerCount;

        m_cameras = new List<GameObject>();

        float viewportX = 0.0f;
        float viewportWidth = 1.0f / m_numPlayers;

        for(int i = 0; i < m_numPlayers; ++i)
        {
            // add a new camera to the list through Instantiate
            m_cameras.Add(Instantiate(m_cameraPrefab));
            // name the camera
            m_cameras[i].name = "Player_" + i + "_Camera";
            // grab the camera component
            Camera currentCamera = m_cameras[i].GetComponent<Camera>();
            // set the viewport position
            currentCamera.rect = new Rect(viewportX, 0, viewportWidth, 1);


            // offset the viewport X for the next camera
            viewportX += viewportWidth;
        }
	}
	
	// Update is called once per frame
	void Update ()
    {
        if (Input.GetKeyDown("1"))
        {
            m_cameras[0].GetComponent<CameraController>().setOrthoSize(10, 1.0f);
            m_cameras[1].GetComponent<CameraController>().setOrthoSize(20, 1.0f);
            m_cameras[2].GetComponent<CameraController>().setOrthoSize(30, 1.0f);
        }
        if (Input.GetKeyDown("2"))
        {
            m_cameras[0].GetComponent<CameraController>().setOrthoSize(30, 1.0f);
            m_cameras[1].GetComponent<CameraController>().setOrthoSize(10, 1.0f);
            m_cameras[2].GetComponent<CameraController>().setOrthoSize(20, 1.0f);
        }
        if (Input.GetKeyDown("3"))
        {
            m_cameras[0].GetComponent<CameraController>().setOrthoSize(20, 1.0f);
            m_cameras[1].GetComponent<CameraController>().setOrthoSize(30, 1.0f);
            m_cameras[2].GetComponent<CameraController>().setOrthoSize(10, 1.0f);
        }
    }
}
