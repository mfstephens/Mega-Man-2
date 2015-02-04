using UnityEngine;
using System.Collections;

public class GroundFlash : MonoBehaviour {
	
	// Use this for initialization
	void Start () {
		StartCoroutine(flash ());
	}
	
	// Update is called once per frame
	void Update () {
	}
	
	IEnumerator flash() {
		while(true) {
			gameObject.renderer.material.color = new Color(1f, 1f, 1f, 0f);
			yield return new WaitForSeconds(0.11f);
			gameObject.renderer.material.color = new Color(1f, 1f, 1f, 1.0f);
			yield return new WaitForSeconds(.11f);
		}
	}
}
