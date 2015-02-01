using UnityEngine;
using System.Collections;

public class CustomWeapon : MonoBehaviour {
	private PE_Obj peo;
	public float speed;

	// Use this for initialization
	void Start () {
		peo = GetComponent<PE_Obj>();
		peo.grav = PE_GravType.none;
	}
	
	// Update is called once per frame
	void Update () {
		peo.vel.x = speed;

		if (transform.position.x < -7f) {
			PhysEngine.objs.Remove(this.GetComponent<PE_Obj>());
			Destroy (gameObject);
		}
		if (transform.position.x > 160f) {
			PhysEngine.objs.Remove(this.GetComponent<PE_Obj>());
			Destroy (gameObject);
		}
	}

	void OnTriggerStay(Collider other) { OnTriggerEnter(other);}
	
	void OnTriggerEnter(Collider other) {
		if (other.GetComponent<BreakableObject> () != null) {
			BreakableObject bo = other.GetComponent<BreakableObject>();
			bo.damage();
		} 
		PhysEngine.objs.Remove(this.GetComponent<PE_Obj>());
		Destroy(gameObject);
	}
}
