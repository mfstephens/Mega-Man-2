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
	}
}

