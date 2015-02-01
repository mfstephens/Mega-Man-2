using UnityEngine;
using System.Collections;

public class BurokkiHandler : MonoBehaviour {
	public float hp = 1;
	
	void FixedUpdate(){
		if (hp <= 0) {
			PhysEngine.objs.Remove(gameObject.GetComponent<PE_Obj>());
			PhysEngine.objs.Remove(transform.parent.gameObject.GetComponent<PE_Obj>());
			Destroy(transform.parent.gameObject);
		}
		
	}
	
	public void DecrementHP(){
		hp--;
	}
}
