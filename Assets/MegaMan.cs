using UnityEngine;
using System.Collections;

public class MegaMan : MonoBehaviour {
	private PE_Obj peo;
	bool facingRight, jumping;
	float 	jump_start, shoot_start;
	Animator anim;
	Camera main_cam;
	public Vector3 cam_pos_last_frame;
	public Vector3	vel;
	public bool		grounded = false;
	public GameObject blasterPrefab;
	public float 	health = 28;
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
	}
	
	// Update is called once per frame
	// Note that we use Update for input but FixedUpdate for physics. This is because Unity input is handled based on Update
	void Update () {
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

	
		peo.vel = vel;
	} // end Update()

	




	/* void OnTriggerEnter(Collider other) {
		// Ignore collisions of still objects
		PE_Obj otherPEO = other.GetComponent<PE_Obj>();
		if (otherPEO == null) return;
		if (otherPEO.coll != PE_Collider.press)return;
		health -= 8f;
		if (health <= 0) died ();
		Vector3 temp = this.transform.position;
		temp.x -= 1f;
		transform.position = temp;
	}


	void FixedUpdate(){



	}


	void died(){
		float temp = Time.time;	
		while (temp + 3.5f >= Time.time) {};
		Application.LoadLevel (Application.loadedLevel);
		}

*/

}

