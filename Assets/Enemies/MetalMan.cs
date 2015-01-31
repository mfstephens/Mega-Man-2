using UnityEngine;
using System.Collections;


public class MetalMan : MonoBehaviour {
	public float hp = 28;
	
	void Awake(){
		hp = 28;
		
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
