using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Character : MonoBehaviour {

    public float m_acceleration, m_speed, m_rotSpeed;
    public Camera m_camera;

    private Rigidbody m_rb;
    private Animator m_anim;

	void Start ()
    {
        m_rb = GetComponent<Rigidbody>();
        m_anim = GetComponent<Animator>();
	}
	
	void Update ()
    {
        m_rb.ResetInertiaTensor();

        float xInput = Input.GetAxis("Horizontal"), zInput = Input.GetAxis("Vertical");
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
            m_rb.velocity = new Vector3(0.0f, m_rb.velocity.y, 0.0f);
        }

        float gravity = m_rb.velocity.y;
        Vector3 horMotion = new Vector3(m_rb.velocity.x, 0.0f, m_rb.velocity.z);
        horMotion = Mathf.Clamp(horMotion.magnitude, 0.0f, m_speed) * horMotion.normalized;
        m_rb.velocity = new Vector3(horMotion.x, m_rb.velocity.y, horMotion.z);

        m_camera.transform.position = transform.position - (m_camera.transform.rotation * new Vector3(0.0f, 0.0f, 1.0f)) * 50.0f;
	}
}
