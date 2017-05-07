using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BasicMovement : MonoBehaviour {

	public float speed = 10.0f;
	public float jump = 50.0f;
	public float sprint = 2.0f;
	public float wallCheckDist = 0.75f;
	public float groundCheckDist = 1.0f;

	public float direction = 1.0f;

	public Rigidbody2D body;

	public SpriteRenderer srenderer;

	enum State
	{
		OnGround,
		Falling,
		Gliding,
		Sliding,
		Default
	};

	// Use this for initialization
	void Start () {
		if (body == null) {
			body = GetComponent<Rigidbody2D> ();
		}

		if (srenderer == null) {
			srenderer = GetComponent<SpriteRenderer> ();
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

		if (lr > 0 && direction < 0 || lr < 0 && direction > 0) {
			direction *= -1;
		}

		if (OnGround ()) {
			SetState (State.OnGround);
		} else if (HittingWall ()) {
			SetState (State.Sliding);
		} else {
			SetState (State.Default);
		}
	}

	bool HittingWall() {
		RaycastHit2D cast = Physics2D.Raycast (transform.position, Vector2.right * direction, wallCheckDist);
		Debug.DrawRay (transform.position, Vector2.right * direction * wallCheckDist, Color.green);
		if (cast.collider != null) {
			Debug.Log ("Wall raycast hitting: " + cast.collider.name);
			if (cast.collider.tag == "Ground") {
				return true;
			}
		}
		return false;
	}

	bool OnGround() {
		RaycastHit2D cast = Physics2D.Raycast (transform.position, -Vector2.up, groundCheckDist);
		Debug.DrawRay (transform.position, -Vector2.up * groundCheckDist, Color.green);
		if (cast.collider != null) {
			Debug.Log ("Ground raycast hitting: " + cast.collider.name);
			if (cast.collider.tag == "Ground") {
				return true;
			}
		}
		return false;
	}

	void SetState(State state) {
		switch (state) {
		case State.OnGround:
			srenderer.color = Color.red;
			break;
		case State.Falling:
			srenderer.color = Color.blue;
			break;
		case State.Gliding:
			srenderer.color = Color.gray;
			break;
		case State.Sliding:
			srenderer.color = Color.green;
			break;
		default:
			srenderer.color = Color.black;
			break;
		};
	}
}
