using UnityEngine;
using System.Collections;

public class FollowCam : MonoBehaviour {

	// Use this for initialization
	void Start () {

	}
	
	// Update is called once per frame
	void Update(){


	}

	void FixedUpdate () {

		// Get the position of mega man
		Vector3 mega_man_pos = GameObject.Find ("Mega Man").transform.position;
		
		// Cam follow for top level
		if (mega_man_pos.x <= 0.000)return;
		if (mega_man_pos.x >= 69.11 && mega_man_pos.y >= -1.35) return;
		if (mega_man_pos.x >= 65 && mega_man_pos.y < -1.35 && mega_man_pos.y > -7.5) {
			Set_cam_y (mega_man_pos);
			return;
		}
		if (mega_man_pos.x >= 65 && mega_man_pos.y <= -7.5 && mega_man_pos.y > -10.6) {
			Vector3 temp_cam_pos = mega_man_pos;
			temp_cam_pos.y = -8.2f;
			Set_cam_y (temp_cam_pos);
			return;
				}
		if (mega_man_pos.x < 67 && mega_man_pos.y < -9 && mega_man_pos.y > -14.88) {
			Vector3 temp_cam_pos = mega_man_pos;
			temp_cam_pos.y += .41f;
			Set_cam_y (temp_cam_pos);
			return;
		}
		if (mega_man_pos.x > 65 && mega_man_pos.x < 69.2 && mega_man_pos.y <= -14.88) {
			Vector3 temp_cam_pos = mega_man_pos;
			temp_cam_pos.y = -14.9f;
			Set_cam_y (temp_cam_pos);
			return;
			}
		if (mega_man_pos.x > 146)return;
		Set_cam_x (mega_man_pos);

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
