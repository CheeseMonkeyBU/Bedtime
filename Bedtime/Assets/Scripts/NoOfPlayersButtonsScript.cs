using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class NoOfPlayersButtonsScript : MonoBehaviour {

    public Button twoPlayersButton;
    public Button threePlayersButton;
    public Button fourPlayersButton;

    public string sceneName = "Anim";

	// Use this for initialization
	void Start () {

 
	}
	
	// Update is called once per frame
	void Update () {

	}

    public void TwoPlayers()
    {
        GameData.g_numberOfPlayers = 2;
        SceneManager.LoadScene(sceneName);
    }

    public void ThreePlayers()
    {
        GameData.g_numberOfPlayers = 3;
        SceneManager.LoadScene(sceneName);
    }

    public void FourPlayers()
    {
        GameData.g_numberOfPlayers = 4;
        SceneManager.LoadScene(sceneName);
    }
}
