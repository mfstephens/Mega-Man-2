using UnityEngine;
using System.Collections;

public class Door_Open : MonoBehaviour {
	GameObject mega_man;
	bool done, done1, done3, should_push;
	float move_to_pos;
	// Use this for initialization
	void Start () {
		mega_man = GameObject.Find ("Mega Man");
		done = false;
		done1 = false;
		done3 = false;
		if(Application.loadedLevel == 1) move_to_pos = 135.4f;
		else move_to_pos = 108.4f;
	}
	
	// Update is called once per frame
	void FixedUpdate () {
		if(!done3 && done1 && mega_man.transform.position.x < move_to_pos){
			Vector3 temp = mega_man.transform.position;
			temp.x += 2f * Time.deltaTime;
			mega_man.transform.position = temp;
		} if(!done3 && done1 && mega_man.transform.position.x >= move_to_pos) done3 = true;
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
			done = true;
			audio.PlayOneShot(audio.clip, 1f);
		}
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
	}
}
