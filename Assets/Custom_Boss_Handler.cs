using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Custom_Boss_Handler : MonoBehaviour {
	public float hp = 28, move_speed;
	GameObject health_bar, mega_man;
	public bool flashing = false, stuck = false;
	public GameObject IceBallPrefab;
	PE_Obj peo_boss;
	float hp_fill_start, hp_fill_end, time_btwn_shoot, stuck_duration, stuck_start, dodge_dist, deflecting_start, deflecting_duration;
	float flash_duration, flash_start, shoot_duration, shoot_start, stomp_duration, stomp_start, move_speed_dodge;
	static public List<GameObject> iceballs;
	bool start, done, additional_random_shot, deflecting, stomping, shooting, behind;
	Animator anim;
	Vector3 spawn1;
	bool dead, last_positive_move;
	float max_fallbehind_dist = 4f;
	
	void Start(){
		hp = 28f;
		move_speed = .5f;
		move_speed_dodge = 1.3f;
		iceballs = new List<GameObject>();
		peo_boss = transform.GetComponent<PE_Obj> ();
		mega_man = GameObject.Find ("Mega Man");
		anim = GetComponent<Animator> ();
		hp_fill_start = 0f;
		hp_fill_end = (.18f * 28f);
		start = done = false;
		health_bar = GameObject.Find("Health Bar Boss").gameObject;
		additional_random_shot = last_positive_move = false;
		time_btwn_shoot = 4.2f;
		shoot_duration = 1.5f;
		stomp_duration = 3f;
		stuck_duration = 10f;
		deflecting_duration = 1f;
		dodge_dist = 3f;
		spawn1.Set (121.67f, -2.12f, -4f);
		flash_duration = .8f;
		dead = deflecting = stomping = shooting = behind = false;
	}
	
	
	void Update(){

		if (dead)return;
		if(mega_man.transform.position.x < 113){
			done = false;
			health_bar.GetComponent<HealthBarBoss> ().decreaseByAmount (28);
			start = false;
			shoot_start = Time.time + 1f;
			return;
		}
		if (mega_man.transform.position.x > 115.5 && mega_man.transform.position.y <= -2.36 && !done) {
			done = start_routine();
			return;
		}
		else if(done) {
			Vector3 temp2 = transform.position;
			if(transform.position.x < mega_man.transform.position.x - max_fallbehind_dist){
				behind = true;
			}else if(transform.position.x > mega_man.transform.position.x + max_fallbehind_dist){
				behind = true;
			} else behind = false;
			anim.SetBool("Taunt", false);
			if(stuck == true) {stuck_start = Time.time; anim.SetBool("Stuck", true); peo_boss.vel.x = 0f;}
			if(shooting && (shoot_duration + shoot_start < Time.time)){ 
				anim.SetBool("Shooting", false); 
				shooting = false;
				if(!deflecting){
					if(last_positive_move) transform.eulerAngles = new Vector3 (0, 0, 0);
					else transform.eulerAngles = new Vector3 (0, 180, 0);
				}
			}
			if(stomping && (stomp_duration + stomp_start < Time.time)) {anim.SetBool("Stomp", false); stomping = false;}
			if(stuck && (stuck_duration + stuck_start < Time.time)) {anim.SetBool("Stuck", false); stuck = false;}
			if(deflecting && (deflecting_duration + deflecting_start < Time.time)) {
				anim.SetBool("Deflect", false); 
				deflecting = false;
				if(last_positive_move) transform.eulerAngles = new Vector3 (0, 0, 0);
				else transform.eulerAngles = new Vector3 (0, 180, 0);
			}

			if (!deflecting && !stomping && ((shoot_start + time_btwn_shoot <= Time.time) || (additional_random_shot && shoot_start + shoot_duration < Time.time))) {
				if(mega_man.transform.position.x <= transform.position.x) transform.eulerAngles = new Vector3 (0, 180, 0);
				else transform.eulerAngles = new Vector3 (0, 0, 0);
				iceballs.Add(Instantiate(IceBallPrefab) as GameObject);
				peo_boss.vel.x = 0f;
				anim.SetBool("Shooting", true);
				shoot_start = Time.time;
				shooting = true;
				float random = Random.Range(0,6);
				if(random >= 3 && iceballs.Count <= 1 && !mega_man.GetComponent<MegaMan_Custom>().hit_by_ice) additional_random_shot = true;
				else additional_random_shot = false;
			}
			else if(!shooting && Mathf.Abs(mega_man.transform.position.x - transform.position.x) <= 1.3f){
				if(mega_man.transform.position.y <= -1.9 && !stomping){
					GameObject temp = Instantiate(IceBallPrefab) as GameObject;
					temp.GetComponent<IceBall>().stomp = true;
					anim.SetBool("Stomp", true);
					anim.SetBool ("Deflect", false);
					stomping = true;
					stomp_start = Time.time;
					shoot_start = Time.time;
					if(mega_man.transform.position.x <= transform.position.x) transform.eulerAngles = new Vector3 (0, 180, 0);
					else transform.eulerAngles = new Vector3 (0, 0, 0);
					peo_boss.vel.x = 0f;
				}
				
			}
			else if(MegaMan_Custom.blasters.Count >= 1 && !stomping){
				foreach(GameObject b in MegaMan_Custom.blasters){
					if(Mathf.Abs(b.transform.position.x - transform.position.x) <= 1.5f){
						if(b.transform.position.y <= -1.5f){
							deflecting = true;
							deflecting_start = Time.time;
							anim.SetBool("Deflect", true);
							if(transform.position.x < mega_man.transform.position.x) transform.eulerAngles = new Vector3 (0f, 180f, 0f);
							else transform.eulerAngles = new Vector3 (0f, 0f, 0f);
							peo_boss.vel.x = 0f;
							anim.SetBool ("Shooting", false);
							shooting = false;
						}
						if(deflecting){
							foreach(GameObject i in iceballs){
								if(Mathf.Abs(i.transform.position.x - transform.position.x) < 1f){
									iceballs.Remove(i);
									Destroy (i);
									break;
								}
							}


						}
					}
				}
			}
	
			
			if (flashing && flash_duration + flash_start < Time.time) {
				StartCoroutine (flash ());
				flash_start = Time.time;
			}
			if (flash_duration + flash_start < Time.time) flashing = false;



			if (!stuck && !shooting && !deflecting && !stomping){
				if(mega_man.transform.position.y > 0) {
					if(transform.position.x > mega_man.transform.position.x + dodge_dist){
						peo_boss.vel.x = -move_speed_dodge;
						last_positive_move = false;
						transform.eulerAngles = new Vector3 (0, 180, 0);
					} else if(transform.position.x < mega_man.transform.position.x - dodge_dist){
						peo_boss.vel.x = move_speed_dodge;
						last_positive_move = true;
						transform.eulerAngles = new Vector3 (0, 0, 0);
					} else if(last_positive_move){
						peo_boss.vel.x = move_speed_dodge;
					}
					else peo_boss.vel.x = -move_speed_dodge;
				}else {
					if(mega_man.transform.position.x <= transform.position.x){
						transform.eulerAngles = new Vector3 (0, 180, 0);
						last_positive_move = false;
						if(behind) peo_boss.vel.x = -move_speed_dodge;
						else peo_boss.vel.x = -move_speed;
					}
					else{
						transform.eulerAngles = new Vector3 (0, 0, 0);
						last_positive_move = true;
						if(behind) peo_boss.vel.x = move_speed_dodge;
						else peo_boss.vel.x = move_speed;
					}
				}
			}
		
			if (health_bar.GetComponent<HealthBarBoss>().healthUnits.Count <= 0) {
				peo_boss.vel.x = 0f;
				dead = true;
				StartCoroutine(flash_bright());
			}
			anim.SetFloat("Speed", Mathf.Abs(peo_boss.vel.x));
		}
		
	}
	
	IEnumerator flash(){
		gameObject.renderer.material.color = new Color(0.5f, 0.5f, 0.5f, 1.0f);
		yield return new WaitForSeconds(0.16f);
		gameObject.renderer.material.color = new Color(1.1f, 1.1f, 1.1f, 1.0f);
		yield return new WaitForSeconds(.16f);
		
	}
	
	IEnumerator flash_bright(){
		for(int i = 0; i < 10; i++){
			anim.SetBool("dead", true);
			gameObject.renderer.material.color = new Color(1.3f, 1.3f, 1.3f, 1.0f);
			yield return new WaitForSeconds(0.17f);
			gameObject.renderer.material.color = new Color(3f, 3f, 3f, 3f);
			yield return new WaitForSeconds(.17f);
		}
		gameObject.renderer.material.color = new Color(1f, 1f, 1f, 0f);
		yield return new WaitForSeconds(4f);
		PhysEngine.objs.Remove(GetComponent<PE_Obj>());
		Application.LoadLevel (0);
		Destroy(gameObject);
	}
	
	bool start_routine(){
		mega_man.GetComponent<MegaMan_Custom> ().no_movement = true;
		mega_man.GetComponent<PE_Obj> ().still = true;
		Vector3 temp = health_bar.GetComponent<RectTransform>().anchoredPosition3D;
		temp.x = -138f;
		health_bar.GetComponent<RectTransform>().anchoredPosition3D = temp;
		anim.SetBool ("Taunt", true);
		if(!start){StartCoroutine (refill()); hp_fill_start = Time.time; start = true;}
		if(hp_fill_start + hp_fill_end < Time.time){
			mega_man.GetComponent<MegaMan_Custom> ().no_movement = false;
			mega_man.GetComponent<PE_Obj> ().still = false;
			return true;
		} else return false;
	}

	void OnTriggerEnter(Collider other) {
		if (other.GetComponent<SphereCollider> () != null) {
			health_bar.GetComponent<HealthBarBoss> ().decreaseByAmount (7);
			Destroy (other.transform.parent.gameObject);
		}
	}
	
	IEnumerator refill() {
		for(int i = 0; i < 28f; i++) {
			health_bar.GetComponent<HealthBarBoss>().increaseByOne();
			yield return new WaitForSeconds(.08f);
		}
	}
	
}
