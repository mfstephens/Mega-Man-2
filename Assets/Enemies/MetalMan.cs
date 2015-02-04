using UnityEngine;
using System.Collections;
using System.Collections.Generic;


public class MetalMan : MonoBehaviour {
	public float hp = 28;
	GameObject health_bar, mega_man;
	public bool flashing = false;
	public GameObject RazorPrefab;
	float hp_fill_start, hp_fill_end, last_throw, time_btwn_throw, time_btwn_wait, last_jump;
	float jump_wait, jump_wait_when_attacked, x_slow_rate, reverse_pform_start, try_reverse_pform, flash_duration, flash_start;
	public float jump_vel, vel;
	static public List<GameObject> razors;
	bool start, done, throwing, additional_random_throw, pos1, in_transition;
	Animator anim;
	Vector3 spawn1, spawn2, spawn3;
	Platform metal_man_platform;
	GameObject r_arrow1, r_arrow2, l_arrow1, l_arrow2;
	bool dead;

	void Start(){
		hp = 28f;
		razors = new List<GameObject>();
		mega_man = GameObject.Find ("Mega Man");
		anim = GetComponent<Animator> ();
		hp_fill_start = 0f;
		hp_fill_end = (.075f * 28f);
		start = done = false;
		health_bar = GameObject.Find("Health Bar Boss").gameObject;
		additional_random_throw = false;
		time_btwn_throw = .46f;
		time_btwn_wait = 2.4f;
		jump_vel = 7.1f;
		jump_wait = 2.1f;
		x_slow_rate = 2f;
		jump_wait_when_attacked = 1.5f;
		spawn1.Set (148.22f, -16.68f, -3.15f);
		spawn2.Set (143.59f, -16.68f, -3.15f);
		spawn3.Set (143.59f, -30f, -3f);
		in_transition = false;
		try_reverse_pform = 4.5f;
		pos1 = true;
		flash_duration = .5f;
		metal_man_platform = GameObject.Find ("Boss_Platform").GetComponent<Platform> ();
		dead = false;
		r_arrow1 = GameObject.Find ("arrow_right1");
		r_arrow2 = GameObject.Find ("arrow_right1");
		l_arrow1 = GameObject.Find ("arrow_left1");
		l_arrow2 = GameObject.Find ("arrow_left2");
		l_arrow1.renderer.material.color = l_arrow2.renderer.material.color = new Color (1f, 1f, 1f, 0f);
	}


	void Update(){
		if (dead)return;
		if(mega_man.transform.position.x < 142){
			done = false;
			health_bar.GetComponent<HealthBarBoss> ().decreaseByAmount (28);
			start = false;
			return;
		}
		if (mega_man.transform.position.x > 142.9 && mega_man.transform.position.y <= -16.65 && !done) {
			done = start_routine();
			last_throw = Time.time + .2f;
			last_jump = Time.time + 2f;
			reverse_pform_start = Time.time;
			return;
		}
		else {
			if (done && ((last_throw + time_btwn_wait <= Time.time) || (additional_random_throw && last_throw + time_btwn_throw <Time.time))) {
				razors.Add(Instantiate(RazorPrefab) as GameObject);
				anim.SetBool("Throwing", true);
				last_throw = Time.time;
				float random = Random.Range(0,6);
				if(random >= 3 && razors.Count < 3) additional_random_throw = true;
				else additional_random_throw = false;
			}
			else if(done && MegaMan.blasters.Count >= 1 && razors.Count < 2 && (last_throw + time_btwn_throw < Time.time)){
				razors.Add(Instantiate(RazorPrefab) as GameObject);
				anim.SetBool("Throwing", true);
				last_throw = Time.time;
				float random = Random.Range(0,6);
				if(random >= 3 && razors.Count < 3) additional_random_throw = true;
				else additional_random_throw = false;
			}
			if(done && Mathf.Abs(mega_man.transform.position.x - transform.position.x) <= 2.1f){
				if(pos1 && GetComponent<PE_Obj>().ground){
					GetComponent<PE_Obj>().vel.y = jump_vel + 2f;
					GetComponent<PE_Obj>().vel.x = -3.5f;
					last_jump = Time.time;
					in_transition = true;

				} else if(!pos1 && GetComponent<PE_Obj>().ground){
					GetComponent<PE_Obj>().vel.y = jump_vel + 2f;
					GetComponent<PE_Obj>().vel.x = 3.5f;
					last_jump = Time.time;
					in_transition = true;
				}

			}
			if(done && last_jump + jump_wait < Time.time && GetComponent<PE_Obj>().ground){
				GetComponent<PE_Obj>().vel.y = jump_vel;
				last_jump = Time.time;
			} else if(done && MegaMan.blasters.Count >= 1 && jump_wait_when_attacked + last_jump < Time.time && GetComponent<PE_Obj>().ground){
				GetComponent<PE_Obj>().vel.y = jump_vel;
				last_jump = Time.time;
			}
			if(in_transition && !pos1){
				if(transform.position.x >= spawn1.x){
					transform.position = spawn1;
					in_transition = false;
					pos1 = true;
					GetComponent<PE_Obj>().vel.x = 0f;
				}
				else GetComponent<PE_Obj>().vel.x -= x_slow_rate * Time.deltaTime;
			}
			else if(in_transition && pos1){
				if(transform.position.x <= spawn2.x){
					transform.position = spawn2;
					in_transition = false;
					pos1 = false;
					GetComponent<PE_Obj>().vel.x = 0f;
				} else GetComponent<PE_Obj>().vel.x += x_slow_rate * Time.deltaTime;
			}
		}

		if (reverse_pform_start + try_reverse_pform <= Time.time) {
			reverse_pform_start = Time.time;
			float random = Random.Range(0,4);
			if(random >= 2){
				if(metal_man_platform.type == PlatformType.backward){
					metal_man_platform.type = PlatformType.forward;
					l_arrow1.renderer.material.color = l_arrow2.renderer.material.color = new Color (1f, 1f, 1f, 0f);
					r_arrow1.renderer.material.color = r_arrow2.renderer.material.color = new Color (1f, 1f, 1f, 1f);
				}
				else {
					metal_man_platform.type = PlatformType.backward;
					l_arrow1.renderer.material.color = l_arrow2.renderer.material.color = new Color (1f, 1f, 1f, 1f);
					r_arrow1.renderer.material.color = r_arrow2.renderer.material.color = new Color (1f, 1f, 1f, 0f);
				}
			}
		}
		if (flashing && flash_duration + flash_start < Time.time) {
			StartCoroutine (flash ());
			flash_start = Time.time;
		}
		if (flash_duration + flash_start < Time.time) flashing = false;
		if(mega_man.transform.position.x <= transform.position.x) transform.eulerAngles = new Vector3 (0, 0, 0);
		if(mega_man.transform.position.x > transform.position.x) transform.eulerAngles = new Vector3 (0, 180, 0);
		anim.SetBool ("Grounded", GetComponent<PE_Obj> ().ground);
		if ((last_throw + time_btwn_throw) < Time.time) anim.SetBool ("Throwing", false);
		if (health_bar.GetComponent<HealthBarBoss>().healthUnits.Count <= 0 && done) {
			dead = true;
			audio.Play ();
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
	
}

