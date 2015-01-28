using UnityEngine;
using System.Collections;

public class Blaster : MonoBehaviour {
	private PE_Obj peo;
	public float	speed;
	// Use this for initialization

	void Start () {
		peo = GetComponent<PE_Obj>();
		peo.grav = PE_GravType.none;
	}
	
	// Update is called once per frame
	// Note that we use Update for input but FixedUpdate for physics. This is because Unity input is handled based on Update
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


	void OnTriggerEnter(Collider other) {
		PE_Obj otherPEO = other.GetComponent<PE_Obj>();
		if (otherPEO.coll == PE_Collider.mole) {
			other.GetComponent<MoleHandler>().DecrementHP();
			PhysEngine.objs.Remove(this.GetComponent<PE_Obj>());
			Destroy (gameObject);
		}
		else if (otherPEO.coll == PE_Collider.pierobot) {
			other.GetComponent<WheelHandler>().DecrementHP();
			PhysEngine.objs.Remove(this.GetComponent<PE_Obj>());
			Destroy (gameObject);
			}

		else if (otherPEO.coll == PE_Collider.burokki)Destroy (this);
		else if (otherPEO.coll == PE_Collider.burokkiface) {
			other.GetComponent<BurokkiHandler>().DecrementHP();
			PhysEngine.objs.Remove(this.GetComponent<PE_Obj>());
			Destroy (gameObject);
		}
		else if (otherPEO.coll == PE_Collider.boss) {

			PhysEngine.objs.Remove(this.GetComponent<PE_Obj>());
			Destroy (gameObject);
		}
	}



}

