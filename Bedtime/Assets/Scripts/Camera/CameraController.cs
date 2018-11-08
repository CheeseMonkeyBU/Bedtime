using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    Camera m_cameraComponent;

    float m_trauma;
    float m_maxShakeRotation = 0.5f;

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
        m_trauma -= 0.05f;
        if (m_trauma < 0.0f)
            m_trauma = 0.0f;

        if(m_trauma > 0.01f)
        {
            gameObject.transform.Rotate(new Vector3(
                m_maxShakeRotation * Mathf.Pow(m_trauma, 2) * Random.Range(-1.0f, 1.0f),
                m_maxShakeRotation * Mathf.Pow(m_trauma, 2) * Random.Range(-1.0f, 1.0f),
                m_maxShakeRotation * Mathf.Pow(m_trauma, 2) * Random.Range(-1.0f, 1.0f)));
        }
	}

    public void addTrauma(float _trauma)
    {
        m_trauma += _trauma;
    }

    public void setOrthoSize(float _targetSize, float _speed)
    {
        m_zoomSpeed = _speed;
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
