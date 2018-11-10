using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using TMPro;

public class WinController : MonoBehaviour {

    void OnTriggerEnter(Collider _other)
    {
        if (_other.tag != "Player")
            return;
        RectTransform winUI = FindObjectOfType<UIController>().getWinPanel();
        winUI.GetComponentInChildren<Image>().enabled = true;
        winUI.GetComponentInChildren<TextMeshProUGUI>().enabled = true;
        DarknessController[] killers = FindObjectsOfType<DarknessController>();
        foreach (DarknessController dc in killers)
        {
            dc.GetComponentInChildren<ParticleSystem>().Stop();
            Destroy(dc);
        }
        StartCoroutine(waitForMenu());
    }

    private IEnumerator waitForMenu()
    {
        yield return new WaitForSeconds(5);
        SceneManager.LoadScene(0);
    }
}
