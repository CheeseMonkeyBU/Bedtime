using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour {

    public Color m_aColor, m_bColor, m_xColor, m_yColor;

	public int m_playerNumber;
	public GamePlayController m_gpController;

	public Powerup.PowerupType m_heldPower;

    public GameObject m_icon;

    //[HideInInspector]
    public float m_speed = 8.0f;
    public float m_acceleration;
    public float m_rotSpeed;

    // This doesn't change when player if effected by freeze etc. 
    public float defaultSpeed;

    public Camera m_camera;
    public Canvas m_canvas;

    public GameObject iceBlock;
    public GameObject forceField;

    //public float m_powerLength = 5.0f;
    private float m_elapsed;

    private bool m_light = false;
    public float m_battery;
    public float m_maxBattery = 100;

    private Rigidbody m_rb;
	private Animator m_anim;

    public bool m_hasStatusEffect = false;
    public bool m_isInvincible = false;

    public bool m_inCutScene = false;

    public StairController m_recentStairController;

	void Start () {
		m_rb = GetComponent<Rigidbody>();
		m_anim = GetComponent<Animator>();

        defaultSpeed = m_speed;

        m_heldPower = Powerup.PowerupType.None;
        m_battery = 15;

        m_camera.transform.position = transform.position - (m_camera.transform.rotation * new Vector3(0.0f, 0.0f, 1.0f)) * 70.0f;

        if (m_playerNumber == 0)
            m_icon.GetComponent<MeshRenderer>().material.color = m_aColor;
        else if (m_playerNumber == 1)
            m_icon.GetComponent<MeshRenderer>().material.color = m_bColor;
        else if (m_playerNumber == 2)
            m_icon.GetComponent<MeshRenderer>().material.color = m_xColor;
        else if (m_playerNumber == 3)
            m_icon.GetComponent<MeshRenderer>().material.color = m_yColor;
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
            }
            else if (Input.GetButtonDown("PlayerB" + (m_playerNumber + 1).ToString()))
            {
                // if B button pressed
                Debug.Log("Player " + m_playerNumber + " targeting player 0 with current powerup");

                usePowerup(1);
            }
            else if (Input.GetButtonDown("PlayerX" + (m_playerNumber + 1).ToString()))
            {
                // if X button pressed
                Debug.Log("Player " + m_playerNumber + " targeting player 0 with current powerup");

                usePowerup(2);
            }
            else if (Input.GetButtonDown("PlayerY" + (m_playerNumber + 1).ToString()))
            {
                // if Y button pressed
                Debug.Log("Player " + m_playerNumber + " targeting player 0 with current powerup");

                usePowerup(3);
            }
        }

        m_camera.transform.position = transform.position - (m_camera.transform.rotation * new Vector3(0.0f, 0.0f, 1.0f)) * 70.0f;
    }

    void FixedUpdate()
    {
        if (m_inCutScene)
            return;

        if (Input.GetButtonDown("Light" + (m_playerNumber + 1).ToString()) && m_battery > 0)
        {
            m_light = !m_light;
            GetComponent<Animator>().SetBool("Light", m_light);
            GetComponentInChildren<Light>().enabled = m_light;
            GetComponentInChildren<Light>().transform.parent.GetComponentInChildren<Collider>().enabled = m_light;
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
	}

    public void setCurrentHeldPowerup(Powerup.PowerupType _type)
    {
        m_heldPower = _type;
        m_canvas.GetComponent<CanvasController>().changePowerupIcon(_type);
    }

    public void usePowerup(int _playerID)
    {
        // If managed to use status effect
        if(m_gpController.applyPowerupToPlayer(_playerID, m_heldPower))
        {
            Debug.Log("Applying Powerup");
            m_canvas.GetComponent<CanvasController>().clearPowerupIcon();
            m_heldPower = Powerup.PowerupType.None;
        }
        // else, dont clear the icon, you still have it
    }

    public bool canTakeStatusEffect()
    {
        if(m_hasStatusEffect == false && m_isInvincible == false)
        {
            return true;
        }
        else
        {
            return false;
        }
    }

    public GameObject getCanvasGameObject()
    {
        return m_camera.GetComponent<CameraController>().canvas;
    }

    public void kill()
    {
        FindObjectOfType<GamePlayController>().OnPlayerDeath();
        FindObjectOfType<ScreenController>().removeViewport(m_camera.gameObject);
        Destroy(gameObject);
    }
}
