using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScreenController : MonoBehaviour
{
    int m_numPlayers;

    [SerializeField]
    GameObject m_cameraPrefab;
    [SerializeField]
    RectTransform m_barPrefab;

    [SerializeField]
    List<GameObject> m_cameras;

    [SerializeField]
    Canvas m_canvas;

    float m_border = 0.005f;

    bool m_isSizeTransitioning = false;
    float m_transitionSpeed = 2.0f;

    float m_viewportX;

    List<RectTransform> m_bars;

    // Use this for initialization
    void Awake ()
    {
        Debug.Log("Assigning Camera List");
        m_cameras = new List<GameObject>();
        m_bars = new List<RectTransform>();
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
            m_cameras[0].GetComponent<CameraController>().setOrthoSize(30, 1.0f);
        }
        if (Input.GetKeyDown("4"))
        {
            removeViewport(m_cameras[Random.Range(0, m_cameras.Count - 1)]);
        }
        if (Input.GetKeyDown("space"))
        {
            m_cameras[0].GetComponent<CameraController>().addTrauma(0.2f);
        }

        for(int i = 0; i < m_cameras.Count; i++)
        {
            Rect r = m_cameras[i].GetComponent<Camera>().rect;
            float canvasWidth = m_canvas.pixelRect.width, canvasHeight = m_canvas.pixelRect.height, barWidth = m_border * canvasWidth;
            m_bars[i].position = new Vector3(r.x * canvasWidth + r.width * canvasWidth + barWidth / 2, canvasHeight / 2, 0.0f);
            m_bars[i].sizeDelta = new Vector2(barWidth, canvasHeight);
        }
    }

    public void setViewports()
    {
        StopCoroutine("addLerpViewportReset");
        StopCoroutine("removeLerpViewportReset");
        float viewportWidth = (1.0f - m_border) / (m_cameras.Count);
        float viewportX = 0.0f;

        viewportX = 0.0f;
        for (int i = 0; i < m_cameras.Count; ++i)
        {
            m_cameras[i].GetComponent<Camera>().rect = new Rect(viewportX, 0, viewportWidth, 1);
            viewportX = viewportX + viewportWidth + m_border;
        }
    }


    public Camera addViewport()
    {
        // add a new camera to the list through Instantiate
        m_cameras.Add(Instantiate(m_cameraPrefab));
        int newestCamera = m_cameras.Count - 1;
        m_cameras[newestCamera].name = "Player_" + newestCamera + "_Camera";

        m_bars.Add(Instantiate(m_barPrefab, m_canvas.transform));

        StartCoroutine("addLerpViewportReset");

        return m_cameras[newestCamera].GetComponent<Camera>();
    }

    public void removeViewport(GameObject _camera)
    {
        if(m_cameras.Count > 1)
        {
            gameObject.GetComponent<UIController>().removeCanvasByCamera(_camera);
            Destroy(_camera);
            m_cameras.Remove(_camera);
            StopCoroutine("removeLerpViewportReset");
            StartCoroutine("removeLerpViewportReset", _camera);
        }
    }

    IEnumerator addLerpViewportReset()
    {
        m_isSizeTransitioning = true;

        float elapsedTime = 0.0f;
        float startViewportWidth = (1.0f - m_border) / (m_cameras.Count - 1);
        float endViewportWidth = (1.0f - m_border) / (m_cameras.Count);
        float viewportX = 0.0f;

        while (elapsedTime < m_transitionSpeed)
        {
            viewportX = 0.0f;
            for (int i = 0; i < m_cameras.Count; ++i)
            {
                float lerpedWidth = Mathf.SmoothStep(startViewportWidth, endViewportWidth, (elapsedTime / m_transitionSpeed));

                m_cameras[i].GetComponent<Camera>().rect = new Rect(viewportX, 0, lerpedWidth, 1);
                viewportX = viewportX + lerpedWidth + m_border;
                elapsedTime += Time.deltaTime;
            }

            yield return new WaitForEndOfFrame();
        }

        m_isSizeTransitioning = false;
    }

    // I dont know how this works but it does. Don't change or risk breaking all viewports!
    IEnumerator removeLerpViewportReset()
    {
        m_isSizeTransitioning = true;

        float elapsedTime = 0.0f;
        float startViewportWidth = (1.0f - m_border) / (m_cameras.Count + 1);
        float endViewportWidth = (1.0f - m_border) / (m_cameras.Count);

        float endViewportX = 0.0f;

        List<float> cameraStartViewportX = new List<float>();
        for(int i = 0; i < m_cameras.Count; ++i)
        {
            cameraStartViewportX.Add(m_cameras[i].GetComponent<Camera>().rect.x);
            Debug.Log(cameraStartViewportX[i]);
        }

        while (elapsedTime < m_transitionSpeed)
        {
            endViewportX = 0.0f;

            // for each camera
            for (int i = 0; i < m_cameras.Count; ++i)
            {
                float lerpedWidth = Mathf.SmoothStep(startViewportWidth, endViewportWidth, (elapsedTime / m_transitionSpeed));
                float lerpedViewportX = Mathf.SmoothStep(cameraStartViewportX[i], endViewportX, (elapsedTime / m_transitionSpeed));

                m_cameras[i].GetComponent<Camera>().rect = new Rect(lerpedViewportX, 0, lerpedWidth, 1);
                endViewportX = endViewportX + endViewportWidth + m_border;
                elapsedTime += Time.deltaTime;

                gameObject.GetComponent<UIController>().updateCanvases();
            }

            yield return new WaitForEndOfFrame();
        }

        m_isSizeTransitioning = false;
    }
}
