using UnityEngine;
using System.Collections;

public class Pierobot : MonoBehaviour {
	public GameObject WheelPrefab, MrBotPrefab;

	GameObject mega_man, wheel1, wheel2, wheel3, wheel4, wheel5, mrbot1, mrbot2, mrbot3, mrbot4, mrbot5;
	public float velocity = .5f;
	public float spawn_view = 4f;
	Vector3 spawn1, spawn2, spawn3, spawn4, spawn5, wheel1_last_pos, wheel2_last_pos,  wheel3_last_pos,  wheel4_last_pos,  wheel5_last_pos;
	bool w1_forward, w2_forward, w3_forward, w4_forward, w5_forward;
	// Use this for initialization
	void Start () {
		mega_man = GameObject.Find ("Mega Man");
		spawn1.Set (75.85f, -13.1f, -3f);
		spawn2.Set (79.07f, -13.470f, -3f);
		spawn3.Set (87.11f, -13.86f, -3f);
		spawn4.Set (91.27f, -13.8f, -3f);
		spawn5.Set (94.94f, -13.83f, -3f);
		w1_forward = w2_forward = w3_forward = w4_forward = w5_forward = false;
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	void FixedUpdate(){
		wheel1_update ();
		wheel2_update ();
		wheel3_update ();
		wheel4_update ();
		wheel5_update ();

	}
	void wheel5_update(){
		Vector3 mm_pos = mega_man.transform.position;
		
		// wheel
		if ((mm_pos.x >= spawn5.x - spawn_view) && wheel5 == null && mm_pos.x <= (spawn5.x - spawn_view +.08f)) {
			wheel5 = Instantiate(WheelPrefab) as GameObject;
			wheel5.transform.position = spawn5;
			wheel5_last_pos = wheel5.transform.position;
			w5_forward = true;
			wheel5.transform.GetComponent<Spin>().is_left = true;
		}
		else if ((mm_pos.x <= spawn5.x + spawn_view) && wheel5 == null  && mm_pos.x >= (spawn5.x + spawn_view - .08f)) {
			wheel5 = Instantiate(WheelPrefab) as GameObject;
			wheel5.transform.position = spawn5;
			wheel5_last_pos = wheel5.transform.position;
			w5_forward = false;
			wheel5.transform.GetComponent<Spin>().is_left = false;
		}
		// destroy if out of view
		if (wheel5 != null && ((mm_pos.x < wheel5.transform.position.x - spawn_view 
		                        || mm_pos.x > wheel5.transform.position.x + spawn_view)
		                       || mm_pos.y > wheel5.transform.position.y + 3f )){
			PhysEngine.objs.Remove(wheel5.GetComponent<PE_Obj>());
			Destroy(wheel5);
			if(mrbot5 != null) Destroy (mrbot5);
		}
		
		// make mrbot aka pink guy
		if (wheel5 != null && mrbot5 == null && wheel5.transform.GetComponent<PE_Obj>().still == true
		    && ((mm_pos.x >= (spawn5.x - spawn_view + .25f) && w5_forward) 
		    || (mm_pos.x <= (spawn5.x + spawn_view - .25f) && !w5_forward))) {
			mrbot5 = Instantiate(MrBotPrefab) as GameObject;
			Vector3 temp = spawn5;
			temp.y += 3f;
			mrbot5.transform.position = temp;
		}
		// make mrbot fall until he hits wheel
		if (mrbot5 != null && wheel5 != null && mrbot5.transform.position.y > (spawn5.y + .95f)) {
			Vector3 temp = mrbot5.transform.position;
			temp.y -= 3.5f * Time.deltaTime;
			mrbot5.transform.position = temp;
		}
		
		// set in motion once mrbot falls ontop of wheel and megaman is close enough
		if( wheel5 != null && mrbot5 != null && mrbot5.transform.position.y  <= (spawn5.y + .95f) 
		   &&((mm_pos.x >= (spawn5.x - spawn_view + .6f) && w5_forward) 
		   || (mm_pos.x <= (spawn5.x + spawn_view - .6f) && !w5_forward))){
			wheel5.transform.GetComponent<PE_Obj>().still = false;
		}
		
		if (wheel5 != null && (wheel5.transform.GetComponent<PE_Obj>().ground == true)) {
			if(w5_forward) wheel5.transform.GetComponent<PE_Obj>().vel.x = -velocity;
			if(!w5_forward) wheel5.transform.GetComponent<PE_Obj>().vel.x = velocity;
		}
		if (wheel5 != null && mrbot5 != null) {
			Vector3 move = wheel5.transform.position - wheel5_last_pos;
			mrbot5.transform.position += move;
			wheel5_last_pos = wheel5.transform.position;
		}
		
	} 


	void wheel4_update(){
		Vector3 mm_pos = mega_man.transform.position;
		
		// wheel
		if ((mm_pos.x >= spawn4.x - spawn_view) && wheel4 == null && mm_pos.x <= (spawn4.x - spawn_view +.08f)) {
			wheel4 = Instantiate(WheelPrefab) as GameObject;
			wheel4.transform.position = spawn4;
			wheel4_last_pos = wheel4.transform.position;
			w4_forward = true;
			wheel4.transform.GetComponent<Spin>().is_left = true;
		}
		else if ((mm_pos.x <= spawn4.x + spawn_view) && wheel4 == null  && mm_pos.x >= (spawn4.x + spawn_view - .08f)) {
			wheel4 = Instantiate(WheelPrefab) as GameObject;
			wheel4.transform.position = spawn4;
			wheel4_last_pos = wheel4.transform.position;
			w4_forward = false;
			wheel4.transform.GetComponent<Spin>().is_left = false;
		}
		// destroy if out of view
		if (wheel4 != null && ((mm_pos.x < wheel4.transform.position.x - spawn_view 
		                        || mm_pos.x > wheel4.transform.position.x + spawn_view)
		                       || mm_pos.y > wheel4.transform.position.y + 3f )){
			PhysEngine.objs.Remove(wheel4.GetComponent<PE_Obj>());
			Destroy(wheel4);
			if(mrbot4 != null) Destroy (mrbot4);
		}
		
		// make mrbot aka pink guy
		if (wheel4 != null && mrbot4 == null && wheel4.transform.GetComponent<PE_Obj>().still == true
		    && ((mm_pos.x >= (spawn4.x - spawn_view + .25f) && w4_forward) 
		    || (mm_pos.x <= (spawn4.x + spawn_view - .25f) && !w4_forward))) {
			mrbot4 = Instantiate(MrBotPrefab) as GameObject;
			Vector3 temp = spawn4;
			temp.y += 3f;
			mrbot4.transform.position = temp;
		}
		// make mrbot fall until he hits wheel
		if (mrbot4 != null && wheel4 != null && mrbot4.transform.position.y > (spawn4.y + .95f)) {
			Vector3 temp = mrbot4.transform.position;
			temp.y -= 3.5f * Time.deltaTime;
			mrbot4.transform.position = temp;
		}
		
		// set in motion once mrbot falls ontop of wheel and megaman is close enough
		if( wheel4 != null && mrbot4 != null && mrbot4.transform.position.y  <= (spawn4.y + .95f) 
		   &&((mm_pos.x >= (spawn4.x - spawn_view + .6f) && w4_forward) 
		   || (mm_pos.x <= (spawn4.x + spawn_view - .6f) && !w4_forward))){
			wheel4.transform.GetComponent<PE_Obj>().still = false;
		}
		
		if (wheel4 != null && (wheel4.transform.GetComponent<PE_Obj>().ground == true)) {
			if(w4_forward) wheel4.transform.GetComponent<PE_Obj>().vel.x = -velocity;
			if(!w4_forward) wheel4.transform.GetComponent<PE_Obj>().vel.x = velocity;
		}
		if (wheel4 != null && mrbot4 != null) {
			Vector3 move = wheel4.transform.position - wheel4_last_pos;
			mrbot4.transform.position += move;
			wheel4_last_pos = wheel4.transform.position;
		}
		
	} 


	void wheel3_update(){
		Vector3 mm_pos = mega_man.transform.position;
		
		// wheel
		if ((mm_pos.x >= spawn3.x - spawn_view) && wheel3 == null && mm_pos.x <= (spawn3.x - spawn_view +.08f)) {
			wheel3 = Instantiate(WheelPrefab) as GameObject;
			wheel3.transform.position = spawn3;
			wheel3_last_pos = wheel3.transform.position;
			w3_forward = true;
			wheel3.transform.GetComponent<Spin>().is_left = true;
		}
		else if ((mm_pos.x <= spawn3.x + spawn_view) && wheel3 == null  && mm_pos.x >= (spawn3.x + spawn_view - .08f)) {
			wheel3 = Instantiate(WheelPrefab) as GameObject;
			wheel3.transform.position = spawn3;
			wheel3_last_pos = wheel3.transform.position;
			w3_forward = false;
			wheel3.transform.GetComponent<Spin>().is_left = false;
		}
		// destroy if out of view
		if (wheel3 != null && ((mm_pos.x < wheel3.transform.position.x - spawn_view 
		                        || mm_pos.x > wheel3.transform.position.x + spawn_view)
		                       || mm_pos.y > wheel3.transform.position.y + 3f )){
			PhysEngine.objs.Remove(wheel3.GetComponent<PE_Obj>());
			Destroy(wheel3);
			if(mrbot3 != null) Destroy (mrbot3);
		}
		
		// make mrbot aka pink guy
		if (wheel3 != null && mrbot3 == null && wheel3.transform.GetComponent<PE_Obj>().still == true
		    && ((mm_pos.x >= (spawn3.x - spawn_view + .25f) && w3_forward) 
		    || (mm_pos.x <= (spawn3.x + spawn_view - .25f) && !w3_forward))) {
			mrbot3 = Instantiate(MrBotPrefab) as GameObject;
			Vector3 temp = spawn3;
			temp.y += 3f;
			mrbot3.transform.position = temp;
		}
		// make mrbot fall until he hits wheel
		if (mrbot3 != null && wheel3 != null && mrbot3.transform.position.y > (spawn3.y + .95f)) {
			Vector3 temp = mrbot3.transform.position;
			temp.y -= 3.5f * Time.deltaTime;
			mrbot3.transform.position = temp;
		}
		
		// set in motion once mrbot falls ontop of wheel and megaman is close enough
		if( wheel3 != null && mrbot3 != null && mrbot3.transform.position.y  <= (spawn3.y + .95f) 
		   &&((mm_pos.x >= (spawn3.x - spawn_view + .6f) && w3_forward) 
		   || (mm_pos.x <= (spawn3.x + spawn_view - .6f) && !w3_forward))){
			wheel3.transform.GetComponent<PE_Obj>().still = false;
		}
		
		if (wheel3 != null && (wheel3.transform.GetComponent<PE_Obj>().ground == true)) {
			if(w3_forward) wheel3.transform.GetComponent<PE_Obj>().vel.x = -velocity;
			if(!w3_forward) wheel3.transform.GetComponent<PE_Obj>().vel.x = velocity;
		}
		if (wheel3 != null && mrbot3 != null) {
			Vector3 move = wheel3.transform.position - wheel3_last_pos;
			mrbot3.transform.position += move;
			wheel3_last_pos = wheel3.transform.position;
		}
		
	} 


	 void wheel2_update(){
		Vector3 mm_pos = mega_man.transform.position;
		
		// wheel
		if ((mm_pos.x >= spawn2.x - spawn_view) && wheel2 == null && mm_pos.x <= (spawn2.x - spawn_view +.08f)) {
			wheel2 = Instantiate(WheelPrefab) as GameObject;
			wheel2.transform.position = spawn2;
			wheel2_last_pos = wheel2.transform.position;
			w2_forward = true;
			wheel2.transform.GetComponent<Spin>().is_left = true;
		}
		else if ((mm_pos.x <= spawn2.x + spawn_view) && wheel2 == null  && mm_pos.x >= (spawn2.x + spawn_view - .08f)) {
			wheel2 = Instantiate(WheelPrefab) as GameObject;
			wheel2.transform.position = spawn2;
			wheel2_last_pos = wheel2.transform.position;
			w2_forward = false;
			wheel2.transform.GetComponent<Spin>().is_left = false;
		}
		// destroy if out of view
		if (wheel2 != null && ((mm_pos.x < wheel2.transform.position.x - spawn_view 
		                        || mm_pos.x > wheel2.transform.position.x + spawn_view)
		                       || mm_pos.y > wheel2.transform.position.y + 3f )){
			PhysEngine.objs.Remove(wheel2.GetComponent<PE_Obj>());
			Destroy(wheel2);
			if(mrbot2 != null) Destroy (mrbot2);
		}
		
		// make mrbot aka pink guy
		if (wheel2 != null && mrbot2 == null && wheel2.transform.GetComponent<PE_Obj>().still == true
		    && ((mm_pos.x >= (spawn2.x - spawn_view + .25f) && w2_forward) 
		    || (mm_pos.x <= (spawn2.x + spawn_view - .25f) && !w2_forward))) {
			mrbot2 = Instantiate(MrBotPrefab) as GameObject;
			Vector3 temp = spawn2;
			temp.y += 3f;
			mrbot2.transform.position = temp;
		}
		// make mrbot fall until he hits wheel
		if (mrbot2 != null && wheel2 != null && mrbot2.transform.position.y > (spawn2.y + .95f)) {
			Vector3 temp = mrbot2.transform.position;
			temp.y -= 3.5f * Time.deltaTime;
			mrbot2.transform.position = temp;
		}
		
		// set in motion once mrbot falls ontop of wheel and megaman is close enough
		if( wheel2 != null && mrbot2 != null && mrbot2.transform.position.y  <= (spawn2.y + .95f) 
		   &&((mm_pos.x >= (spawn2.x - spawn_view + .6f) && w2_forward) 
		   || (mm_pos.x <= (spawn2.x + spawn_view - .6f) && !w2_forward))){
			wheel2.transform.GetComponent<PE_Obj>().still = false;
		}
		
		if (wheel2 != null && (wheel2.transform.GetComponent<PE_Obj>().ground == true)) {
			if(w2_forward) wheel2.transform.GetComponent<PE_Obj>().vel.x = -velocity;
			if(!w2_forward) wheel2.transform.GetComponent<PE_Obj>().vel.x = velocity;
		}
		if (wheel2 != null && mrbot2 != null) {
			Vector3 move = wheel2.transform.position - wheel2_last_pos;
			mrbot2.transform.position += move;
			wheel2_last_pos = wheel2.transform.position;
		}

	} 

	void wheel1_update(){
		Vector3 mm_pos = mega_man.transform.position;
		
		// wheel
		if ((mm_pos.x >= spawn1.x - spawn_view) && wheel1 == null && mm_pos.x <= (spawn1.x - spawn_view +.08f)) {
			wheel1 = Instantiate(WheelPrefab) as GameObject;
			wheel1.transform.position = spawn1;
			wheel1_last_pos = wheel1.transform.position;
			w1_forward = true;
			wheel1.transform.GetComponent<Spin>().is_left = true;
		}
		else if ((mm_pos.x <= spawn1.x + spawn_view) && wheel1 == null  && mm_pos.x >= (spawn1.x + spawn_view - .08f)) {
			wheel1 = Instantiate(WheelPrefab) as GameObject;
			wheel1.transform.position = spawn1;
			wheel1_last_pos = wheel1.transform.position;
			w1_forward = false;
			wheel1.transform.GetComponent<Spin>().is_left = false;
		}
		// destroy if out of view
		if (wheel1 != null && ((mm_pos.x < wheel1.transform.position.x - spawn_view 
		                        || mm_pos.x > wheel1.transform.position.x + spawn_view)
		                       || mm_pos.y > wheel1.transform.position.y + 3f )){
			PhysEngine.objs.Remove(wheel1.GetComponent<PE_Obj>());
			Destroy(wheel1);
			if(mrbot1 != null) Destroy (mrbot1);
		}
		
		// make mrbot aka pink guy
		if (wheel1 != null && mrbot1 == null && wheel1.transform.GetComponent<PE_Obj>().still == true
		    && ((mm_pos.x >= (spawn1.x - spawn_view + .25f) && w1_forward) 
		    || (mm_pos.x <= (spawn1.x + spawn_view - .25f) && !w1_forward))) {
			mrbot1 = Instantiate(MrBotPrefab) as GameObject;
			Vector3 temp = spawn1;
			temp.y += 3f;
			mrbot1.transform.position = temp;
		}
		// make mrbot fall until he hits wheel
		if (mrbot1 != null && wheel1 != null && mrbot1.transform.position.y > (spawn1.y + .95f)) {
			Vector3 temp = mrbot1.transform.position;
			temp.y -= 3.5f * Time.deltaTime;
			mrbot1.transform.position = temp;
		}
		
		// set in motion once mrbot falls ontop of wheel and megaman is close enough
		if( wheel1 != null && mrbot1 != null && mrbot1.transform.position.y <= (spawn1.y + .95f) 
		   &&((mm_pos.x >= (spawn1.x - spawn_view + .6f) && w1_forward) 
		   || (mm_pos.x <= (spawn1.x + spawn_view - .6f) && !w1_forward))){
			wheel1.transform.GetComponent<PE_Obj>().still = false;
		}
		
		if (wheel1 != null && (wheel1.transform.GetComponent<PE_Obj>().ground == true)) {
			if(w1_forward) wheel1.transform.GetComponent<PE_Obj>().vel.x = -velocity;
			if(!w1_forward) wheel1.transform.GetComponent<PE_Obj>().vel.x = velocity;
		}
		if (wheel1 != null && mrbot1 != null) {
			Vector3 move = wheel1.transform.position - wheel1_last_pos;
			mrbot1.transform.position += move;
			wheel1_last_pos = wheel1.transform.position;
		}


	}


}
