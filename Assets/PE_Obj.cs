using UnityEngine;
using System.Collections;



public class PE_Obj : MonoBehaviour {
	public bool			still = false;
	public PE_Collider	coll = PE_Collider.cube;
	public PE_GravType	grav = PE_GravType.constant;
	
	public Vector3		acc = Vector3.zero;
	
	public Vector3		vel = Vector3.zero;
	public Vector3		vel0 = Vector3.zero;
	
	public Vector3		pos0 = Vector3.zero;
	public Vector3		pos1 = Vector3.zero;
	
	public virtual void Start() {
		if (PhysEngine.objs.IndexOf(this) == -1) {
			PhysEngine.objs.Add(this);
		}
	}
	
	// Update is called once per frame
	void Update () {

	}
	
	void OnTriggerEnter(Collider other) {
		// Ignore collisions of still objects
		if (still) return;
		
		PE_Obj otherPEO = other.GetComponent<PE_Obj>();
		if (otherPEO == null) return;
		
		ResolveCollisionWith(otherPEO);
	}
	
	void OnTriggerStay(Collider other) {
		OnTriggerEnter(other);
	}

	void OnTriggerExit(Collider other) {
		acc.y = 0;
	}
	
	void ResolveCollisionWith(PE_Obj that) {
		// Assumes that "that" is still
		
		switch (this.coll) {
			case PE_Collider.megaman:
				switch (that.coll) {
					case PE_Collider.floor:
						if (vel.y < 0) {
							acc.y = -PhysEngine.gravity.y;
							vel.y = 0;
						}
						break;
					case PE_Collider.cube:
						print ("hit cube");
						vel.x = 0;
						break;
				}
				break;
			default:
				acc = Vector3.zero;
				break;
		}
	}
	
	
}
