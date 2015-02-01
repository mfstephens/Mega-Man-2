using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public enum WeaponType {
	Blaster,
	CustomWeapon
}

public class MegaMan : MonoBehaviour {
	PE_Obj peo;
	bool facingRight, jumping, enemy_collision, immune, advanced1, advanced2, no_movement;
	float 	jump_start, shoot_start, immunity_start, flash_start, wait;
	Vector3 spawn;
	Animator anim;
	Camera main_cam;
	GameObject health;
	public GameObject blasterPrefab, customWeaponPrefab;
	public float wait_time_left;
	static public List<GameObject> blasters;
	public WeaponType currentWeapon;



	public Vector3 cam_pos_last_frame;
	public Vector3	vel;
	public float    displacementVelX = 0f;
	public bool		grounded = false;
	public float flash_duration =.5f;
	public float immunity_duration = 4f;
	public float collision_duration = .2f;
	public float 	shootingSpeed = 3.7f;
	public float	hSpeed = 1.5f;
	public float	acceleration = 10f;
	public float	jumpVel = 3.1f;
	public float	airSteeringAmt = 1f;
	public float 	air_time = .19f;
	public float	airMomentumX = 1; // 0 for no momentum (i.e. 100% drag), 1 for total momentum
	public float	groundMomentumX = 0.1f;
	
	public Vector2	maxSpeed = new Vector2( 10, 15 ); // Different x & y to limit maximum falling velocity


	// Use this for initialization
	void Start () {
		peo = GetComponent<PE_Obj>();
		anim = GetComponent<Animator> ();
		main_cam = GameObject.Find ("Main Camera").camera;
		cam_pos_last_frame = main_cam.transform.position;
		jumping = false;
		enemy_collision = immune = false;
		health = GameObject.Find ("Health Bar");
		immunity_start = -4f;
		blasters = new List<GameObject>();
		advanced1 = advanced2 = no_movement = false;
		wait = 0f;
	}
	
	// Update is called once per frame
	// Note that we use Update for input but FixedUpdate for physics. This is because Unity input is handled based on Update
	void Update () {
		set_immunity ();
		if (immunity_start <= Time.time) enemy_collision = false;

		if(level_advance_wait() || no_movement) return;
		else{
			vel = peo.vel; // Pull velocity from the PE_Obj
			grounded = (peo.ground != null);
		
			// Horizontal movement
			float vX = Input.GetAxis ("Horizontal"); // Returns a number [-1..1]
			vel.x = vX * hSpeed;

			anim.SetFloat ("speed", Mathf.Abs (vel.x));

			 // set direction to face
			if (vel.x > 0)
				transform.eulerAngles = new Vector3 (0, 0, 0);
			if (vel.x < 0)
				transform.eulerAngles = new Vector3 (0, 180, 0);



			//jump
			if ((Input.GetKeyDown (KeyCode.X) || Input.GetKeyDown (KeyCode.Period)) && grounded) {
				peo.ground = null; // Jumping will set ground = null
				jumping = true;
				jump_start = Time.time;
			}
			if (jumping && ((jump_start + air_time) < Time.time)) {
				jumping = false;
			}
			if (Input.GetKeyUp (KeyCode.X) || Input.GetKeyUp (KeyCode.Period)) {
				jumping = false;
			}
			if (((Input.GetKey (KeyCode.X) || Input.GetKey (KeyCode.Period))) && jumping) {
				vel.y = jumpVel;
			}
			// end jump 

			// shoot
			if ((Input.GetKeyDown (",") || Input.GetKeyDown (KeyCode.Z)) && blasters.Count < 3) {
				anim.SetBool ("shooting", true);
				shoot_start = Time.time;
				if (currentWeapon == WeaponType.Blaster) {
					blasters.Add(Instantiate(blasterPrefab) as GameObject);
				} else if (currentWeapon == WeaponType.CustomWeapon) {
					blasters.Add (Instantiate(customWeaponPrefab) as GameObject);
				}
			}
			if ((shoot_start + .25f) <= Time.time) anim.SetBool ("shooting", false);

			// end shoot

			anim.SetBool ("on_ground", grounded);

			if (grounded && displacementVelX != 0) {
				vel.x += displacementVelX;
			}
	
		}

		if (enemy_collision && !immune) {
			bump_back_and_flash();
		}

		peo.vel = vel;
	} // end Update()

