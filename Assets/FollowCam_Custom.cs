//using UnityEngine;
//using System.Collections;
//
//public class FollowCam_Custom : MonoBehaviour {
//	GameObject mega_man;
//	Vector3 mega_man_pos;
//	public float shake = 0f;
//	float shakeAmount = 0.1f;
//	float decreaseFactor = 1.0f;
//	
//	// Use this for initialization
//	void Start () {
//		mega_man = GameObject.Find ("Mega Man");
//	}
//	
//	// Update is called once per frame
//	void Update(){
//
//		
//		if (shake > 0) {
//			Vector3 random = Random.insideUnitSphere * shakeAmount;
//			random.z = -100f;
//			transform.position = random;
//			shake -= Time.deltaTime * decreaseFactor;
//		} else {
//			shake = 0f;
//		}
//
//
//
//		// Get the position of mega man
//		mega_man_pos = mega_man.transform.position;
//
//
//		// Cam follow for top level
//		if (mega_man_pos.x <= 0.000)return;

//
//		print (transform.position.x);
//		
//		
//		
//	}
//
//}

using UnityEngine;
using System.Collections;

public class FollowCam_Custom : MonoBehaviour {
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
		mega_man_pos = mega_man.transform.position;
		
		if (shake > 0 && mega_man_pos.x < 10) {
			Vector3 random = Random.insideUnitSphere * shakeAmount;
			random.z = -100f;
			transform.position = random;
			shake -= Time.deltaTime * decreaseFactor;
		} else {
			shake = 0f;
		}
		

		// Cam follow for top level
		if (mega_man_pos.x <= 0.000)return;
		if (mega_man_pos.x < 103.18 && mega_man_pos.y >= -2.9) Set_cam_x (mega_man_pos);
		//		if (mega_man_pos.x >= 69.11 && mega_man_pos.y >= -2.9) return;
//
//		if (mega_man_pos.y >= -3.93){
//			Set_cam_x (mega_man_pos);
//			Vector3 temp = transform.position;
//			temp.y = 0f;
//			transform.position = temp;
//		}





		if (mega_man_pos.x >= 103.19 && mega_man_pos.x < 106.39)return;
		if (mega_man_pos.x > 106.42 && mega_man_pos.x < 108.82 && transform.position.x < 111) {
			Vector3 temp = transform.position;
			temp.x += 8f * Time.deltaTime;
			transform.position = temp;
		}
		if (mega_man_pos.x > 110 && mega_man_pos.x < 114.95) {
			Vector3 temp = transform.position;
			temp.x = 111.1f;
			temp.y = 0f;
			transform.position = temp;
		}
		if (mega_man_pos.x > 114.95 && transform.position.x < 118.7) {
			Vector3 temp = transform.position;
			temp.x += 8f * Time.deltaTime;
			transform.position = temp;
		}
		if(mega_man_pos.x > 119) Set_cam_x(mega_man_pos);


		
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
