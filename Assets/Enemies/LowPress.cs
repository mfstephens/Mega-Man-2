using UnityEngine;
using System.Collections;

public class LowPress : MonoBehaviour{

	bool starting_position, going_down;
	float downward_vel, downward_accel, upward_vel, frames_passed;
	float init_down_vel, init_down_accel, init_upward_vel;
	SpriteRenderer[] my_renderer;
	Transform mega_man;

	void Awake(){
		my_renderer = GetComponentsInChildren<SpriteRenderer> ();
		foreach(SpriteRenderer rend in my_renderer){
			if(rend.sprite != my_renderer[10].sprite) rend.enabled = false;
		}

	}

	// Use this for initialization
	void Start () {
		starting_position = true;
		going_down = false;
		frames_passed = 0f;
		init_down_accel = -.0001f;
		init_down_vel = -.05f;
		init_upward_vel = .012f;
		downward_accel = init_down_accel;
		downward_vel = init_down_vel;
		upward_vel = init_upward_vel;
		mega_man = GameObject.Find ("Mega Man").transform;
	}

	void FixedUpdate () {

		// press is going down
		if((Mathf.Abs (mega_man.position.x - this.transform.position.x)) < 1.19f || going_down){
			if (starting_position || going_down) {
				frames_passed++;
				if(frames_passed < 4f) return; // stall slightly before going down
					starting_position = false;
					going_down = true;
					downward_accel += downward_accel / 2f;
					downward_vel += downward_accel;
					Vector3 temp_pos = this.transform.position;
					temp_pos.y += downward_vel;
				if(temp_pos.y < .44f){ // if press has hit ground
					temp_pos.y = .44f;
					going_down = false;
					downward_vel = init_down_vel;
					downward_accel = init_down_accel;
					frames_passed = 0f;
					my_renderer [9].enabled = true;
				}
			
				this.transform.position = temp_pos; // update press position
			}
		}

		// press is coming up
		if (!starting_position && !going_down) {
			frames_passed++;
			if(frames_passed < 60f) return; // stall for 1.5sec when press hits ground
			Vector3 temp_pos = this.transform.position;
			temp_pos.y += upward_vel;
			if(temp_pos.y >= 2.81f){ // if press is back at starting location
				temp_pos.y = 2.81f;
				starting_position = true;
				frames_passed = 0f;
			}
			this.transform.position = temp_pos; // update press position
		}

		// update sprite images according to press position
		float y = this.transform.position.y;
		if (y < 2.77) my_renderer [0].enabled = true;
		else my_renderer [0].enabled = false;
		if (y < 2.50) my_renderer [1].enabled = true;
		else my_renderer [1].enabled = false;
		if (y < 2.31) my_renderer [2].enabled = true;
		else my_renderer [2].enabled = false;
		if (y < 2.07) my_renderer [3].enabled = true;
		else my_renderer [3].enabled = false;
		if (y < 1.83) my_renderer [4].enabled = true;
		else my_renderer [4].enabled = false;
		if (y < 1.59) my_renderer [5].enabled = true;
		else my_renderer [5].enabled = false;
		if (y < 1.35) my_renderer [6].enabled = true;
		else my_renderer [6].enabled = false;
		if (y < 1.118) my_renderer [7].enabled = true;
		else my_renderer [7].enabled = false;
		if (y < .841) my_renderer [8].enabled = true;
		else my_renderer [8].enabled = false;
		if (y < .574) my_renderer [9].enabled = true;
		else my_renderer [9].enabled = false;

	}

}
