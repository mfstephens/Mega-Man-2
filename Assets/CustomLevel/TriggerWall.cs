using UnityEngine;
using System.Collections;

public class TriggerWall : MonoBehaviour {

	private MegaMan mega_man;
	public float triggerDistance = 1.0f;

	// Use this for initialization
	void Start () {
		GameObject go = GameObject.Find ("Mega Man");
		mega_man = go.GetComponent<MegaMan> ();
	}
	
	// Update is called once per frame
	void Update () {
		print ("dist: " + (transform.position.x + triggerDistance));
		print ("megaman: " + mega_man.transform.position.x);
		if (mega_man.transform.position.x >= (transform.position.x + triggerDistance)) {
			gameObject.GetComponent<PE_Obj>().still = false;
		}
	}
}
