using UnityEngine;
using System.Collections;

public class MoleHandler : MonoBehaviour {
	public float hp = 3;

	void Awake(){
		hp = 3;

	}
	void FixedUpdate(){
		if (hp <= 0) {
			PhysEngine.objs.Remove(GetComponent<PE_Obj>());
			Destroy(gameObject);
		}
		
	}
	
	public void DecrementHP(){
		hp--;
	}
}
