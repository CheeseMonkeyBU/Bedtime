using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ButtonsScript : MonoBehaviour {

    public Image m_playerA, m_playerB, m_playerX, m_playerY;
    public Color m_highlightColorA, m_highlightColorB, m_highlightColorX, m_highlightColorY;

    private Color m_normalColor;
    private float m_aLerp = 0, m_bLerp = 0, m_xLerp = 0, m_yLerp = 0;

    void Start () {
        m_normalColor = m_playerA.color;
	}
	
	void Update () {
        if (Input.GetButton("PlayerA1") || Input.GetButton("PlayerB1") || Input.GetButton("PlayerX1") || Input.GetButton("PlayerY1"))
            m_aLerp = Mathf.Clamp(m_aLerp + Time.deltaTime, 0, 0.1f);
        else
            m_aLerp = Mathf.Clamp(m_aLerp - Time.deltaTime, 0, 0.1f);

        if (Input.GetButton("PlayerA2") || Input.GetButton("PlayerB2") || Input.GetButton("PlayerX2") || Input.GetButton("PlayerY2"))
            m_bLerp = Mathf.Clamp(m_bLerp + Time.deltaTime, 0, 0.1f);
        else
            m_bLerp = Mathf.Clamp(m_bLerp + Time.deltaTime, 0, 0.1f);

        if (Input.GetButton("PlayerA3") || Input.GetButton("PlayerB3") || Input.GetButton("PlayerX3") || Input.GetButton("PlayerY3"))
            m_xLerp = Mathf.Clamp(m_xLerp + Time.deltaTime, 0, 0.1f);
        else
            m_xLerp = Mathf.Clamp(m_xLerp + Time.deltaTime, 0, 0.1f);

        if (Input.GetButton("PlayerA4") || Input.GetButton("PlayerB4") || Input.GetButton("PlayerX4") || Input.GetButton("PlayerY4"))
            m_yLerp = Mathf.Clamp(m_yLerp + Time.deltaTime, 0, 0.1f);
        else
            m_yLerp = Mathf.Clamp(m_yLerp + Time.deltaTime, 0, 0.1f);

        m_playerA.color = Color.Lerp(m_normalColor, m_highlightColorA, m_aLerp * 10);
        m_playerB.color = Color.Lerp(m_normalColor, m_highlightColorB, m_bLerp * 10);
        m_playerX.color = Color.Lerp(m_normalColor, m_highlightColorX, m_xLerp * 10);
        m_playerY.color = Color.Lerp(m_normalColor, m_highlightColorY, m_yLerp * 10);
    }
}
