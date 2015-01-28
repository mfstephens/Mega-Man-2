using UnityEngine;
using System.Collections;

public class MrBotHandler : MonoBehaviour {
	public float hp = 2;
	
	void FixedUpdate(){
		if (hp <= 0) {
			Destroy(gameObject);
		}
		
	}
	public void DecrementHP(){
		hp--;
	}
}
