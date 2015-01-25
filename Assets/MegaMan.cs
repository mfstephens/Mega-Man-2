using UnityEngine;
using System.Collections;

public class MegaMan : MonoBehaviour {
	private PE_Obj peo;
	
	public Vector3	vel;
	public bool		grounded = false;
	private bool facingRight, jumping;
	Animator anim;

	public GameObject blasterPrefab;
	public float shootingSpeed = 2.5f;
	public float	hSpeed = 1.5f;
	public float	acceleration = 5.4f;
	public float	jumpVel = 10f;
	public float	airSteeringAmt = 1f;
	float starttime;
	public float air_time = .3f;
	public float	airMomentumX = 1; // 0 for no momentum (i.e. 100% drag), 1 for total momentum
	public float	groundMomentumX = 0.1f;
	
	public Vector2	maxSpeed = new Vector2( 10, 15 ); // Different x & y to limit maximum falling velocity
	
	// Use this for initialization
	void Start () {
		peo = GetComponent<PE_Obj>();
		jumping = false;
		anim = GetComponent<Animator> ();
	}
	
	// Update is called once per frame
	// Note that we use Update for input but FixedUpdate for physics. This is because Unity input is handled based on Update
	void Update () {
		vel = peo.vel; // Pull velocity from the PE_Obj
		grounded = (peo.ground != null);
		
		// Horizontal movement
		float vX = Input.GetAxis("Horizontal"); // Returns a number [-1..1]
		vel.x = vX * hSpeed;

		anim.SetFloat ("speed", Mathf.Abs (vel.x));

		// set direction to face
		if (vel.x > 0)transform.eulerAngles = new Vector3 (0, 0, 0);
		if (vel.x < 0)transform.eulerAngles = new Vector3 (0, 180, 0);
	


		//jump
		if ((Input.GetKeyDown (KeyCode.X) || Input.GetKeyDown (KeyCode.Period)) && grounded){
			peo.ground = null; // Jumping will set ground = null
			jumping = true;
			starttime = Time.time;
		}
		if (jumping && ((starttime + air_time) < Time.time)) {
			jumping = false;
		}
		if (Input.GetKeyUp (KeyCode.X) || Input.GetKeyUp (KeyCode.Period)){
			jumping = false;
		}
		if (((Input.GetKey (KeyCode.X) || Input.GetKey (KeyCode.Period))) && jumping) {
			vel.y = jumpVel;
		}
		// jump 

		// shoot
		if (Input.GetKeyDown (",")) {
			GameObject bf = Instantiate(blasterPrefab) as GameObject;
			bf.transform.position = transform.position;
			Blaster blaster = bf.GetComponent<Blaster> ();
			
			// we are facing backwards
			if (transform.eulerAngles.y != 0) {
				blaster.speed = -shootingSpeed;
			} else {
				blaster.speed = shootingSpeed;
			}
		}

		anim.SetBool ("on_ground",grounded);
		peo.vel = vel;
		
	}
}

