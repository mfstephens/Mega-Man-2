using UnityEngine;
using System.Collections;

public class MegaMan : MonoBehaviour {
	private PE_Obj peo;
	
	public Vector3	vel;
	public bool		grounded = false;
	private bool facingRight, inAir;
	Animator anim;
	Vector3 front, right_face, left_face;
	
	public float	hSpeed = 1.5f;
	public float	acceleration = 5.4f;
	public float	jumpVel = 100f;
	public float	airSteeringAmt = 1f;
	
	public float	airMomentumX = 1; // 0 for no momentum (i.e. 100% drag), 1 for total momentum
	public float	groundMomentumX = 0.1f;
	
	public Vector2	maxSpeed = new Vector2( 10, 15 ); // Different x & y to limit maximum falling velocity
	
	// Use this for initialization
	void Start () {
		peo = GetComponent<PE_Obj>();
		
		inAir = false;
		anim = GetComponent<Animator> ();
		
		// set front to the very front edge of the level
		// will be compared against to dertmine Mega Man's direction
		front = GameObject.Find ("StartWall").transform.position;
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

		if (vel.x > 0)transform.eulerAngles = new Vector3 (0, 0, 0);
		if (vel.x < 0)transform.eulerAngles = new Vector3 (0, 180, 0);
	

		// Jumping with A (which is x or .)
		float vY = (Input.GetAxis ("Vertical"));

		if (Input.GetKeyUp(KeyCode.X) || Input.GetKeyUp(KeyCode.Period)) {
			if (grounded) {
				vel.y = vY * jumpVel;
				peo.ground = null; // Jumping will set ground = null
			}		
		}
		peo.vel = vel;
		
	}
}

