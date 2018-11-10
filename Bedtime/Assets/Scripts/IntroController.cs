using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IntroController : MonoBehaviour {

    public float m_introLength;
    public Color m_lightColor, m_darkColor;
    public Light m_tvLight, m_mainLight;

	void Start () {
        PlayerController[] players = FindObjectsOfType<PlayerController>();
        foreach (PlayerController p in players)
        {
            p.GetComponentInChildren<Animator>().SetBool("Sit", true);
            p.transform.rotation = Quaternion.Euler(0, 180, 0);
            p.m_inCutScene = true;
        }
        StartCoroutine(waitForScene());
	}
	
	void Update () {
		
	}

    IEnumerator waitForScene()
    {
        PlayerController[] players = FindObjectsOfType<PlayerController>();

        yield return new WaitForSeconds(5);
        
        foreach (PlayerController p in players)
        {
            p.GetComponentInChildren<Animator>().SetBool("Sit", false);
            p.m_inCutScene = false;
        }
        m_mainLight.intensity = 0;
        Destroy(m_tvLight.transform.parent.gameObject);

        FindObjectOfType<SpawnController>().spawnDarkness();
    }
}
