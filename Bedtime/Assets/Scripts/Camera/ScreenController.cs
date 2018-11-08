using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScreenController : MonoBehaviour
{
    int m_numPlayers;

    [SerializeField]
    GameObject m_cameraPrefab;

    //[SerializeField]
    List<GameObject> m_cameras;

    [SerializeField]
    Canvas m_canvas;

    float m_border = 0.005f;


    // Use this for initialization
    void Awake ()
    {
        Debug.Log("Assigning Camera List");
        m_cameras = new List<GameObject>();
	}
	
	// Update is called once per frame
	void Update ()
    {
        if (Input.GetKeyDown("1"))
        {
            m_cameras[0].GetComponent<CameraController>().setOrthoSize(10, 1.0f);
        }
        if (Input.GetKeyDown("2"))
        {
            removeViewport(0);
        }
        if (Input.GetKeyDown("3"))
        {
            m_cameras[0].GetComponent<CameraController>().setOrthoSize(30, 1.0f);
        }
    }

    public Camera addViewport()
    {
        // add a new camera to the list through Instantiate
        m_cameras.Add(Instantiate(m_cameraPrefab));
        int newestCamera = m_cameras.Count - 1;
        m_cameras[newestCamera].name = "Player_" + newestCamera + "_Camera";


        resetViewportLayout();

        Debug.Log("Number of Cameras: " + m_cameras.Count);
        return m_cameras[newestCamera].GetComponent<Camera>();
    }

    public void removeViewport(int _index)
    {
        m_cameras.RemoveAt(_index);
        resetViewportLayout();
    }

    void resetViewportLayout()
    {
        float viewportX = 0.0f;

        float border = m_border;
        if(m_cameras.Count <= 1)
        {
            border = 0.0f;
        }
        float viewportWidth = (1.0f - border) / (m_cameras.Count);

        // loop through all cameras to space them all out now there is a new one
        for (int i = 0; i < m_cameras.Count; ++i)
        {
            // grab the camera component
            Camera currentCamera = m_cameras[i].GetComponent<Camera>();
            // set the viewport position
            currentCamera.rect = new Rect(viewportX, 0, viewportWidth, 1);

            


            // offset the viewport X for the next camera
            viewportX = viewportX + viewportWidth + border;
        }

        GL.Clear(true, true, new Color(0, 0, 0, 1), 1.0f);
    }
}
