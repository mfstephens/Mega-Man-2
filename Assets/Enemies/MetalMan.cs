using UnityEngine;
using System.Collections;
using System.Collections.Generic;


public class MetalMan : MonoBehaviour {
	public float hp = 28;
	GameObject health_bar, mega_man;
	public GameObject RazorPrefab;
	float hp_fill_start, hp_fill_end, last_throw, time_btwn_throw, time_btwn_wait, last_jump, jump_wait;
	public float jump_vel, vel;
	static public List<GameObject> razors;
	bool start, done, throwing;
	Animator anim;


	void Start(){
		hp = 28f;
		razors = new List<GameObject>();
		mega_man = GameObject.Find ("Mega Man");
		anim = GetComponent<Animator> ();
		hp_fill_start = 0f;
		hp_fill_end = (.075f * 28f);
		start = done = false;
		health_bar = GameObject.Find("Health Bar Boss").gameObject;
		bool throwing = false;
		time_btwn_throw = .65f;
		time_btwn_wait = 4.5f;
		jump_vel = 20f;
		jump_wait = 1f;

		// health_bar.GetComponent<HealthBarBoss> ().decreaseByAmount (28);
	}


	void Update(){
		if (mega_man.transform.position.x > 142 && mega_man.GetComponent<PE_Obj>().ground && !done) {
			done = start_routine();
			last_throw = Time.time + 1f;
			last_jump = Time.time + 2f;
			return;
		}
		else if (done && last_throw < Time.time - time_btwn_wait) {
			razors.Add(Instantiate(RazorPrefab) as GameObject);
			anim.SetBool("Throwing", true);
			last_throw = Time.time;
			float random = Random.Range(0,3);
			if(random > 2 && razors.Count < 2) last_throw = (Time.time + time_btwn_wait - time_btwn_throw);
		}
		else if(done && MegaMan.blasters.Count >= 1 && razors.Count < 3 && (last_throw + time_btwn_throw < Time.time)){
			razors.Add(Instantiate(RazorPrefab) as GameObject);
			anim.SetBool("Throwing", true);
			last_throw = Time.time;
		}
		else if(done && last_jump + jump_wait < Time.time){
			GetComponent<PE_Obj>().vel.y = jump_vel;
			last_jump = Time.time;

		}










		anim.SetBool ("Grounded", GetComponent<PE_Obj> ().ground);
		if ((last_throw + .1f) <= Time.time) anim.SetBool ("Throwing", false);
		if (hp <= 0 && done) {
			PhysEngine.objs.Remove(GetComponent<PE_Obj>());
			Destroy(gameObject);
		}
		
	}


	bool start_routine(){
		mega_man.GetComponent<MegaMan> ().no_movement = true;
		mega_man.GetComponent<PE_Obj> ().still = true;
		Vector3 temp = health_bar.GetComponent<RectTransform>().anchoredPosition3D;
		temp.x = -138f;
		health_bar.GetComponent<RectTransform>().anchoredPosition3D = temp;
		anim.SetBool ("Grounded", GetComponent<PE_Obj> ().ground);
		if(!start){StartCoroutine (refill()); hp_fill_start = Time.time; start = true;}
		if(hp_fill_start + hp_fill_end < Time.time){
			mega_man.GetComponent<MegaMan> ().no_movement = false;
			mega_man.GetComponent<PE_Obj> ().still = false;
			return true;
		} else return false;
	}

	

	IEnumerator refill() {
		for(int i = 0; i < 28f; i++) {
			health_bar.GetComponent<HealthBarBoss>().increaseByOne();
			yield return new WaitForSeconds(.07f);
		}
	}

	public void DecrementHP(){
		hp--;
	}
}

