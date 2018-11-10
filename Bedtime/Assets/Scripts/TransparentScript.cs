using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TransparentScript : MonoBehaviour {

	// Use this for initialization
	void Start () {
		foreach (Material m in gameObject.GetComponent<MeshRenderer>().materials) {
			
			m.color = new Color(m.color.r, m.color.g, m.color.b, 0.5f);
			continue;
		}
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
