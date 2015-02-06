using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Custom_Boss_Handler : MonoBehaviour {
	public float hp = 28, move_speed;
	GameObject health_bar, mega_man;
	public bool flashing = false, stuck = false;
	public GameObject IceBallPrefab;
	PE_Obj peo_boss;
	float hp_fill_start, hp_fill_end, last_shot, time_btwn_shoot, stuck_duration, stuck_start;
	float flash_duration, flash_start, shoot_duration, shoot_start, stomp_duration, stomp_start;
	static public List<GameObject> iceballs;
	bool start, done, additional_random_shot, deflecting, stomping, shooting;
	Animator anim;
	Vector3 spawn1;
	bool dead;
	
	void Start(){
		hp = 28f;
		move_speed = 1.3f;
		iceballs = new List<GameObject>();
		peo_boss = transform.GetComponent<PE_Obj> ();
		mega_man = GameObject.Find ("Mega Man");
		anim = GetComponent<Animator> ();
		hp_fill_start = 0f;
		hp_fill_end = (.075f * 28f);
		start = done = false;
		health_bar = GameObject.Find("Health Bar Boss").gameObject;
		additional_random_shot = false;
		time_btwn_shoot = 3f;
		shoot_duration = .8f;
		stomp_duration = .4f;
		stuck_duration = 2f;
		spawn1.Set (121.67f, -2.12f, -4f);
		flash_duration = .8f;
		dead = deflecting = stomping = shooting = false;
	}
	
	
	void Update(){
		if (dead)return;
		if(mega_man.transform.position.x < 113){
			done = false;
			health_bar.GetComponent<HealthBarBoss> ().decreaseByAmount (28);
			start = false;
			return;
		}
		if (mega_man.transform.position.x > 115.5 && mega_man.transform.position.y <= -2.36 && !done) {
			done = start_routine();
			last_shot = Time.time + .2f;
			return;
		}
		else {
			if(stuck == true) {stuck_start = Time.time; anim.SetBool("Stuck", true); peo_boss.vel.x = 0f;}
			if(shoot_duration + shoot_start < Time.time){ anim.SetBool("Shooting", false); shooting = false;}
			if(stomp_duration + stomp_start < Time.time) {anim.SetBool("Stomp", false); stomping = false;}
			if(stuck_duration + stuck_start < Time.time) {anim.SetBool("Stuck", false); stuck = false;}

			if (done && ((last_shot + time_btwn_shoot <= Time.time) || (additional_random_shot && last_shot + shoot_duration < Time.time))) {
				iceballs.Add(Instantiate(IceBallPrefab) as GameObject);
				peo_boss.vel.x = 0f;
				anim.SetBool("Shooting", true);
				last_shot = Time.time;
				float random = Random.Range(0,6);
				if(random >= 3 && iceballs.Count < 3) additional_random_shot = true;
				else additional_random_shot = false;
			}
			else if(done && Mathf.Abs(mega_man.transform.position.x - transform.position.x) <= 1.3f){
				if(mega_man.transform.position.y <= -1.9){
					GameObject temp = Instantiate(IceBallPrefab) as GameObject;
					temp.GetComponent<IceBall>().stomp = true;
					anim.SetBool("Stomp", true);
					stomping = true;
					peo_boss.vel.x = 0f;
				}
				
			}
			else if(done && MegaMan_Custom.blasters.Count >= 1 && !stuck){
				foreach(GameObject b in MegaMan_Custom.blasters){
					if(Mathf.Abs(b.transform.position.x - transform.position.x) <= 1.5f){
						deflecting = true;
						anim.SetBool("Deflect", true);
						peo_boss.vel.x = 0f;
					}
				}
			}

		}

		if (stuck || shooting || stomping) deflecting = false;

		if (flashing && flash_duration + flash_start < Time.time) {
			StartCoroutine (flash ());
			flash_start = Time.time;
		}
		if (flash_duration + flash_start < Time.time) flashing = false;
		if(mega_man.transform.position.x <= transform.position.x){
			transform.eulerAngles = new Vector3 (0, 180, 0);
			if (!stuck && !shooting && !deflecting && !stomping){
				peo_boss.vel.x = move_speed;
			}
		}
		if(mega_man.transform.position.x > transform.position.x){
			transform.eulerAngles = new Vector3 (0, 0, 0);
			if (!stuck && !shooting && !deflecting && !stomping){
				peo_boss.vel.x = move_speed;
			}
		}
		if (health_bar.GetComponent<HealthBarBoss>().healthUnits.Count <= 0 && done) {
			peo_boss.vel.x = 0f;
			dead = true;
			StartCoroutine(flash_bright());
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
	
	
	
	IEnumerator refill() {
		for(int i = 0; i < 28f; i++) {
			health_bar.GetComponent<HealthBarBoss>().increaseByOne();
			yield return new WaitForSeconds(.08f);
			anim.SetBool("Taunt", false);
		}
	}
	
}
