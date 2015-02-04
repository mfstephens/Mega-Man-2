using UnityEngine;
using System.Collections;

public class BreakableObject : MonoBehaviour {
	public int totalHitPoints = 5;
	public int hitPoints = 5;

	// Use this for initialization
	void Start () {
	}
	
	// Update is called once per frame
	void Update () {
		if (hitPoints == 0) {
			PhysEngine.objs.Remove(this.GetComponent<PE_Obj>());
			Destroy(gameObject);
		}
	}

	public void damage() {
		if (hitPoints > 0) {
			hitPoints -= 1;
			Color textureColor = gameObject.renderer.material.color;
			textureColor.a -= 0.20f;
			gameObject.renderer.material.color = textureColor;
		}
	}
}
