using UnityEngine;
using System.Collections;

public class Spring : MonoBehaviour {
	public GameObject SpringPrefab;
	GameObject mega_man, spring1, spring2;
	public float spawn_view = 4.2f;
	Vector3 spawn1, spawn2;
	// Use this for initialization
	void Start () {
		mega_man = GameObject.Find ("Mega Man");
		spawn1.Set (117.3f, -16.9f, -3f);
		spawn2.Set (122.95f, -15.93f, -3f);
	}
	
	// Update is called once per frame
	void FixedUpdate () {
		spawn_update (spawn1, spring1);
		spawn_update (spawn2, spring2);
	}
	
	void spawn_update(Vector3 spawn, GameObject spring){
		Vector3 mm_pos = mega_man.transform.position;
		
		if ((mm_pos.x >= spawn.x - spawn_view) && (spring == null) && mm_pos.x <= (spawn.x - spawn_view + 1f)) {
			spring = Instantiate(SpringPrefab) as GameObject;
			spring.transform.position = spawn;
			if(spawn == spawn1) spring1 = spring;
			if(spawn == spawn2) spring2 = spring;
		}
		// destroy if out of view
		if (spring != null && ((mm_pos.x + 2.4f < spring.transform.position.x - spawn_view 
		                         || mm_pos.x > spring.transform.position.x + spawn_view + .8f)
		                        || mm_pos.y > spring.transform.position.y + 3f )){
			Destroy(spring);
		}
	}
}
