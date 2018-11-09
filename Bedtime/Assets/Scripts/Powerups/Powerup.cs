using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Powerup : MonoBehaviour
{

    public PowerupType type;
    public int maxPowerupIndex = 3;

    [SerializeField]
    GameObject m_placeholder;
    [SerializeField]
    GameObject m_modelFreeze;
    [SerializeField]
    GameObject m_modelSpeed;
    [SerializeField]
    GameObject m_modelInvincible;
    [SerializeField]
    GameObject m_modelObstacle;

    // Use this for initialization
    void Start ()
    {
        GetComponentInChildren<ParticleSystem>().Stop();
        Debug.Log("Starting Powerup");
        m_placeholder.SetActive(false);
        if (type == PowerupType.Freeze)
        {
            m_modelFreeze.SetActive(true);
            m_modelSpeed.SetActive(false);
            m_modelInvincible.SetActive(false);
            m_modelObstacle.SetActive(false);
        }
        if (type == PowerupType.Speed)
        {
            m_modelFreeze.SetActive(false);
            m_modelSpeed.SetActive(true);
            m_modelInvincible.SetActive(false);
            m_modelObstacle.SetActive(false);
        }
        if (type == PowerupType.Invincible)
        {
            m_modelFreeze.SetActive(false);
            m_modelSpeed.SetActive(false);
            m_modelInvincible.SetActive(true);
            m_modelObstacle.SetActive(false);
        }
        if (type == PowerupType.Obstacles)
        {
            m_modelFreeze.SetActive(false);
            m_modelSpeed.SetActive(false);
            m_modelInvincible.SetActive(false);
            m_modelObstacle.SetActive(true);
        }
    }
	
	// Update is called once per frame
	void Update ()
    {
        transform.Rotate(new Vector3(0, 100, 0) * Time.deltaTime);
	}
    void OnTriggerEnter(Collider _collision)
    {

        GameObject other = _collision.gameObject;
        if (other.tag == "Player")
        {
            Debug.Log("Collided with Player");
            other.GetComponent<PlayerController>().setCurrentHeldPowerup(type);
            Destroy(gameObject);
        }
        else if(other.tag == "Light")
        {
            Debug.Log("Light!!");
            GetComponentInChildren<ParticleSystem>().Play();
        }
    }

    void OnTriggerExit(Collider _other)
    {
        if (_other.tag == "Light")
        {
            GetComponentInChildren<ParticleSystem>().Stop();
        }
    }

    public enum PowerupType
    {
        None = -1,
        Freeze = 0,
        Speed = 1,
        Invincible = 2,
        Obstacles = 3
    }


}
