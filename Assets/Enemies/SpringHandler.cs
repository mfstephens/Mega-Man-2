using UnityEngine;
using System.Collections;

public class SpringHandler : MonoBehaviour {
	GameObject mega_man;
	bool spring1, spring_anim, left, fast;
	Animator anim;
	PE_Obj ground33, ground34;
	float spring_anim_start, spring_anim_duration, speed, fast_speed, height_raise, norm_height;
	// Use this for initialization
	void Start () {
		mega_man = GameObject.Find ("Mega Man");
		anim = GetComponent<Animator> ();
		if (transform.position.y < -16.4f) spring1 = true;
		else spring1 = false;
		spring_anim_duration = 2.5f;
		spring_anim_start = 0f;
		spring_anim = false;
		speed = .5f;
		fast_speed = 3f;
		height_raise = .3f;
		norm_height = transform.position.y;
		left = true;
		fast = false;
		ground33 = GameObject.Find("Ground33").GetComponent<PE_Obj>();
		ground34 = GameObject.Find("Ground34").GetComponent<PE_Obj>();
	}
	
	// Update is called once per frame
	void FixedUpdate () {
		if (spring_anim_duration + spring_anim_start > Time.time) return;
		else {
			Vector3 temp = transform.position;
			temp.y = norm_height;
			transform.position = temp;
			anim.SetBool ("springy", false);
			if(spring1) {
				if(mega_man.GetComponent<PE_Obj>().ground == ground33){
					fast = true;
				} else fast = false;
			}
			if(!spring1) {
				if(mega_man.GetComponent<PE_Obj>().ground == ground34){
					fast = true;
				} else fast = false;
			}

			if(left){
				if(spring1 && temp.x > 114.1) {
					if(fast) temp.x -= fast_speed * Time.deltaTime;
					else temp.x -= speed * Time.deltaTime;
				}
				if(spring1 && temp.x <= 114.1){
					left = false; 
					if(fast) temp.x += fast_speed * Time.deltaTime;
					else temp.x += speed * Time.deltaTime;
				}
				if(!spring1 && temp.x > 119.4){
					if(fast) temp.x -= fast_speed * Time.deltaTime;
					else temp.x -= speed * Time.deltaTime;
				}
				if(!spring1 && temp.x <= 119.4){
					left = false; 
					if(fast) temp.x += fast_speed * Time.deltaTime;
					else temp.x += speed * Time.deltaTime;
				}
				transform.position = temp;
				if (left)
					transform.eulerAngles = new Vector3 (0, 0, 0);
				if (!left)
					transform.eulerAngles = new Vector3 (0, 180, 0);
				return;
			} 
			if(!left){
				if(spring1 && temp.x >= 118.75) {
					if(fast) temp.x -= fast_speed * Time.deltaTime;
					else temp.x -= speed * Time.deltaTime;
					left = true;
				}
				if(spring1 && temp.x <118.75){
					if(fast) temp.x += fast_speed * Time.deltaTime;
					else temp.x += speed * Time.deltaTime;
				}
				if(!spring1 && temp.x >= 126.37){
					if(fast) temp.x -= fast_speed * Time.deltaTime;
					else temp.x -= speed * Time.deltaTime;
					left = true;
				}
				if(!spring1 && temp.x <	126.37) {
					if(fast) temp.x += fast_speed * Time.deltaTime;
					else temp.x += speed * Time.deltaTime;
				}
				if (left)
					transform.eulerAngles = new Vector3 (0, 0, 0);
				if (!left)
					transform.eulerAngles = new Vector3 (0, 180, 0);
				transform.position = temp;
			}
		}
	}

	void OnTriggerStay(Collider other){
		OnTriggerEnter (other);
	}
	
	void OnTriggerEnter(Collider other) {
		PE_Obj otherPEO = other.GetComponent<PE_Obj>();
		if (otherPEO == null) return;
		else if (otherPEO.coll == PE_Collider.megaman){
			anim.SetBool("springy", true);
			if(spring_anim_duration + spring_anim_start < Time.time){
				spring_anim_start = Time.time;
				Vector3 temp = transform.position;
				temp.y += height_raise;
				transform.position = temp;
				StartCoroutine(start_anim());
			}
		}
	}

	IEnumerator start_anim() {
		for(float i = 0f; i < 15; i++) {
			transform.eulerAngles = new Vector3 (0, 180, 0);
			yield return new WaitForSeconds(0.14f);
			transform.eulerAngles = new Vector3 (0, 0, 0);
			yield return new WaitForSeconds(.14f);
		}
	}
}
