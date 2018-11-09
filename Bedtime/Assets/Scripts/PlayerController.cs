using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour {

	public int m_playerNumber;
	public GamePlayController m_gpController;

	public Powerup.PowerupType m_heldPower;

    //[HideInInspector]
    public float m_speed = 8.0f;
    public float m_acceleration;
    public float m_rotSpeed;

    // This doesn't change when player if effected by freeze etc. 
    public float defaultSpeed;

    public Camera m_camera;
    public Canvas m_canvas;

    //public float m_powerLength = 5.0f;
    private float m_elapsed;

    private bool m_light = false;
    public float m_battery;
    public float m_maxBattery = 100;

    private Rigidbody m_rb;
	private Animator m_anim;

    public bool m_isInvincible = false;

	void Start () {
		m_rb = GetComponent<Rigidbody>();
		m_anim = GetComponent<Animator>();

        defaultSpeed = m_speed;

        m_heldPower = Powerup.PowerupType.None;
        m_battery = 15;
	}

    void Update()
    {

        if (m_heldPower != Powerup.PowerupType.None)
        {
            if (Input.GetButtonDown("PlayerA" + (m_playerNumber + 1).ToString()))
            {
                // if A button pressed
                Debug.Log("Player " + m_playerNumber + " targeting player 0 with current powerup");

                usePowerup(0);

                m_heldPower = Powerup.PowerupType.None;
            }
            else if (Input.GetButtonDown("PlayerB" + (m_playerNumber + 1).ToString()))
            {
                // if B button pressed
                Debug.Log("Player " + m_playerNumber + " targeting player 0 with current powerup");

                usePowerup(1);

                m_heldPower = Powerup.PowerupType.None;
            }
            else if (Input.GetButtonDown("PlayerX" + (m_playerNumber + 1).ToString()))
            {
                // if X button pressed
                Debug.Log("Player " + m_playerNumber + " targeting player 0 with current powerup");

                usePowerup(2);

                m_heldPower = Powerup.PowerupType.None;
            }
            else if (Input.GetButtonDown("PlayerY" + (m_playerNumber + 1).ToString()))
            {
                // if Y button pressed
                Debug.Log("Player " + m_playerNumber + " targeting player 0 with current powerup");

                usePowerup(3);

                m_heldPower = Powerup.PowerupType.None;
            }
        }

    }

    void FixedUpdate()
    { 
        if (Input.GetButtonDown("Light" + (m_playerNumber + 1).ToString()) && m_battery > 0)
        {
            m_light = !m_light;
            GetComponent<Animator>().SetBool("Light", m_light);
            GetComponentInChildren<Light>().enabled = m_light;
        }

        if (m_light)
        {
            m_battery -= (Time.deltaTime / 2);
            if(m_battery <= 0)
            {
                m_light = false;
                GetComponent<Animator>().SetBool("Light", m_light);
                GetComponentInChildren<Light>().enabled = m_light;
            }
        }


		//Player Movement code

		m_rb.ResetInertiaTensor();

		float xInput = Input.GetAxis("Horizontal" + (m_playerNumber + 1).ToString()), zInput = Input.GetAxis("Vertical" + (m_playerNumber + 1).ToString());
		if ((xInput != 0 || zInput != 0) && m_speed > 0)
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

    public void setCurrentHeldPowerup(Powerup.PowerupType _type)
    {
        m_heldPower = _type;
        m_canvas.GetComponent<CanvasController>().changePowerupIcon(_type);
    }

    public void usePowerup(int _playerID)
    {
        m_gpController.applyPowerupToPlayer(_playerID, m_heldPower);
    }


    public GameObject getCanvasGameObject()
    {
        return m_camera.GetComponent<CameraController>().canvas;
    }

    public void kill()
    {
        FindObjectOfType<ScreenController>().removeViewport(m_camera.gameObject);
        Destroy(gameObject);
    }
}
