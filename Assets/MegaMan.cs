using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class MegaMan : MonoBehaviour {
	PE_Obj peo;
	bool facingRight, jumping, enemy_collision, immune, advanced1, advanced2, coroutine, respawning;
	float jump_start, shoot_start, immunity_start, flash_start, start_wait, collision_anim, respawn_wait_time;
	Vector3 spawn1, spawn2, spawn3;
	Animator anim;
	Camera main_cam;
	public GameObject health;

	public GameObject blasterPrefab, customWeaponPrefab;
	public float num_energy_tanks = 0f;
	public float num_lives = 3f;
	public float wait_time_left;
	static public List<GameObject> blasters;
	public WeaponType currentWeapon;
	public AudioSource[] sounds;


	public Vector3 cam_pos_last_frame;
	public Vector3	vel;
	public float    displacementVelX = 0f;
	public bool		grounded = false;
	public bool	 no_movement;
	public float flash_duration =.5f;
	public float immunity_duration = 2.7f;
	public float collision_duration = .05f;
	public float 	shootingSpeed = 2.91f;
	public float	hSpeed = 1.32f;
	public float	acceleration = 0f;
	public float	jumpVel = 3.2f;
	public float	airSteeringAmt = 1f;
	public float 	air_time = .2f;
	public float	airMomentumX = 1; // 0 for no momentum (i.e. 100% drag), 1 for total momentum
	public float	groundMomentumX = 0.1f;
	public Vector2	maxSpeed = new Vector2( 10, 15 ); // Different x & y to limit maximum falling velocity


	// Use this for initialization
	void Start () {
		peo = GetComponent<PE_Obj>();
		anim = GetComponent<Animator> ();
		main_cam = GameObject.Find ("Main Camera").camera;
		sounds = GetComponents<AudioSource>();
		cam_pos_last_frame = main_cam.transform.position;
		jumping = false;
		enemy_collision = immune = false;
		health = GameObject.Find ("Health Bar");
		immunity_start = -4f;
		blasters = new List<GameObject>();
		advanced1 = advanced2 = no_movement = respawning = false;
		start_wait= 0f;
		respawn_wait_time = 4.3f;
		collision_anim = .4f;
		coroutine = false;
		spawn1.Set (-.3f, 1.32f, -3f);
		spawn2.Set (66.72f, -16.2f, -3f);
		spawn3.Set (138.5f, -14.78f, -3.1f);
	}

	// Update is called once per frame
	// Note that we use Update for input but FixedUpdate for physics. This is because Unity input is handled based on Update
	void Update () {
		set_immunity ();
		if (immunity_start <= Time.time) enemy_collision = false;
		check_health_lives_and_respawn_or_die ();

		if(level_advance_wait() || no_movement || respawning ) return;
		else{
			coroutine = false;
			vel = peo.vel; // Pull velocity from the PE_Obj
			if(!grounded && peo.ground) sounds[3].Play ();
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


		set_immunity ();
		if (immunity_start <= Time.time) enemy_collision = false;
		if (enemy_collision && !immune) {
			no_movement = true;
			bump_back_and_flash();
		}
	}


	void OnTriggerExit(Collider other){
		if(other.GetComponent<Door_Open>() != null){
			no_movement = false;
			return;
		}
		if(other.GetComponent<Door_Close>() != null){
			no_movement = false;
			return;
		}
		if(other.GetComponent<Door_Open_Close>() != null){
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
			if(other.GetComponent<Door_Close>() != null){
				no_movement = true;
				return;
			}
			if(other.GetComponent<Door_Open_Close>() != null){
				no_movement = true;
				return;
			}
			if(other.GetComponent<SpringHandler>() != null){
				if(!immune && immunity_start < Time.time){
					enemy_collision = true;
					// 4 damage
					for(int i = 0; i < 4; i++) health.GetComponent<HealthBar>().decreaseByOne();
					immunity_start = Time.time + collision_duration;
					sounds[1].Play ();
				}
			}

			if(other.GetComponent<MrBotHandler>() != null){
				if(!immune && immunity_start < Time.time){
					enemy_collision = true;
					// 4 damage
					for(int i = 0; i < 4; i++) health.GetComponent<HealthBar>().decreaseByOne();
					immunity_start = Time.time + collision_duration;
					sounds[1].Play ();
				}
			}
			if(other.GetComponent<Razor>() != null){
				if(!immune && immunity_start < Time.time){
					enemy_collision = true;
					// 4 damage
					for(int i = 0; i < 4; i++) health.GetComponent<HealthBar>().decreaseByOne();
					immunity_start = Time.time + collision_duration;
					sounds[1].Play ();
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

		if (GetComponent<PE_Obj>().still) return; //still is set when picking up health pellets, and no damage should be taken while frozen
		else if (otherPEO.coll == PE_Collider.press) {
			if(!immune && immunity_start < Time.time){
				enemy_collision = true;
				// 8 damage
				for(int i = 0; i < 8; i++) health.GetComponent<HealthBar>().decreaseByOne();
				immunity_start = Time.time + collision_duration;
				sounds[1].Play ();
			}
		}
		else if (otherPEO.coll == PE_Collider.mole) {
			if(!immune && immunity_start < Time.time){
				enemy_collision = true;
				// 4 damage
				for(int i = 0; i < 4; i++) health.GetComponent<HealthBar>().decreaseByOne();
				immunity_start = Time.time + collision_duration;
				sounds[1].Play ();
			}
		}
		else if (otherPEO.coll == PE_Collider.pierobot) {
			if(!immune && immunity_start < Time.time){
				enemy_collision = true;
				// 4 damage
				for(int i = 0; i < 4; i++) health.GetComponent<HealthBar>().decreaseByOne();
				immunity_start = Time.time + collision_duration;
				sounds[1].Play ();
			}
		}
		else if (otherPEO.coll == PE_Collider.burokki) {
			if(!immune && immunity_start < Time.time){
				enemy_collision = true;
				// 8 damage
				for(int i = 0; i < 8; i++) health.GetComponent<HealthBar>().decreaseByOne();
				immunity_start = Time.time + collision_duration;
				sounds[1].Play ();
			}
		}
		if (otherPEO.coll == PE_Collider.spikewall) {
			health.GetComponent<HealthBar>().empty();
			died ();
		}
		if (otherPEO.coll == PE_Collider.spike) {
			health.GetComponent<HealthBar>().empty ();
			died ();
		}

	}

	void bump_back_and_flash(){
		Color original = gameObject.renderer.material.color;
		anim.SetBool ("enemy_collision", true);
		if (vel.x >= 0) {
			peo.vel.x = -3.5f;
			peo.vel.y = 2.2f;
		} else {
			peo.vel.x = 3.5f;
			peo.vel.y = 2.2f;
		}
		if(!coroutine){ StartCoroutine(flash ()); coroutine = true;}
		renderer.material.color = original;
	}

	IEnumerator flash() {
		for (int n = 0; n < 12; ++n) {
			gameObject.renderer.material.color = new Color(0.2f, 0.2f, 0.2f, 1.0f);
			yield return new WaitForSeconds(0.1f);
			gameObject.renderer.material.color = new Color(1.0f, 1.0f, 1.0f, 1.0f);
			yield return new WaitForSeconds(0.1f);
		}
	}

	void set_immunity(){
		if (immunity_start <= Time.time &&( immunity_start + immunity_duration) > Time.time){
			immune = true;
			no_movement = false;
		}
		if(immunity_start + collision_anim <= Time.time) anim.SetBool ("enemy_collision", false);
		if(immunity_duration + immunity_start < Time.time) immune = false;
	}

	bool level_advance_wait(){
		if(transform.position.x > 70 && transform.position.y <= -3 && transform.position.y >= -3.5 && !advanced1){
			if(start_wait + 2.2f < Time.time) start_wait = Time.time;
			vel.y = 0;
			vel.x = 0;
			GetComponent<PE_Obj>().still = true;
			if(start_wait + 1.8f <= Time.time){
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
			if(start_wait + 2.2f < Time.time) start_wait = Time.time;
			vel.y = 0f;
			vel.x = 0f;
			GetComponent<PE_Obj>().still = true;
			if(start_wait + 1.8f <= Time.time){
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

		if (transform.position.x > 142.5 && transform.position.y < -15.1){
			sounds[0].Stop();
			if(!sounds[2].isPlaying && !respawning)sounds[2].Play ();
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

		if (transform.position.x > 149.5 && transform.position.y <= -15) {
			Vector3 temp = transform.position;
			temp.x = 148.8f;
			temp.y = 16.66f;
			transform.position = temp;
			peo._pos0 = peo.pos1 = temp;
			peo.vel.x = peo.vel0.x = peo.vel.y = peo.vel0.y = 0f;
			peo.ground = GameObject.Find ("Boss_Platform").GetComponent<PE_Obj>();
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
	} // end level_advanced_wait

	void check_health_lives_and_respawn_or_die(){
		if((transform.position.x < 65.38 && transform.position.y < -4)
		   || (transform.position.x < 130 && transform.position.y < -19)
		   || (transform.position.x < 160 && transform.position.y < -19)) goto start_resp;
		if (health.GetComponent<HealthBar>().healthUnits.Count > 0) return;
	start_resp:
		if (num_lives > 0 && !respawning) {
			sounds[0].Stop ();
			sounds[2].Stop ();
			sounds[4].Play ();
			num_lives--;
			respawning = true;
			start_wait = Time.time;
			StartCoroutine(flash_bright());
			GetComponent<PE_Obj>().still = true;
			peo.coll = PE_Collider.burokki; //random one with physics and not mega-man so cant get hit
			no_movement = true;
			peo.vel.x = 0;
			peo.vel.y = 0;
		} 
		else if(respawning && start_wait + respawn_wait_time > Time.time) return;
		else if(respawning && start_wait + respawn_wait_time < Time.time){
			if(transform.position.x < 74 && transform.position.x > -5){
				transform.position = spawn1;
				peo._pos0 = spawn1;
				peo._pos1 = spawn1;
			}
			else if(transform.position.x >= 74 && transform.position.x < 130){
				transform.position = spawn2;
				peo._pos0 = spawn2;
				peo._pos1 = spawn2;
			}
			else if(transform.position.x >= 130 && transform.position.x < 160){
				transform.position = spawn3;
				peo._pos0 = spawn3;
				peo._pos1 = spawn3;
			}
			sounds[0].Play ();
			health.GetComponent<HealthBar>().increaseByAmount(28);
			GetComponent<PE_Obj>().still = false;
			no_movement = false;
			respawning = false;
			peo.coll = PE_Collider.megaman;
			gameObject.renderer.material.color = new Color(1f, 1f, 1f, 1.0f);
		} else died ();	
	}


	IEnumerator flash_bright(){
		for(int i = 0; i < 6; i++){
			gameObject.renderer.material.color = new Color(1.3f, 1.3f, 1.3f, 1.0f);
			yield return new WaitForSeconds(0.32f);
			gameObject.renderer.material.color = new Color(2.3f, 2.3f, 2.3f, 1.0f);
			yield return new WaitForSeconds(.32f);
		}
	}





	void died(){
//		float temp = Time.time;
//		while (temp + 3.5f >= Time.time) {};
		print ("died");
		Application.LoadLevel (Application.loadedLevel);
	}



}

