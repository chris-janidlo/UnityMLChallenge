using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
[RequireComponent(typeof(Collider))]
public class CMovement : MonoBehaviour {

	public Transform Head;
	public float JumpForce;
	// distance from middle of this transform to ground
	public float GroundDistance;
	// how much distance maximum can be moved per second while grounded
	public float MaxGroundSpeed;
	// same but when not grounded
	public float MaxAirSpeed;
	public float GroundAcceleration;
	public float AirAcceleration;
	// how many degrees maximum can be looked per frame
	public Vector2 MaxLookDelta;

	private Rigidbody rb;
	private Vector3 movement;
	private Vector3 look;

	void Awake () {
		if (Head.parent != transform)
			throw new Exception("Head must be a child of this.transform");
		rb = GetComponent<Rigidbody>();
	}

	void FixedUpdate () {
		rb.velocity += transform.TransformDirection(movement) * Time.deltaTime;
		float max = IsGrounded() ? MaxGroundSpeed : MaxAirSpeed;
		if (rb.velocity.magnitude > max)
			rb.velocity = rb.velocity.normalized * max;

	}

	public bool IsGrounded() {
		return Physics.Raycast(transform.position, Vector3.down, GroundDistance + 0.01f);
	}

	public void Jump () {
		if (IsGrounded())
			rb.AddForce(Vector3.up * JumpForce, ForceMode.Impulse);
	}

	// delta.x is forward move, delta.y is strafing
	// delta will be clamped to -1, 1
	public void Move (Vector2 delta) {
		Vector2 clamped = clampDeltaVector(delta);
		float acc = IsGrounded() ? GroundAcceleration : AirAcceleration;
		movement = new Vector3(clamped.y, 0, clamped.x) * acc;
	}

	// delta.x is horizontal look, delta.y is vertical look
	// delta will be clamped to -1, 1
	public void MoveEyes (Vector2 delta) {

		throw new NotImplementedException();
	}

	private Vector2 clampDeltaVector (Vector2 deltaVector) {
		Func<float, float> clamp = x => Mathf.Clamp(x, -1, 1);
		return new Vector2(clamp(deltaVector.x), clamp(deltaVector.y));
	}

}