	void FixedUpdate(){

	}
	

	void OnTriggerExit(Collider other){
		if(other.GetComponent<Door_Open>() != null){
			no_movement = false;
			return;
		}
		
	}

	void OnTriggerStay(Collider other){
		OnTriggerEnter (other);
	}

	void OnTriggerEnter(Collider other) {
		PE_Obj otherPEO = other.GetComponent<PE_Obj>();
		if (otherPEO == null) {
			// no movement input allowed when moving through doors
			if(other.GetComponent<Door_Open>() != null){
				no_movement = true;
				return;
			}
			if(other.GetComponent<MrBotHandler>() != null){
				if(!immune){
					enemy_collision = true;
					// 4 damage
					for(int i = 0; i < 4; i++) health.GetComponent<HealthBar>().decreaseByOne();
				}
			}
			return;
		}
	
		// moving platforms only should affect megaman
		if ((this.GetComponent<MegaMan>() != null) && (otherPEO.GetComponent<Platform>() != null)) {
			Platform pf = otherPEO.GetComponent<Platform>() as Platform;
			
			// check what direction platform is moving
			if (pf.type == PlatformType.forward) {
				displacementVelX = pf.speed;
			} else if (pf.type == PlatformType.backward) {
				displacementVelX = -pf.speed;
			} else {
				displacementVelX = 0f;
			}
		}

		if (otherPEO.coll == PE_Collider.press) {
			if(!immune){
				enemy_collision = true;
				// 8 damage
				for(int i = 0; i < 8; i++) health.GetComponent<HealthBar>().decreaseByOne();
			}
		}
		if (otherPEO.coll == PE_Collider.mole) {
			if(!immune){
				enemy_collision = true;
				// 4 damage
				for(int i = 0; i < 4; i++) health.GetComponent<HealthBar>().decreaseByOne();
			}
		}
		if (otherPEO.coll == PE_Collider.pierobot) {
			if(!immune){
				enemy_collision = true;
				// 4 damage
				for(int i = 0; i < 4; i++) health.GetComponent<HealthBar>().decreaseByOne();
			}
		}
		if (otherPEO.coll == PE_Collider.burokki) {
			if(!immune){
				enemy_collision = true;
				// 8 damage
				for(int i = 0; i < 8; i++) health.GetComponent<HealthBar>().decreaseByOne();
			}
		}
		if (otherPEO.coll == PE_Collider.spikewall) {
			health.GetComponent<HealthBar>().empty();
			died ();
		}
		if (otherPEO.coll == PE_Collider.spike) {
			if(!immune) {
				enemy_collision = true;
				health.GetComponent<HealthBar>().decreaseByAmount(4);
			}
		}

	}
	
	void bump_back_and_flash(){
		Color original = gameObject.renderer.material.color;
		Vector3 temp = transform.position;
		if (vel.x > 0) {
			vel.x = -20.0f;
		} else {
			vel.x = 20.0f;
		}
		transform.position = temp;
		StartCoroutine(flash ());
		renderer.material.color = original;
	}
	
	IEnumerator flash() {
		for (int n = 0; n < 20; ++n) {
			gameObject.renderer.material.color = new Color(0.2f, 0.2f, 0.2f, 1.0f);
			yield return new WaitForSeconds(0.1f);
			gameObject.renderer.material.color = new Color(1.0f, 1.0f, 1.0f, 1.0f);
			yield return new WaitForSeconds(0.1f);
		}
	}

