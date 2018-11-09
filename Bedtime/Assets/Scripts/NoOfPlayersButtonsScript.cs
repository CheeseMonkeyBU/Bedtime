using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class NoOfPlayersButtonsScript : MonoBehaviour {

    public Button twoPlayersButton;
    public Button threePlayersButton;
    public Button fourPlayersButton;

    private string sceneName = "Anim";

	// Use this for initialization
	void Start () {

 
	}
	
	// Update is called once per frame
	void Update () {

	}

    public void TwoPlayers()
    {
        NumberOfPlayers.numberOfPlayers = 2;
        SceneManager.LoadScene(sceneName);
    }

    public void ThreePlayers()
    {
        NumberOfPlayers.numberOfPlayers = 3;
        SceneManager.LoadScene(sceneName);
    }

    public void FourPlayers()
    {
        NumberOfPlayers.numberOfPlayers = 4;
        SceneManager.LoadScene(sceneName);
    }
}
