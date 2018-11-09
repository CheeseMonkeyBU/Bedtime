using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DarknessSphereController : MonoBehaviour {

    float elapsed = 0.0f;

	// Use this for initialization
	void Start () {
        float epsilon = Random.Range(0.0f, 0.0001f);


        transform.localPosition = transform.localPosition + new Vector3(epsilon, epsilon, epsilon); 
        transform.localScale = new Vector3(0,0,0);
	}
	void Update () {
        elapsed += Time.deltaTime;

        if(elapsed >= 5.0f)
        {
            float step = Mathf.SmoothStep(0.0f, 5.0f, (elapsed - 5.0f) / 20.0f);

            transform.localScale = new Vector3(step, step, step);
        }

        if(elapsed >= 15.0f)
        {
            GetComponent<SphereCollider>().enabled = true;
        }

        if (elapsed >= 30.0f)
        {
            Destroy(gameObject);
        }
    }

}
