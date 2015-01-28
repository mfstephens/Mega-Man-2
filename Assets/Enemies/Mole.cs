using UnityEngine;
using System.Collections;

public class Mole : MonoBehaviour {
	public GameObject MolePrefab;
	GameObject mega_man;
	public float respawn_rate1 = 8f;
	public float respawn_rate2 = 9f;
	public float respawn_rate3 = 11f;
	public float spawn_depth = 1f;
	public float mole_vel = .8f;
	float group1_spawn, group2_spawn, group3_spawn;
	float g4_top, g5_left, g5_top, g5_right, g6_left, g6_top, g7_left, g7_top;
	float g40_bottom, g41_left, g41_bottom, g41_right, g42_left, g42_bottom, g42_right, g43_left, g43_bottom, g43_right;
	// Use this for initialization
	void Start () {
		mega_man = GameObject.Find ("Mega Man");
		group1_spawn = group2_spawn = group3_spawn = 0f;
		GameObject g4 = GameObject.Find ("Ground4");
		GameObject g5 = GameObject.Find ("Ground5");
		GameObject g6 = GameObject.Find ("Ground6");
		GameObject g7 = GameObject.Find ("Ground7");
		GameObject g40 = GameObject.Find ("Ground40");
		GameObject g41 = GameObject.Find ("Ground41");
		GameObject g42 = GameObject.Find ("Ground42");
		GameObject g43 = GameObject.Find ("Ground43");
		g5_left = g5.transform.position.x - g5.transform.lossyScale.x / 2;
		g5_top = g5.transform.position.y + g5.transform.lossyScale.y / 2;
		g5_right = g5.transform.position.x + g5.transform.lossyScale.x / 2;
		g6_left = g6.transform.position.x - g6.transform.lossyScale.x / 2;
		g6_top = g6.transform.position.y + g6.transform.lossyScale.y / 2;
		g7_left = g7.transform.position.x - g7.transform.lossyScale.x / 2;
		g7_top = g7.transform.position.y + g7.transform.lossyScale.y / 2;
		g4_top = g4.transform.position.y + g4.transform.lossyScale.y / 2;
		g41_left = g41.transform.position.x - g41.transform.lossyScale.x / 2;
		g41_bottom = g41.transform.position.y - g41.transform.lossyScale.y / 2;
		g41_right = g41.transform.position.x + g41.transform.lossyScale.x / 2;
		g42_left = g42.transform.position.x - g42.transform.lossyScale.x / 2;
		g42_bottom = g42.transform.position.y - g42.transform.lossyScale.y / 2;
		g42_right = g42.transform.position.x + g42.transform.lossyScale.x / 2;
		g43_left = g43.transform.position.x - g43.transform.lossyScale.x / 2;
		g43_bottom = g43.transform.position.y - g43.transform.lossyScale.y / 2;
		g43_right = g43.transform.position.x + g43.transform.lossyScale.x / 2;
		g40_bottom = g40.transform.position.y - g40.transform.lossyScale.y / 2;
	}


	void FixedUpdate(){
		Vector3 mm_pos = mega_man.transform.position;
		if(mm_pos.x < 28.72f) group1_spawn = group2_spawn = group3_spawn = Time.time - respawn_rate1*1.25f;
		if (mm_pos.x >= 29.72f && mm_pos.x < 42.57) {
			// spawn moles
			mole_spawn();
		}

		//update velocity
		foreach (GameObject mole in GameObject.FindGameObjectsWithTag("mr_mole_up")) {
			Vector3 mole_pos = mole.transform.position;
			mole_pos.y += mole_vel * Time.deltaTime;
			mole.transform.position = mole_pos;
			if(mole_pos.y > 5 || mole_pos.y < -5){
				PhysEngine.objs.Remove(mole.GetComponent<PE_Obj>());
				Destroy(mole);
			}
		}
		foreach (GameObject mole in GameObject.FindGameObjectsWithTag("mr_mole_down")) {
			Vector3 mole_pos = mole.transform.position;
			mole_pos.y -= mole_vel * Time.deltaTime;
			mole.transform.position = mole_pos;
			if(mole_pos.y > 5 || mole_pos.y < -5){
				PhysEngine.objs.Remove(mole.GetComponent<PE_Obj>());
				Destroy(mole);
			}
		}


	}

