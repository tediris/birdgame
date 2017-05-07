using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BasicMovement : MonoBehaviour {

	public float speed = 10.0f;
	public float jump = 50.0f;
	public float sprint = 2.0f;

	public Rigidbody2D body;

	// Use this for initialization
	void Start () {
		if (body == null) {
			body = GetComponent<Rigidbody2D> ();
		}
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	void FixedUpdate() {
		float lr = Input.GetAxis ("Horizontal") * speed;
		bool sprintInput = Input.GetKeyDown (KeyCode.LeftShift);
		bool jumpInput = Input.GetKeyDown (KeyCode.W);

		if (sprintInput) {
			lr *= sprint;
		}

		float vertInput = body.velocity.y;

		if (jumpInput) {
			vertInput += jump;
		}

		body.velocity = new Vector2 (lr, vertInput);


	}
}
