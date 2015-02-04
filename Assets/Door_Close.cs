using UnityEngine;
using System.Collections;

public class Door_Close : MonoBehaviour {
	GameObject mega_man;
	bool done, done1;
	// Use this for initialization
	void Start () {
		mega_man = GameObject.Find ("Mega Man");
		Renderer[] door_bars = GetComponentsInChildren<Renderer> ();
		for (int i = 0; i < 16; ++i) {
			door_bars[i].material.color = new Color(1f, 1f, 1f, 0f);
		}
		done = done1 = false;
	}
	
	// Update is called once per frame
	void Update () {
		if (done1 == true){
			Vector3 temp = mega_man.transform.position;
			temp.x += .01f;
			mega_man.transform.position = temp;
			PE_Obj mm = mega_man.GetComponent<PE_Obj>();
			mm.pos0 = mm.pos1 = temp;
			mm.still = false;
			mm.vel.y += 2f;
			done1 = false;
			Destroy(GetComponent<Door_Close>());
		}
		
	}

	void OnTriggerExit(Collider other){
		PE_Obj otherPEO = other.GetComponent<PE_Obj>();
		if (otherPEO == null) return;
		if (otherPEO.coll == PE_Collider.megaman && !done) {
			otherPEO.still = true;
			otherPEO.vel.x = 0;
			otherPEO.vel.y = 0;
			StartCoroutine(close_and_push());
			done = true;
		}
	}
		

	IEnumerator close_and_push() {
		Renderer[] door_bars = GetComponentsInChildren<Renderer> ();
		yield return new WaitForSeconds(.5f);
		audio.PlayOneShot(audio.clip, 1f);
		yield return new WaitForSeconds(.17f);
		for (int i = 15; i >= 0; --i) {
			door_bars[i].material.color = new Color(1f, 1f, 1f, 1f);
			--i;
			if(i >= 0) door_bars[i].material.color = new Color(1f, 1f, 1f, 1f);
			--i;
			if(i >= 0) door_bars[i].material.color = new Color(1f, 1f, 1f, 1f);
			--i;
			if(i >= 0) door_bars[i].material.color = new Color(1f, 1f, 1f, 1f);
			yield return new WaitForSeconds(0.18f);
		}
		done1 = true;
	}
}
