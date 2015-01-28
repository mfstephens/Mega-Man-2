using UnityEngine;
using System.Collections;

public class WheelHandler : MonoBehaviour {
	public float hp = 2;
	
	void FixedUpdate(){
		if (hp <= 0) {
			PhysEngine.objs.Remove(GetComponent<PE_Obj>());
			Destroy(this);
		}
		
	}
	
	public void DecrementHP(){
		hp--;
	}
}
