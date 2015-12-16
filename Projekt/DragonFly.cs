using UnityEngine;
using System.Collections;

public class DragonFly : MonoBehaviour {

	float startTime;
	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void FixedUpdate () {

		transform.position += transform.forward * 10.0f*Time.deltaTime;
		transform.Rotate(-Input.GetAxis("Vertical")+(Input.GetAxis("Horizontal") ),0.0f,-Input.GetAxis("Horizontal"));
		if (Input.GetAxis ("Horizontal") == 0) {
			 startTime = Time.time;
		}
		float t = Time.time - startTime;
		transform.rotation = Quaternion.Slerp (transform.rotation, Quaternion.Euler(transform.rotation.x, transform.rotation.y,0), t);

	}
}
