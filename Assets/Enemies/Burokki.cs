using UnityEngine;
using System.Collections;

public class Burokki : MonoBehaviour {
	public GameObject burokkiPrefab;
	GameObject mega_man, burokki1, burokki2, burokki3;
	public float velocity = .2f;
	public float spawn_view = 4f;
	Vector3 spawn1, spawn2, spawn3;
	// Use this for initialization
	void Start () {
		mega_man = GameObject.Find ("Mega Man");
		spawn1.Set (98.85f, -14.95f, -3f);
		spawn2.Set (104.63f, -13.49f, -3f);
		spawn3.Set (111.26f, -12.03f, -3f);
	}
	
	// Update is called once per frame
	void Update () {
		burokki_update (spawn1, burokki1);
		burokki_update (spawn2, burokki2);
		burokki_update (spawn3, burokki3);
	}

	void burokki_update(Vector3 spawn, GameObject burokki){
		Vector3 mm_pos = mega_man.transform.position;

		if ((mm_pos.x >= spawn.x - spawn_view) && (burokki == null) && mm_pos.x <= (spawn.x - spawn_view +.08f)) {
			burokki = Instantiate(burokkiPrefab) as GameObject;
			if(spawn == spawn1) burokki1 = burokki;
			if(spawn == spawn2) burokki2 = burokki;
			if(spawn == spawn3) burokki3 = burokki;
			burokki.transform.position = spawn;
			burokki.transform.GetComponent<PE_Obj>().vel.x = -velocity;
		}
		// destroy if out of view
		if (burokki != null && ((mm_pos.x < burokki.transform.position.x - spawn_view 
		                        || mm_pos.x > burokki.transform.position.x + spawn_view)
		                       || mm_pos.y > burokki.transform.position.y + 3f )){
			PhysEngine.objs.Remove(burokki.GetComponent<PE_Obj>());
			Destroy(burokki);
		}
	}
}