	// Update is called once per frame
	void Update () {
	
	}


	void mole_spawn(){
		Vector3 mole_pos = mega_man.transform.position;
		 if((group1_spawn + respawn_rate1) <= Time.time) {
			group1_spawn = Time.time;
			GameObject mole1 = Instantiate(MolePrefab) as GameObject;
			mole_pos = mega_man.transform.position;
			mole_pos.x += 1f;
			mole1.transform.position = mole_pos;
			mole1.tag = "mr_mole_up";
			mole1.transform.position = spawn_location(mole1);
			GameObject mole2 = Instantiate(MolePrefab) as GameObject;
			mole_pos = mega_man.transform.position;
			mole_pos.x += 3f;
			mole2.transform.position = mole_pos;
			mole2.tag = "mr_mole_up";
			mole2.transform.position = spawn_location(mole2);
		} 
		 if((group2_spawn + respawn_rate2) <= Time.time) {
			group2_spawn = Time.time;
			GameObject mole1 = Instantiate(MolePrefab) as GameObject;
			mole_pos = mega_man.transform.position;
			mole_pos.x += 2f;
			mole1.transform.position = mole_pos;
			mole1.tag = "mr_mole_down";
			mole1.transform.position = spawn_location(mole1);
			mole1.transform.eulerAngles = new Vector3 (180, 0, 0);
			GameObject mole2 = Instantiate(MolePrefab) as GameObject;
			mole_pos = mega_man.transform.position;
			mole_pos.x += 4f;
			mole2.transform.position = mole_pos;
			mole2.tag = "mr_mole_down";
			mole2.transform.position = spawn_location(mole2);
			mole2.transform.eulerAngles = new Vector3 (180, 0, 0);
		} 
		if((group3_spawn + respawn_rate3) <= Time.time) {
			group3_spawn = Time.time;
			GameObject mole1 = Instantiate(MolePrefab) as GameObject;
			mole_pos = mega_man.transform.position;
			mole_pos.x += 1.5f;
			mole1.transform.position = mole_pos;
			mole1.tag = "mr_mole_down";
			mole_pos = spawn_location(mole1);
			mole_pos.y += .5f;
			mole1.transform.position = mole_pos;
			mole1.transform.eulerAngles = new Vector3 (180, 0, 0);
			GameObject mole2 = Instantiate(MolePrefab) as GameObject;
			mole_pos = mega_man.transform.position;
			mole_pos.x += 3.3f;
			mole2.transform.position = mole_pos;
			mole2.tag = "mr_mole_up";
			mole_pos = spawn_location(mole2);
			mole_pos.y -= .5f;
			mole2.transform.position = mole_pos;
		} 
	}

	Vector3 spawn_location (GameObject mole){
		Vector3 mole_pos = mole.transform.position;
		bool going_up = false;
		if (mole.tag == "mr_mole_up") going_up = true;
		if(going_up){
			if(mole_pos.x >= g5_left && mole_pos.x < g5_right){
				mole_pos.y = g5_top - spawn_depth;
			}
			else if(mole_pos.x >= g6_left && mole_pos.x < g7_left){
				mole_pos.y = g6_top - spawn_depth;
			}
			else if(mole_pos.x > g7_left){
				mole_pos.y = g7_top - spawn_depth;
			}
			else{
				mole_pos.y = g4_top - spawn_depth;
			}
		} else {
			if(mole_pos.x >= g43_left && mole_pos.x < g43_right){
				mole_pos.y = g43_bottom + spawn_depth;
			}
			else if(mole_pos.x >= g42_left && mole_pos.x < g42_right){
				mole_pos.y = g42_bottom + spawn_depth;
			}
			else if(mole_pos.x >= g41_left && mole_pos.x < g41_right){
				mole_pos.y = g41_bottom + spawn_depth;
			}
			else{
				mole_pos.y = g40_bottom + spawn_depth;
			}
		}

		return mole_pos;
	}
}
