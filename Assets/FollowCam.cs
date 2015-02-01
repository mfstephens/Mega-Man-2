using UnityEngine;
using System.Collections;

public class FollowCam : MonoBehaviour {
	bool advanced1, advanced2, advanced3, advanced4;
	float wait, wait_time;
	GameObject mega_man;
	Vector3 mega_man_pos;
	public float shake = 0f;
	float shakeAmount = 0.1f;
	float decreaseFactor = 1.0f;

	// Use this for initialization
	void Start () {
		advanced1 = advanced2 = false;
		wait = 0;
		wait_time = .8f;
		mega_man = GameObject.Find ("Mega Man");
	}
	
	// Update is called once per frame
	void Update(){


		if (shake > 0) {
			Vector3 random = Random.insideUnitSphere * shakeAmount;
			random.z = -100f;
			transform.position = random;
			shake -= Time.deltaTime * decreaseFactor;
		} else {
			shake = 0f;
		}

		// Get the position of mega man
		mega_man_pos = mega_man.transform.position;
		// Cam follow for top level
		if (mega_man_pos.x <= 0.000)return;
		if (mega_man_pos.x < 69.11 && mega_man_pos.y >= -2.9) Set_cam_x (mega_man_pos);
		if (mega_man_pos.x >= 69.11 && mega_man_pos.y >= -2.9) return;

		// wait to pan down after first level advance
		if (mega_man_pos.x >= 65.5 && mega_man_pos.x < 75 && mega_man_pos.y <= -2.9 && !advanced1) {
			if(wait + wait_time + .5f < Time.time) wait = Time.time;
			if(wait + wait_time <= Time.time){
				advanced1 = true;
			}
			else return;
		}
		// pan down after first level advance
		if (advanced1 && mega_man_pos.y > -10.6 && transform.position.y > -8.2 ) {
			Vector3 temp = transform.position;
			temp.y -= 9f * Time.deltaTime;
			transform.position = temp;
		} 

		// wait to pan down after second level advance
		if (mega_man_pos.x > 65 && mega_man_pos.x < 75 && mega_man_pos.y <= -11.2 && !advanced2) {
			if(wait + wait_time + .5f < Time.time) wait = Time.time;
			if(wait + wait_time <= Time.time){
				advanced2 = true;
			}
			else return;
		}

		// pan down after second level adance
		if (mega_man_pos.x > 65 && mega_man_pos.y <= -11.2 && transform.position.y > -14.88 ) {
			Vector3 temp = transform.position;
			temp.y -= 9f * Time.deltaTime;
			transform.position = temp;
		} 
		if (mega_man_pos.x >= 69.35 && mega_man_pos.y < -11 && mega_man_pos.x < 130.5) {
			Set_cam_x(mega_man_pos);
		}

		if (mega_man_pos.x >= 130.5 && mega_man_pos.x < 133.7)return;
		if (mega_man_pos.x > 134 && mega_man_pos.x < 141.84 && transform.position.x < 138.48) {
			Vector3 temp = transform.position;
			temp.x += 8f * Time.deltaTime;
			transform.position = temp;
		}
		if (mega_man_pos.x > 141.9 && transform.position.x < 145.88) {
			Vector3 temp = transform.position;
			temp.x += 8f * Time.deltaTime;
			transform.position = temp;
		}



	}

	void FixedUpdate () {
		
	}

	void Set_cam_y(Vector3 pos){
		// Set camera's x value to Mega Man's x value
		Vector3 cam_temp = this.transform.position;
		cam_temp.y = pos.y;
		this.transform.position = cam_temp;
	}

	void Set_cam_x(Vector3 mega_man_pos){
		// Set camera's x value to Mega Man's x value
		Vector3 cam_temp = this.transform.position;
		cam_temp.x = mega_man_pos.x;
		this.transform.position = cam_temp;
	}
}
