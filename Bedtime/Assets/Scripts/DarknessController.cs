using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DarknessController : MonoBehaviour {

    public GameObject darknessInstance;

    public Sprite[] randomSprites;

    float elapsed = 0;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
        elapsed += Time.deltaTime;

        if(elapsed >= 0.05f)
        {
            GameObject o = Instantiate(darknessInstance);
            o.transform.position = transform.position;

            o.GetComponent<SpriteRenderer>().sprite = randomSprites[Random.Range(0, randomSprites.Length)];

            elapsed = 0.0f;
        }

	}
}
