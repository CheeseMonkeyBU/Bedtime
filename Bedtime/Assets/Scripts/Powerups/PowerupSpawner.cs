using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PowerupSpawner : MonoBehaviour
{

    [SerializeField]
    GameObject m_powerup;

    // spawn chance as a percentage
    float SpawnChance = 0.333333333333f;

	// Use this for initialization
	void Start ()
    {
        float spawnResult = Random.Range(0.0f, 1.0f);
        if(spawnResult <= SpawnChance)
        {
            // spawn a powerup!
            Vector3 spawnPosition = transform.position + new Vector3(0, 2.5f, 0);
            GameObject spawnedPowerup = Instantiate(m_powerup, spawnPosition, Quaternion.identity);
            Powerup powerup = spawnedPowerup.GetComponent<Powerup>();

            int powerupType = Random.Range(0, 4);
            powerup.type = (Powerup.PowerupType)powerupType;
        }
        else
        {
            Destroy(gameObject);
        }
	}
}
