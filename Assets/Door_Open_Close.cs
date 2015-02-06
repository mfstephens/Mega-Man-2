using UnityEngine;
using System.Collections;

public class Door_Open_Close : MonoBehaviour {
	GameObject mega_man;
	bool done, done1, done2, done3, done4;
	float move_to_pos;
	// Use this for initialization
	void Start () {
		mega_man = GameObject.Find ("Mega Man");
		done = done1 = done2 = done3 = done4 = false;
		if(Application.loadedLevel == 1) move_to_pos = 143.05f;
		else move_to_pos = 115.72f;
	}


	
	void OnTriggerStay(Collider other){
		OnTriggerEnter (other);
	}

	

	void FixedUpdate () {
		if (mega_man.transform.position.x < transform.position.x) GetComponents<AudioSource> () [1].Stop ();
		if(!done2 && done1 && mega_man.transform.position.x < move_to_pos){
			Vector3 temp = mega_man.transform.position;
			temp.x += 2f * Time.deltaTime;
			mega_man.transform.position = temp;
			StopCoroutine(open_and_push());
		} 
		if(!done2 && done1 && mega_man.transform.position.x >= move_to_pos) done2 = true;
		if (done && done1 && done2 && !done3){
			StartCoroutine(close_and_push());
			done3 = true;
		}
		if(done && done1 && done2 && done3 && done4){
			StopCoroutine(close_and_push());
			PE_Obj mm = mega_man.GetComponent<PE_Obj>();
			mm.pos0 = mm.pos1 = mm.transform.position;
			mm.still = false;
			mm.vel.y += 2f;
			done = done1 = done2 = done3 = done4 = false;
		}
	}
	

	void OnTriggerEnter(Collider other) {
		PE_Obj otherPEO = other.GetComponent<PE_Obj>();
		if (otherPEO == null) return;
		if (otherPEO.coll == PE_Collider.megaman && !done) {
			otherPEO.still = true;
			otherPEO.vel.x = 0;
			otherPEO.vel.y = 0;
			StartCoroutine(open_and_push());
			done = true;
			audio.PlayOneShot(audio.clip, 1f);
		}
	}


	IEnumerator close_and_push() {
		Renderer[] door_bars = GetComponentsInChildren<Renderer> ();
		yield return new WaitForSeconds(0.65f);
		audio.PlayOneShot(audio.clip, 1f);
		yield return new WaitForSeconds(0.165f);
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
		mega_man.GetComponents<AudioSource> () [0].Stop ();
		GetComponents<AudioSource> () [1].Play ();
		done4 = true;
		yield return null;
	}
	
	IEnumerator open_and_push() {
		Renderer[] door_bars = GetComponentsInChildren<Renderer> ();
		yield return new WaitForSeconds(0.17f);
		for (int i = 0; i < 16; ++i) {
			door_bars[i].material.color = new Color(1f, 1f, 1f, 0f);
			++i;
			if(i < 16) door_bars[i].material.color = new Color(1f, 1f, 1f, 0f);
			++i;
			if(i < 16) door_bars[i].material.color = new Color(1f, 1f, 1f, 0f);
			++i;
			if(i < 16) door_bars[i].material.color = new Color(1f, 1f, 1f, 0f);
			yield return new WaitForSeconds(0.18f);
		}
		done1 = true;
		yield return null;
	}
}
