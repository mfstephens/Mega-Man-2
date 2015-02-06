using UnityEngine;
using System.Collections;
using System.Collections.Generic;


public class MegaMan_Custom : MonoBehaviour {
	PE_Obj peo;
	bool facingRight, jumping, enemy_collision, immune, coroutine, respawning;
	float jump_start, shoot_start, immunity_start, flash_start, start_wait, collision_anim, respawn_wait_time;
	float frozen_start, frozen_duration;
	Vector3 spawn1, spawn2;
	Animator anim;
	public GameObject health;


	public bool jeremyMode = false;

	private Color originalColor;
	public GameObject blasterPrefab, customWeaponPrefab;
	public float num_energy_tanks = 0f;
	public float num_lives = 100f;
	public float wait_time_left;
	static public List<GameObject> blasters;
	public WeaponType currentWeapon;
	public AudioSource[] sounds;
	public bool hit_by_ice;

	
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
		hit_by_ice = false;
		peo = GetComponent<PE_Obj>();
		anim = GetComponent<Animator> ();
		sounds = GetComponents<AudioSource>();
		jumping = false;
		enemy_collision = immune = false;
		health = GameObject.Find ("Health Bar");
		immunity_start = -4f;
		blasters = new List<GameObject>();
		no_movement = respawning = false;
		start_wait = 0f;
		frozen_start = 0f;
		frozen_duration = 1.5f;
		respawn_wait_time = 4.3f;
		collision_anim = .4f;
		coroutine = false;
		spawn1.Set (.05f, -1.32f, -4f);
		spawn2.Set (111.27f, -.7f, -4f);
		originalColor = gameObject.renderer.material.color;
	}
	
	// Update is called once per frame
	// Note that we use Update for input but FixedUpdate for physics. This is because Unity input is handled based on Update
	void Update () {
		set_immunity ();
		if (immunity_start <= Time.time) enemy_collision = false;
		check_health_lives_and_respawn_or_die ();
		
		if(no_movement || respawning ) return;
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

			// jeremy mode
			if (Input.GetKeyDown(KeyCode.J)) {
				jeremyMode = !jeremyMode;
				gameObject.renderer.material.color = new Color(0f, 1.0f, .2f, 1.0f);
				if(!jeremyMode) gameObject.renderer.material.color = originalColor;
			}
			
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
			if(jeremyMode) return;
			if(other.GetComponent<IceBall>() != null){
				if(!immune && immunity_start < Time.time){
					if(other.GetComponent<IceBall>().stomp) {
						enemy_collision = true;
						// 5 damage
						for(int i = 0; i < 5; i++) health.GetComponent<HealthBar>().decreaseByOne();
						immunity_start = Time.time + collision_duration;
						sounds[1].Play ();
						return;
					} else{
						for(int i = 0; i < 4; i++) health.GetComponent<HealthBar>().decreaseByOne();
						immunity_start = Time.time + collision_duration;
						sounds[1].Play ();
						no_movement = true;
						peo.still = true;
						frozen_start = Time.time;
						hit_by_ice = true;
						gameObject.renderer.material.color = new Color(1f, 2.2f, 1f, 1.0f);
						return;
					}
				}
			}
			return;
		}
		if(jeremyMode) return;
		if (GetComponent<PE_Obj>().still) return; //still is set when picking up health pellets, and no damage should be taken while frozen
		else {
			if(jeremyMode) return;
			if (otherPEO.coll == PE_Collider.press) {
				if(!immune && immunity_start < Time.time){
					enemy_collision = true;
					// 8 damage
					for(int i = 0; i < 8; i++) health.GetComponent<HealthBar>().decreaseByOne();
					immunity_start = Time.time + collision_duration;
					sounds[1].Play ();
				}
			}
			if (otherPEO.coll == PE_Collider.spikewall) {
				if (!immune) {
					health.GetComponent<HealthBar>().empty();
				}
			}
			if (otherPEO.coll == PE_Collider.spike) {
				if (!immune) {
					health.GetComponent<HealthBar>().empty ();
				}

			}
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
		if (jeremyMode) {
			immune = true;
			enemy_collision = false;
		}
		if (hit_by_ice && (frozen_start + frozen_duration < Time.time)) {
			peo.still = false;
			no_movement = false;
			gameObject.renderer.material.color = new Color(1.0f, 1.0f, 1.0f, 1.0f);
			}
		if (immunity_start <= Time.time &&( immunity_start + immunity_duration) > Time.time){
			immune = true;
			no_movement = false;
		}
		if(immunity_start + collision_anim <= Time.time) anim.SetBool ("enemy_collision", false);
		if(immunity_duration + immunity_start < Time.time) immune = false;
	}
	

	void check_health_lives_and_respawn_or_die(){
		if(transform.position.y < -4) goto start_resp;
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
			if(transform.position.x < 98){
				died ();	
			}
			else{
				transform.position = spawn2;
				peo._pos0 = spawn2;
				peo._pos1 = spawn2;
			}
			sounds[0].Play ();
			health.GetComponent<HealthBar>().increaseByAmount(28);
			GetComponent<PE_Obj>().still = false;
			no_movement = false;
			respawning = false;
			peo.coll = PE_Collider.megaman;
			gameObject.renderer.material.color = new Color(1f, 1f, 1f, 1.0f);
			died ();
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
		Application.LoadLevel (Application.loadedLevel);
	}
	
	
	
}

