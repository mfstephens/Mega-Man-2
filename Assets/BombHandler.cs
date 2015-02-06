using UnityEngine;
using System.Collections;

public class BombHandler : MonoBehaviour {
	Vector3 spawn1, spawn2, spawn3;
	float respawn1_start, respawn2_start, respawn3_start, respawn_wait;
	float ground_duration, fall_speed;
	bool bomb1_start, bomb2_start, bomb3_start, bomb1_fall, bomb2_fall, bomb3_fall;
	bool blowup1, blowup2, blowup3, blowup;
	GameObject bomb1, bomb2, bomb3;
	public GameObject BombPrefab;
	// Use this for initialization
	void Start () {
		spawn1.Set (122.67f, 2.04f, -5f);
		spawn2.Set (125.61f, 2.04f, -5f);
		spawn3.Set (138.15f, 2.04f, -5f);
		bomb1 = Instantiate(BombPrefab) as GameObject;
		bomb2 = Instantiate (BombPrefab) as GameObject;
		bomb3 = Instantiate (BombPrefab) as GameObject;
		bomb1.transform.position = spawn1;
		bomb2.transform.position = spawn2;
		bomb3.transform.position = spawn3;
		fall_speed = 1f;
		ground_duration = 4f;
		respawn_wait = 15f;
		bomb1_start = bomb2_start = bomb3_start = true;
		blowup1 = blowup2 = blowup3 = blowup = false;
	}
	
	// Update is called once per frame
	void Update () {
		if(!bomb1_start){
			bomb1 = Instantiate(BombPrefab) as GameObject;
			bomb1.transform.position = spawn1;
			bomb1_start = true;
			blowup1 = false;
		}
		if(!bomb2_start){
			bomb2 = Instantiate(BombPrefab) as GameObject;
			bomb2.transform.position = spawn1;
			bomb2_start = true;
			blowup2 = false;
		}
		if(!bomb3_start){
			bomb3 = Instantiate(BombPrefab) as GameObject;
			bomb3.transform.position = spawn1;
			bomb3_start = true;
			blowup3 = false;
		}
		bombhandle (bomb1);
		bombhandle (bomb2);
		bombhandle (bomb3);
	}
	
	
	void bombhandle(GameObject bomb){
		if (bomb != null){
		if (bomb.transform.FindChild("Breakable") == null) {
			if((bomb.transform.position == spawn1) && !bomb1_fall) {respawn1_start = Time.time; bomb1_fall = true;}
			else if((bomb.transform.position == spawn2) && !bomb2_fall) {respawn2_start = Time.time; bomb2_fall = true;}
			else if ((bomb.transform.position == spawn3) && !bomb3_fall) {respawn3_start = Time.time; bomb3_fall = true;}
			Vector3 temp;
			if(bomb.transform.FindChild("Bomb").transform.position != null) temp = bomb.transform.FindChild("Bomb").transform.position;
			if(temp != null && temp.y > -2f){
				temp.y -= fall_speed * Time.deltaTime;
				bomb.transform.FindChild("Bomb").transform.position = temp;
			} else if(temp != null){
				if(bomb.transform.position == spawn1) {blowup1 = true;}
				if(bomb.transform.position == spawn2) blowup2 = true;
				if(bomb.transform.position == spawn3) blowup3 = true;
				if(blowup = false) StartCoroutine(flash_bright());
			}
			}
		else{
			if(bomb1 == null){	
				bomb1_fall = false;
				bomb1_start = false;
			}
			else if((bomb.transform.position == spawn1) && bomb1_fall && (respawn1_start + respawn_wait) < Time.time){
				bomb1_fall = false;
				bomb1_start = false;
				Destroy (bomb1);
			}
			if(bomb2 == null){	
				bomb2_fall = false;
				bomb2_start = false;
			}
			else if((bomb.transform.position == spawn2) && bomb2_fall && (respawn2_start + respawn_wait) < Time.time){
				bomb2_fall = false;
				bomb2_start = false;
				Destroy (bomb2);
			}
			if(bomb3 == null){	
				bomb3_fall = false;
				bomb3_start = false;
			}
			else if((bomb.transform.position == spawn3) && bomb3_fall && (respawn3_start + respawn_wait) < Time.time){
				bomb3_fall = false;
				bomb3_start = false;
				Destroy (bomb3);
			}
		}
		}
		
	}

	IEnumerator flash_bright(){
		print ("worked0");
		if(blowup1 && bomb1 != null){
			print ("worked");
			for(int i = 0; i < 5; i++){
				print ("worked1");
				bomb1.transform.FindChild("Bomb").renderer.material.color = new Color(1f, 1f, 1f, 1.0f);
				yield return new WaitForSeconds(0.17f);
				bomb1.transform.FindChild("Bomb").renderer.material.color = new Color(2.3f, 2.3f, 2.3f, 1f);
				yield return new WaitForSeconds(.17f);
			}
			bomb1.transform.FindChild("Bomb").renderer.material.color = new Color(1f, 1f, 1f, 0f);
			Destroy(bomb1);
		}
	
		if(blowup2 && bomb2 != null){
			for(int i = 0; i < 5; i++){
				bomb2.transform.FindChild("Bomb").renderer.material.color = new Color(1f, 1f, 1f, 1.0f);
				yield return new WaitForSeconds(0.17f);
				bomb2.transform.FindChild("Bomb").renderer.material.color = new Color(2.3f, 2.3f, 2.3f, 1f);
				yield return new WaitForSeconds(.17f);
			}
			bomb2.transform.FindChild("Bomb").renderer.material.color = new Color(1f, 1f, 1f, 0f);
			Destroy(bomb2);
		}

		if(blowup3 && bomb3 != null){
			for(int i = 0; i < 5; i++){
				bomb2.transform.FindChild("Bomb").renderer.material.color = new Color(1f, 1f, 1f, 1.0f);
				yield return new WaitForSeconds(0.17f);
				bomb2.transform.FindChild("Bomb").renderer.material.color = new Color(2.3f, 2.3f, 2.3f, 1f);
				yield return new WaitForSeconds(.17f);
			}
		bomb3.transform.FindChild("Bomb").renderer.material.color = new Color(1f, 1f, 1f, 0f);
		Destroy(bomb3);
		}
	}
}
