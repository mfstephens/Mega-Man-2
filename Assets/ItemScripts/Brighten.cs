using UnityEngine;
using System.Collections;

public class Brighten : MonoBehaviour {

	// Use this for initialization
	void Start () {
		gameObject.renderer.material.color = new Color(1.2f, 1.2f, 1.2f, 1.0f);
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
