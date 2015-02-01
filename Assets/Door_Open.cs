using UnityEngine;
using System.Collections;

public class Door_Open : MonoBehaviour {
	GameObject mega_man;
	bool done, should_push;
	// Use this for initialization
	void Start () {
		mega_man = GameObject.Find ("Mega Man");
		done = false;
	}
	
	// Update is called once per frame
	void Update () {

	}

	void OnTriggerStay(Collider other){
		OnTriggerEnter (other);
	}

	void OnTriggerExit(Collider other){

		Destroy(GetComponent<Door_Close>());
	}
	
	void OnTriggerEnter(Collider other) {
		PE_Obj otherPEO = other.GetComponent<PE_Obj>();
		if (otherPEO == null) return;
		if (otherPEO.coll == PE_Collider.megaman && !done) {
			otherPEO.still = true;
			otherPEO.vel.x = 0;
			otherPEO.vel.y = 0;
			StartCoroutine(open_and_push());
		}

	}
	
	IEnumerator open_and_push() {
		Renderer[] door_bars = GetComponentsInChildren<Renderer> ();
		for (int i = 0; i < 16; ++i) {
			door_bars[i].material.color = new Color(1f, 1f, 1f, 0f);
			++i;
			if(i < 16) door_bars[i].material.color = new Color(1f, 1f, 1f, 0f);
			++i;
			if(i < 16) door_bars[i].material.color = new Color(1f, 1f, 1f, 0f);
			++i;
			if(i < 16) door_bars[i].material.color = new Color(1f, 1f, 1f, 0f);
			yield return new WaitForSeconds(0.2f);
		}
		Vector3 temp = mega_man.transform.position;
		temp.x += 2f * Time.deltaTime;
		mega_man.transform.position = temp;
	}
}
