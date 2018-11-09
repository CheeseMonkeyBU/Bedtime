using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class WinController : MonoBehaviour {

    void OnTriggerEnter(Collider _other)
    {
        RectTransform winUI = FindObjectOfType<UIController>().getWinPanel();
        winUI.GetComponentInChildren<Image>().enabled = true;
        winUI.GetComponentInChildren<TextMeshProUGUI>().enabled = true;
        DarknessController[] killers = FindObjectsOfType<DarknessController>();
        foreach (DarknessController dc in killers)
        {
            dc.GetComponentInChildren<ParticleSystem>().Stop();
            Destroy(dc);
        }
    }
}
