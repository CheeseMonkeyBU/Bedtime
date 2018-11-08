using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    Camera m_cameraComponent;

    // size lerp members
    bool m_isSizeTransitioning = false;
    float m_zoomSpeed = 1.0f;

	// Use this for initialization
	void Start ()
    {
        m_cameraComponent = gameObject.GetComponent<Camera>();

        // transform it to a default position
        gameObject.transform.position = new Vector3(0, 20, 0);
        // set the orthographic size
        m_cameraComponent.orthographicSize = 10;
    }
	
	// Update is called once per frame
	void Update ()
    {
		
	}

    public void setOrthoSize(float _targetSize, float _speed)
    {
        if(!m_isSizeTransitioning)
        {
            StartCoroutine("lerpOrtho", _targetSize);
        }
        else
        {
            StopCoroutine("lerpOrtho");
            m_isSizeTransitioning = false;
            StartCoroutine("lerpOrtho", _targetSize);
        }
    }

    IEnumerator lerpOrtho(float _targetSize)
    {
        m_isSizeTransitioning = true;

        float elapsedTime = 0.0f;
        float startOrthoSize = m_cameraComponent.orthographicSize;

        while (elapsedTime < m_zoomSpeed)
        {
            m_cameraComponent.orthographicSize = Mathf.SmoothStep(startOrthoSize, _targetSize, (elapsedTime / m_zoomSpeed));
            elapsedTime += Time.deltaTime;
            yield return new WaitForEndOfFrame();
        }

        m_isSizeTransitioning = false;
    }
}
