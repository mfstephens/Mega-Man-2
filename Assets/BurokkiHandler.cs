using UnityEngine;
using System.Collections;

public class BurokkiHandler : MonoBehaviour {
	public float hp = 1;
	
	void FixedUpdate(){
		if (hp <= 0) {
			PhysEngine.objs.Remove(GetComponent<PE_Obj>());
			PhysEngine.objs.Remove(GetComponentInParent<PE_Obj>());
			Destroy(gameObject);
		}
		
	}
	
	public void DecrementHP(){
		hp--;
	}
}
