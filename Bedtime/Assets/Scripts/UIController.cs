using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIController : MonoBehaviour {

    [SerializeField]
    public Canvas m_screenCanvas;

    List<GameObject> m_playerCanvas;

    [SerializeField]
    GameObject m_playerCanvasPrefab;

    // Use this for initialization
    void Awake ()
    {
        m_playerCanvas = new List<GameObject>();
	}
	
	// Update is called once per frame
	void Update ()
    {
		
	}

    public Canvas addCameraCanvas(Camera _camera)
    {
        m_playerCanvas.Add(Instantiate(m_playerCanvasPrefab));
        GameObject currentCanvasGO = m_playerCanvas[m_playerCanvas.Count - 1];
        // set the canvas for the camera
        _camera.GetComponent<CameraController>().canvas = currentCanvasGO;
        // make this canvas a child of the main screen canvas
        currentCanvasGO.transform.SetParent(m_screenCanvas.transform);
        // set the name
        currentCanvasGO.name = "Canvas" + m_playerCanvas.Count;

        currentCanvasGO.GetComponent<CanvasController>().camera = _camera.gameObject;

        Canvas currentCanvasComponent = currentCanvasGO.GetComponent<Canvas>();
        RectTransform newCanvasRect = currentCanvasComponent.GetComponent<RectTransform>();
        RectTransform mainCanvasRect = m_screenCanvas.GetComponent<RectTransform>();

        // error check for infinity causing a NaN
        float cameraRectWidth = _camera.rect.width;
        if (cameraRectWidth == Mathf.Infinity)
        {
            cameraRectWidth = 1;
        }

        newCanvasRect.sizeDelta = new Vector2(mainCanvasRect.sizeDelta.x * cameraRectWidth, mainCanvasRect.sizeDelta.y);
        // set pivot (from parent canvas)
        newCanvasRect.pivot = new Vector2(0, 0);
        // set position (from camera)
        newCanvasRect.position = new Vector3(mainCanvasRect.sizeDelta.x * _camera.rect.x, 0, 0);

        return currentCanvasComponent;
    }

    public void removeCanvasByCamera(GameObject _camera)
    {
        GameObject canvasToRemove = _camera.GetComponent<CameraController>().canvas;
        m_playerCanvas.Remove(canvasToRemove);
        Destroy(canvasToRemove);
    }

    public void removeCanvasByPlayer(GameObject _player)
    {
        GameObject canvasToRemove = _player.GetComponent<PlayerController>().getCanvasGameObject();
        m_playerCanvas.Remove(canvasToRemove);
        Destroy(canvasToRemove);
    }

    public void updateCanvases()
    {
        for(int i = 0; i < m_playerCanvas.Count; ++i)
        {
            
            Canvas currentCanvasComponent = m_playerCanvas[i].GetComponent<Canvas>();
            RectTransform newCanvasRect = currentCanvasComponent.GetComponent<RectTransform>();
            RectTransform mainCanvasRect = m_screenCanvas.GetComponent<RectTransform>();
            Camera camera = m_playerCanvas[i].GetComponent<CanvasController>().camera.GetComponent<Camera>();
            // set size (from parent canvas)
            newCanvasRect.sizeDelta = new Vector2(mainCanvasRect.sizeDelta.x * camera.rect.width, mainCanvasRect.sizeDelta.y);
            // set pivot (from parent canvas)
            newCanvasRect.pivot = new Vector2(0, 0);
            // set position (from camera)
            newCanvasRect.position = new Vector3(mainCanvasRect.sizeDelta.x * camera.rect.x, 0, 0);
        }
    }

    public RectTransform getWinPanel()
    {
        return GetComponentInChildren<Image>().rectTransform;
    }
}
