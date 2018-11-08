using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour {

	public int m_playerNumber;
	public GamePlayController m_gpController;
	public Character m_character;

	public Power m_heldPower;
	public Power m_currentEffect;

	private float m_powerLength = 5.0f;
	private float m_elapsed;

	void Start () {
		
	}
	
	void Update () {
		if(m_heldPower != Power.None) {
			if (Input.GetButtonDown ("PlayerA" + m_playerNumber.ToString ())) {
				m_gpController.UsePowerup (0, m_heldPower);
				m_heldPower = Power.None;
			}
			else if (Input.GetButtonDown ("PlayerB" + m_playerNumber.ToString ())) {
				m_gpController.UsePowerup (1, m_heldPower);
				m_heldPower = Power.None;
			}
			else if (Input.GetButtonDown ("PlayerX" + m_playerNumber.ToString ())) {
				m_gpController.UsePowerup (2, m_heldPower);
				m_heldPower = Power.None;
			}
			else if (Input.GetButtonDown ("PlayerY" + m_playerNumber.ToString ())) {
				m_gpController.UsePowerup (3, m_heldPower);
				m_heldPower = Power.None;
			}
		}

		if (m_currentEffect != Power.None) {
			m_elapsed += Time.deltaTime;

			if (m_elapsed >= m_powerLength) {
				m_elapsed = 0;
				m_currentEffect = Power.None;
			}

			Debug.Log ("Has had " + m_currentEffect.ToString () + " for " + m_elapsed);

			switch (m_currentEffect) {
			case Power.StarPower:
				
				break;
			case Power.SlowDown:
				break;
			}
		}
	}

	public void ApplyPower(Power power) {
		m_elapsed = 0;
		m_currentEffect = power;
	}

	public enum Power {
		None,
		StarPower,
		SlowDown
	}

}
