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
	public Vector2 LookSensitivity;
	public Vector2 LookSmoothing;
	// set this to restrict the range of motion
	public Vector2 LookClamp;

	// direction in world space that eyes are currently looking
	public Vector3 LookDirection { get { return Head.forward; } }

	private Rigidbody rb;
	private Vector3 movement;
	private Vector2 look;
	private Vector2 smoothLook;
	private Quaternion targetCharacterDirection;
	private Quaternion targetHeadDirection;

	void Awake () {
		if (Head.parent != transform)
			throw new Exception("Head must be a child of this.transform");
		rb = GetComponent<Rigidbody>();
		targetCharacterDirection = transform.localRotation;
		targetHeadDirection = Head.localRotation;
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

		rb.velocity += transform.TransformDirection(movement) * Time.deltaTime;
		float max = IsGrounded() ? MaxGroundSpeed : MaxAirSpeed;
		if (rb.velocity.magnitude > max)
			rb.velocity = rb.velocity.normalized * max;
	}

	// delta.x is horizontal look, delta.y is vertical look
	// delta will be clamped to -1, 1
	public void MoveEyes (Vector2 delta) {
		Vector2 clamped = clampDeltaVector(delta);
		clamped.Scale(Vector2.Scale(LookSensitivity, LookSmoothing));
		smoothLook.x = Mathf.Lerp(smoothLook.x, clamped.x, 1f / LookSmoothing.x);
		smoothLook.y = Mathf.Lerp(smoothLook.y, clamped.y, 1f / LookSmoothing.y);

		look += smoothLook;

		// clamp look in degrees
		if (LookClamp.x < 360)
			look.x = Mathf.Clamp(look.x, -LookClamp.x * 0.5f, LookClamp.x * 0.5f);
		if (LookClamp.y < 360)
			look.y = Mathf.Clamp(look.y, -LookClamp.y * 0.5f, LookClamp.y * 0.5f);

		// apply look
		Head.transform.localRotation = Quaternion.AngleAxis(-look.y, targetHeadDirection * Vector3.right) * targetHeadDirection;
		var yRotation = Quaternion.AngleAxis(look.x, Vector3.up);
		transform.localRotation = yRotation * targetCharacterDirection;
	}

	private Vector2 clampDeltaVector (Vector2 deltaVector) {
		Func<float, float> clamp = x => Mathf.Clamp(x, -1, 1);
		return new Vector2(clamp(deltaVector.x), clamp(deltaVector.y));
	}

}
