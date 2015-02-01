using UnityEngine;
using System.Collections;

public class ItemFlash : MonoBehaviour {

	// Use this for initialization
	void Start () {
		StartCoroutine(flash ());
	}
	
	// Update is called once per frame
	void Update () {
	}

	IEnumerator flash() {
		while(true) {
			gameObject.renderer.material.color = new Color(0.7f, 0.7f, 0.7f, 1.0f);
			yield return new WaitForSeconds(0.16f);
			gameObject.renderer.material.color = new Color(1.2f, 1.2f, 1.2f, 1.0f);
			yield return new WaitForSeconds(.16f);
		}
	}
}
