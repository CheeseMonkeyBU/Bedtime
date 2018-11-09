using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour {

	public int m_playerNumber;
	public GamePlayController m_gpController;

	public Power m_heldPower;
	public Power m_currentEffect;

	public float m_acceleration, m_speed, m_rotSpeed;
	public Camera m_camera;
    public Canvas m_canvas;

    private float m_powerLength = 5.0f;
    private float m_elapsed;

    private Rigidbody m_rb;
	private Animator m_anim;

	void Start () {
		m_rb = GetComponent<Rigidbody>();
		m_anim = GetComponent<Animator>();
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

		//Player Movement code

		m_rb.ResetInertiaTensor();

		float xInput = Input.GetAxis("Horizontal" + (m_playerNumber + 1).ToString()), zInput = Input.GetAxis("Vertical" + (m_playerNumber + 1).ToString());
		if (xInput != 0 || zInput != 0)
		{
			Vector3 input = new Vector3(xInput, 0.0f, zInput).normalized;
			transform.rotation = Quaternion.Euler(0.0f, m_camera.transform.rotation.eulerAngles.y, 0.0f) * Quaternion.LookRotation(input);
			m_rb.AddForce(transform.rotation * new Vector3(0.0f, 0.0f, 1.0f) * m_acceleration * input.magnitude, ForceMode.Impulse);
			m_anim.SetBool("Walk", true);
		}
		else
		{
			m_anim.SetBool("Walk", false);
			if (m_rb.velocity.y > 0)
				m_rb.velocity = new Vector3(0.0f, 0.0f, 0.0f);
			else
				m_rb.velocity = new Vector3(0.0f, m_rb.velocity.y, 0.0f);
		}

		float gravity = m_rb.velocity.y;
		Vector3 horMotion = new Vector3(m_rb.velocity.x, 0.0f, m_rb.velocity.z);
		horMotion = Mathf.Clamp(horMotion.magnitude, 0.0f, m_speed) * horMotion.normalized;
		m_rb.velocity = new Vector3(horMotion.x, m_rb.velocity.y, horMotion.z);

		m_camera.transform.position = transform.position - (m_camera.transform.rotation * new Vector3(0.0f, 0.0f, 1.0f)) * 50.0f;
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

    public GameObject getCanvasGameObject()
    {
        return m_camera.GetComponent<CameraController>().canvas;
    }

}
