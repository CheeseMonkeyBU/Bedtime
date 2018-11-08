using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GamePlayController : MonoBehaviour {

	public PlayerController[] m_players; 

	// Use this for initialization
	void Start () {
		for (int i = 0; i < m_players.Length; i++) {
			if (m_players [i] != null) {
				m_players [i].m_playerNumber = i + 1;
				m_players [i].m_gpController = this;
			}
		}
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	public void UsePowerup(int player, PlayerController.Power power) {
		if (m_players [player] != null) {
			m_players [player].ApplyPower(power);
		}
	}

}
