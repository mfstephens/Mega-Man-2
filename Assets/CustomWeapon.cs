using UnityEngine;
using System.Collections;

public class CustomWeapon : Blaster {

	// Use this for initialization
	public override void Awake() {
		base.Awake ();
	}
	
	// Update is called once per frame
	public override void FixedUpdate () {
		base.FixedUpdate ();
	}
	

	public override void OnTriggerEnter(Collider other) {
		if (other.GetComponent<BreakableObject> () != null) {
			BreakableObject bo = other.GetComponent<BreakableObject>();
			bo.damage();
		} 
		PhysEngine.objs.Remove(this.GetComponent<PE_Obj>());
		MegaMan.blasters.Remove (gameObject);
		Destroy(gameObject);

		base.OnTriggerEnter (other);
	}
}
