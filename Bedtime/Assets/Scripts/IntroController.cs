using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class IntroController : MonoBehaviour {

    public float m_introLength;
    public Color m_lightColor, m_darkColor;
    public Light m_tvLight, m_mainLight;

    public AudioSource dramaticBang;
    public AudioSource shepardTone1;
    public AudioSource shepardTone2;

    public Text m_clock;
    private Text m_instClock;
    Color initialColor;

    private float minuteLength;
    private float textFadeSpeed;
    private bool fadeTextBool;
    private bool clockCoroutineBool = true;
    bool GameOver = false;

    void Start () {
        PlayerController[] players = FindObjectsOfType<PlayerController>();
        foreach (PlayerController p in players)
        {
            p.GetComponentInChildren<Animator>().SetBool("Sit", true);
            p.transform.rotation = Quaternion.Euler(0, 180, 0);
            p.m_inCutScene = true;
        }

        m_instClock = Instantiate(m_clock, FindObjectOfType<UIController>().m_screenCanvas.transform);

        m_instClock.text = "19:57";
        initialColor = m_instClock.color;

        minuteLength = 3f;
        textFadeSpeed = 1.0f;
        fadeTextBool = false;


        StartCoroutine(waitForScene());
	}
	
	void Update () {


        if (fadeTextBool)
        {
            FadeText();
        }
        if (m_instClock.color.a == 0)
        {
            fadeTextBool = false;
        }

    }

    IEnumerator waitForScene()
    {
        PlayerController[] players = FindObjectsOfType<PlayerController>();

        yield return new WaitForSeconds(minuteLength);
        m_instClock.text = "19:58";

        yield return new WaitForSeconds(minuteLength);
        m_instClock.text = "19:59";

        yield return new WaitForSeconds(minuteLength);
        m_instClock.text = "RU:N!";

        FindObjectOfType<GamePlayController>().PlayDramaticBang();
        fadeTextBool = true;

        // Players stand up
        foreach (PlayerController p in players)
        {
            p.GetComponentInChildren<Animator>().SetBool("Sit", false);
            p.m_inCutScene = false;
        }
        m_mainLight.intensity = 0;
        Destroy(m_tvLight.transform.parent.gameObject);

        yield return new WaitForSeconds(2);

        FindObjectOfType<SpawnController>().spawnDarkness();

        StartCoroutine(ShepardTone1());
    }

    IEnumerator ShepardTone1()
    {
        while (true)
        {
            //shepardTone1.Play();
            FindObjectOfType<GamePlayController>().PlayShepardTone1();
            yield return new WaitForSeconds(116f);

            //shepardTone2.Play();
            FindObjectOfType<GamePlayController>().PlayShepardTone2();
            yield return new WaitForSeconds(116f);

            if (GameOver)
            {
                yield return null;
            }
        }

    }

    void FadeText()
    {
        m_instClock.color = new Color(initialColor.r, initialColor.g, initialColor.b, Mathf.Lerp(m_instClock.color.a, 0, textFadeSpeed * Time.deltaTime));
    }
}
