using UnityEngine;
using System.Collections;

public class Door_Open_Close : MonoBehaviour {
	GameObject mega_man;
	bool done1, done;
	Renderer[] door_bars;
	// Use this for initialization
	void Start () {
		mega_man = GameObject.Find ("Mega Man");
		done = done1 = false;
		door_bars = GetComponentsInChildren<Renderer> ();
	}
	
	// Update is called once per frame
	void Update () {
		if (done == true){
			PE_Obj mm = mega_man.GetComponent<PE_Obj>();
			mm.pos0 = mm.pos1 = mm.transform.position;
			mm.still = false;
			mm.vel.y += 2f;
			for (int i = 15; i >= 0; --i) {
				door_bars[i].material.color = new Color(1f, 1f, 1f, 1f);
				--i;
				if(i >= 0) door_bars[i].material.color = new Color(1f, 1f, 1f, 1f);
				--i;
				if(i >= 0) door_bars[i].material.color = new Color(1f, 1f, 1f, 1f);
				--i;
				if(i >= 0) door_bars[i].material.color = new Color(1f, 1f, 1f, 1f);
			done = false;
			}
		}
		
	}
	
	void OnTriggerStay(Collider other){
		OnTriggerEnter (other);
	}

	void OnTriggerExit(Collider other){
		PE_Obj otherPEO = other.GetComponent<PE_Obj>();
		done1 = true;
		Vector3 temp = mega_man.transform.position;
		temp.x += .01f;
		mega_man.transform.position = temp;
		if (otherPEO == null) return;
		if (otherPEO.coll == PE_Collider.megaman) {
			StartCoroutine(close_and_push());
		}
	}
	
	
	IEnumerator close_and_push() {
		for (int i = 15; i >= 0; --i) {
			done1 = true;
			door_bars[i].material.color = new Color(1f, 1f, 1f, 1f);
			--i;
			if(i >= 0) door_bars[i].material.color = new Color(1f, 1f, 1f, 1f);
			--i;
			if(i >= 0) door_bars[i].material.color = new Color(1f, 1f, 1f, 1f);
			--i;
			if(i >= 0) door_bars[i].material.color = new Color(1f, 1f, 1f, 1f);
			yield return new WaitForSeconds(0.2f);
		}
		done = true;
	}

	
	void OnTriggerEnter(Collider other) {
		PE_Obj otherPEO = other.GetComponent<PE_Obj>();
		if (otherPEO == null) return;
		if (otherPEO.coll == PE_Collider.megaman && !done1) {
			otherPEO.still = true;
			otherPEO.vel.x = 0;
			otherPEO.vel.y = 0;
			StartCoroutine(open_and_push());
		}
		
	}

	IEnumerator open_and_push() {
		while (done1) yield return null;
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
