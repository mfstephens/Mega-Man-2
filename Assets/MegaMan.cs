using UnityEngine;
using System.Collections;

public class MegaMan : PE_Obj {

	private bool facingRight, inAir;
	public float jumpHeight = 8f;
	public float runSpeed = 4f;
	Animator anim;
	Vector3 front, right_face, left_face;


	// Use this for initialization
	public override void Start () {
		base.Start ();
		inAir = false;
		facingRight = true;
		anim = GetComponent<Animator> ();
		
		// set initial animation parameters
		anim.SetBool("idling", true); 
		anim.SetBool("running", false);

		// set front to the very front edge of the level
		// will be compared against to dertmine Mega Man's direction
		front = GameObject.Find ("StartWall").transform.position;
	}

	// Update is called once per frame
	void FixedUpdate () {
		bool keyD = Input.GetKey ("d"), keyA = Input.GetKey ("a"), keyW = Input.GetKey("w");

		// used to determine the direction Mega Man is facing
		right_face = this.transform.FindChild("RightFace").position;
		left_face = this.transform.FindChild("LeftFace").position;
		if ((((front - right_face).magnitude) - ((front - left_face).magnitude)) <= 0) {
			facingRight = true; // negative magnitude means Mega Man is facing right
		} else { 
			facingRight = false; // positive magnitude means Mega Man is facing left
		}
		
		if (keyW) {
			if (vel.y == 0) {
				vel.y = jumpHeight;
			}
		} else {
//			acc.y = 0;
		}
		if ((!keyD && !keyA) || (keyD && keyA)) {
			vel.x = 0;
			if (!inAir) { // Mega Man is standing on ground 
				if(!anim.GetBool("idling")) set_idle_animation(); // set idle animation
			} else { // Mega Man is in air 
				// set airbone Mega Man sprite
			}
			return;
		}
		
		// rightward movement
		if (keyD) {
			if (keyA){ // keyD + keyA means no movement
				if (!anim.GetBool ("idling") && !inAir) set_idle_animation ();
				return;
			}
			if (!facingRight) {
				flip ();
				facingRight = true;
			}
			// set animation running right
			if(!anim.GetBool("running")) set_running_animation();
			
			// update Mega Man's position
			vel.x = runSpeed;
		}
		
		// leftward movement
		if (keyA) {
			if (keyD){ // keyA + keyD means no movement
				if (!anim.GetBool ("idling") && !inAir) set_idle_animation (); 
				return;
			}
			if (facingRight) {
				flip ();
				facingRight = false;
			}
			
			// set animation running left
			if(!anim.GetBool("running")) set_running_animation();
			
			// update Mega Man's position
			vel.x = -runSpeed;
		}

	}

	void flip () {
		Vector3 tempScale = this.transform.localScale;
		tempScale.x *= -1f;
		this.transform.localScale = tempScale;
	}

	void set_running_animation(){
		anim.SetBool("running", true);
		anim.SetBool ("idling", false);
	}
	void set_idle_animation(){
		flip ();
		anim.SetBool ("idling", true);
		anim.SetBool("running", false);
	}
}
