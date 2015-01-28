using UnityEngine;
using System.Collections;

public class MegaMan : MonoBehaviour {
	private PE_Obj peo;
	bool facingRight, jumping, enemy_collision, immune;
	float 	jump_start, shoot_start, immunity_start, flash_start;
	Vector3 spawn;
	Animator anim;
	Camera main_cam;
	GameObject health;
	public Vector3 cam_pos_last_frame;
	public Vector3	vel;
	public bool		grounded = false;
	public GameObject blasterPrefab;
	public float flash_duration =.5f;
	public float immunity_duration = 4f;
	public float collision_duration = .5f;
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
	}
	
	// Update is called once per frame
	// Note that we use Update for input but FixedUpdate for physics. This is because Unity input is handled based on Update
	void Update () {
		set_immunity ();
		if (immunity_start <= Time.time) enemy_collision = false;
		if (enemy_collision && !immune) {
			peo.vel.x = -20f;
			bump_back_and_flash();
		}

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
			if (Input.GetKeyDown (",") || Input.GetKeyDown (KeyCode.Z)) {
				anim.SetBool ("shooting", true);
				shoot_start = Time.time;
				GameObject bf = Instantiate(blasterPrefab) as GameObject;
				Vector3 end_of_gun = transform.position;
				Blaster blaster = bf.GetComponent<Blaster> ();
			
				// we are facing backwards
				if (transform.eulerAngles.y != 0) {
					end_of_gun.x += -.5f;
					blaster.speed = -shootingSpeed;
				} else {
					end_of_gun.x += .5f;
					blaster.speed = shootingSpeed;
				}
				bf.transform.position = end_of_gun;
			}
				if ((shoot_start + .25f) <= Time.time)
					anim.SetBool ("shooting", false);
				// end shoot

			anim.SetBool ("on_ground", grounded);

	
		}
		peo.vel = vel;
	} // end Update()

	
	void FixedUpdate(){
	}



	 void OnTriggerEnter(Collider other) {
		PE_Obj otherPEO = other.GetComponent<PE_Obj>();
		if (otherPEO == null) return;
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

	}
	
	void bump_back_and_flash(){
		//new code here
	/*	// add flash and anim
		flash_start = Time.time;
		renderer.material.color
		if(flash_start + flash_duration < Time.time){ 
		}
		else 	renderer.material.color = colors[0];
		Vector3 temp = transform.position;
		temp.x += (15f * Time.deltaTime);
		transform.position = temp;
		*/
	}

	void set_immunity(){
		if (immunity_duration + immunity_start >= Time.time)immune = true;
		if(immunity_duration + immunity_start < Time.time) immune = false;
		if(enemy_collision && !immune) immunity_start = Time.time + collision_duration;
	}

	void died(){
		float temp = Time.time;	
		while (temp + 3.5f >= Time.time) {};
		Application.LoadLevel (Application.loadedLevel);
		}



}

