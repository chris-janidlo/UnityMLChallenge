using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class CBullet : MonoBehaviour {

	public float Speed;
	public float Damage;

	protected Rigidbody rb;

	// Use this for initialization
	public virtual void Initialize(Vector3 direction) {
		rb = GetComponent<Rigidbody>();
		rb.velocity = direction * Speed;
	}

	protected virtual void OnCollisionEnter (Collision collision) {
		CHealth.TryToDamage(collision.gameObject, Damage);
		Destroy(gameObject);
	}

}
