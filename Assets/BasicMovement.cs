using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BasicMovement : MonoBehaviour {

	public float speed = 10.0f;
	public float jump = 50.0f;
	public float highJump = 70.0f;
	public float sprint = 2.0f;
	public float wallCheckDist = 0.75f;
	public float groundCheckDist = 1.0f;

	public float maxFallVelocity = -50.0f;
	public float maxGlideVelocity = -20.0f;

	public float maxVertVelocity;

	public float longPressTime = 0.2f;
	public float lastJumpPress;
	bool canHighJump = true;

	float direction = 1.0f;

	public Rigidbody2D body;

	public SpriteRenderer srenderer;

	enum State
	{
		OnGround,
		Jumping,
		HighJumping,
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

		maxVertVelocity = maxFallVelocity;
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	void FixedUpdate() {
		float lr = Input.GetAxis ("Horizontal") * speed;
		bool sprintInput = Input.GetKeyDown (KeyCode.LeftShift);
		bool jumpInput = Input.GetKeyDown (KeyCode.W);
		bool glideInput = Input.GetKey (KeyCode.W);

		if (sprintInput) {
			lr *= sprint;
		}

		float vertInput = body.velocity.y;


		if (lr > 0 && direction < 0 || lr < 0 && direction > 0) {
			direction *= -1;
		}

		if (OnGround ()) {
			if (jumpInput) {
				vertInput += jump;
				lastJumpPress = Time.fixedTime;
				canHighJump = true;
			}

			SetState (State.OnGround);
		} else if (HittingWall ()) {
			if (vertInput > 0) {
				vertInput = 0;
			}
			SetState (State.Sliding);
		} else if (vertInput > 0) {
			if (glideInput && Time.fixedTime > lastJumpPress + longPressTime && canHighJump) {
				vertInput += highJump;
				canHighJump = false;
				SetState (State.HighJumping);
			} else {
				if (!glideInput) {
					canHighJump = false;
				}
				SetState (State.Jumping);
			}
		} else if (vertInput < 0) {
			if (glideInput) {
				maxVertVelocity = maxGlideVelocity;
				SetState (State.Gliding);
			} else {
				maxVertVelocity = maxFallVelocity;
				SetState (State.Falling);
			}
			if (vertInput < maxVertVelocity) {
				vertInput = maxVertVelocity;
			}
		} else {
			SetState (State.Default);
		}

		body.velocity = new Vector2 (lr, vertInput);
	}

	bool HittingWall() {
		RaycastHit2D cast = Physics2D.Raycast (transform.position, Vector2.right * direction, wallCheckDist);
		Debug.DrawRay (transform.position, Vector2.right * direction * wallCheckDist, Color.green);
		if (cast.collider != null) {
//			Debug.Log ("Wall raycast hitting: " + cast.collider.name);
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
//			Debug.Log ("Ground raycast hitting: " + cast.collider.name);
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
		case State.Jumping:
			srenderer.color = Color.yellow;
			break;
		case State.HighJumping:
			srenderer.color = Color.white;
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