	void set_immunity(){
		if (immunity_duration + immunity_start >= Time.time)immune = true;
		if(immunity_duration + immunity_start < Time.time) immune = false;
		if(enemy_collision && !immune) immunity_start = Time.time + collision_duration;
	}

	bool level_advance_wait(){
		if(transform.position.x > 70 && transform.position.y <= -3 && transform.position.y >= -3.5 && !advanced1){
			if(wait + 2f < Time.time) wait = Time.time;
			vel.y = 0;
			vel.x = 0;
			GetComponent<PE_Obj>().still = true;
			if(wait + 1.8f <= Time.time){
				GetComponent<PE_Obj>().still = false;
				advanced1 = true;
				return false;
			} 
			return true;
		}
		// if fall through first jump, put mega man back ontop
		if(transform.position.x > 70 && transform.position.x <= 72 && transform.position.y <= -8.7 && transform.position.y > -14.2 && advanced1){
			Vector3 temp = transform.position;
			temp.y = -8.57f;
			transform.position = temp;
			peo._pos0 = peo.pos1 = temp;
			peo.vel.x = peo.vel0.x = peo.vel.y = peo.vel0.y = 0f;
			peo.ground = GameObject.Find ("Platform13_Backward").GetComponent<PE_Obj>();
		}
		// if fall through first jump, put mega man back ontop
		if(transform.position.x > 72 && transform.position.x < 80 && transform.position.y > -11.8 && advanced1){
			Vector3 temp = transform.position;
			temp.y = -8.57f;
			temp.x = 71.57f;
			transform.position = temp;
			peo._pos0 = peo.pos1 = temp;
			peo.vel.x = peo.vel0.x = peo.vel.y = peo.vel0.y = 0f;
			peo.ground = GameObject.Find ("Platform13_Backward").GetComponent<PE_Obj>();
		}
		
		
		if (transform.position.x > 65.8 && transform.position.x < 73 && transform.position.y <= -11.2 && !advanced2 && transform.position.y >= - 11.7){
			if(wait + 2f < Time.time) wait = Time.time;
			vel.y = 0f;
			vel.x = 0f;
			GetComponent<PE_Obj>().still = true;
			if(wait + 1.8f <= Time.time){
				GetComponent<PE_Obj>().still = false;
				advanced2 = true;
				return false;
			} 
			return true;
		}
		// if fall through second jump, put mega man back ontop
		if(transform.position.x > 65 && transform.position.x < 69 && transform.position.y <= -16.8 && advanced2){
			Vector3 temp = transform.position;
			temp.y = -16.27f;
			transform.position = temp;
			peo._pos0 = peo.pos1 = temp;
			peo.vel.x = peo.vel0.x = peo.vel.y = peo.vel0.y = 0f;
			peo.ground = GameObject.Find ("Platform14_Backward").GetComponent<PE_Obj>();
		}


		// if fall through boss parts, put mega man back ontop
		if(transform.position.x > 135 && transform.position.x < 142 && transform.position.y < -14.85){
			Vector3 temp = transform.position;
			temp.y = -14.75f;
			transform.position = temp;
			peo._pos0 = peo.pos1 = temp;
			peo.vel.x = peo.vel0.x = peo.vel.y = peo.vel0.y = 0f;
			peo.ground = GameObject.Find ("Ground35").GetComponent<PE_Obj>();
		}





		if(transform.position.x > 142 && transform.position.y <= -16.8){
			Vector3 temp = transform.position;
			temp.y = -16.7f;
			transform.position = temp;
			peo._pos0 = peo.pos1 = temp;
			peo.vel.x = peo.vel0.x = peo.vel.y = peo.vel0.y = 0f;
			peo.ground = GameObject.Find ("Boss_Platform").GetComponent<PE_Obj>();
		}

		return false;
	}

	void died(){
//		float temp = Time.time;	
//		while (temp + 3.5f >= Time.time) {};
		Application.LoadLevel (Application.loadedLevel);
	}



}

