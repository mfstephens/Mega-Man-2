using UnityEngine;
using System.Collections;

public class GooHandler : MonoBehaviour {
	Vector3 spawn1, spawn2, spawn3;
	float respawn1_start, respawn2_start, respawn3_start, respawn_wait;
	float ground_duration, fall_speed;
	bool goo1_start, goo2_start, goo3_start, goo1_fall, goo2_fall, goo3_fall;
	GameObject goo1, goo2, goo3, boss;
	public GameObject GooPrefab;
	// Use this for initialization
	void Start () {
		boss = GameObject.Find ("Boss_Custom");
		spawn1.Set (122.7f, .28f, -5f);
		spawn2.Set (125.61f, .28f, -5f);
		spawn3.Set (138.15f, .28f, -5f);
		goo1 = Instantiate(GooPrefab) as GameObject;
		goo2 = Instantiate (GooPrefab) as GameObject;
		goo3 = Instantiate (GooPrefab) as GameObject;
		goo1.transform.position = spawn1;
		goo2.transform.position = spawn2;
		goo3.transform.position = spawn3;
		fall_speed = 1f;
		ground_duration = 4f;
		respawn_wait = 15f;
		goo1_start = goo2_start = goo3_start = true;
	}
	
	// Update is called once per frame
	void Update () {
		if(!goo1_start){
			goo1 = Instantiate(GooPrefab) as GameObject;
			goo1.transform.position = spawn1;
			goo1_start = true;
		}
		if(!goo2_start){
			goo2 = Instantiate(GooPrefab) as GameObject;
			goo2.transform.position = spawn1;
			goo2_start = true;
		}
		if(!goo3_start){
			goo3 = Instantiate(GooPrefab) as GameObject;
			goo3.transform.position = spawn1;
			goo3_start = true;
		}
		goohandle (goo1);
		goohandle (goo2);
		goohandle (goo3);
	}

	
	void goohandle(GameObject goo){
		if (goo.transform.FindChild("BreakablePrefab") == null) {
			if((goo.transform.position == spawn1) && !goo1_fall) {respawn1_start = Time.time; goo1_fall = true;}
			else if((goo.transform.position == spawn2) && !goo2_fall) {respawn2_start = Time.time; goo2_fall = true;}
			else if ((goo.transform.position == spawn3) && !goo3_fall) {respawn3_start = Time.time; goo3_fall = true;}
			Vector3 temp = goo.transform.FindChild("Goo").transform.position;
			if(temp.y > -2.91f){
				temp.y -= fall_speed * Time.deltaTime;
				goo.transform.FindChild("Goo").transform.position = temp;
			} else {
				if(Mathf.Abs ((temp - boss.transform.position).magnitude) < 1.7f) {boss.GetComponent<Custom_Boss_Handler>().stuck = true;}
			}
			if((goo.transform.position == spawn1) && goo1_fall && (respawn1_start + respawn_wait) < Time.time){
				goo1_fall = false;
				goo1_start = false;
				boss.GetComponent<Custom_Boss_Handler>().stuck = false;
				Destroy (goo1);

			}
			if((goo.transform.position == spawn2) && goo2_fall && (respawn2_start + respawn_wait) < Time.time){
				goo2_fall = false;
				goo2_start = false;
				boss.GetComponent<Custom_Boss_Handler>().stuck = false;
				Destroy (goo2);
			}
			if((goo.transform.position == spawn3) && goo3_fall && (respawn3_start + respawn_wait) < Time.time){
				goo3_fall = false;
				goo3_start = false;
				boss.GetComponent<Custom_Boss_Handler>().stuck = false;
				Destroy (goo3);
			}
		}

	}
	
}
