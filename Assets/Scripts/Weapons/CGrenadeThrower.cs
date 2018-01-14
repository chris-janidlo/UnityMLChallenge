using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CGrenadeThrower : MonoBehaviour {

	public Rigidbody GrenadePrefab;
	// how long it takes before grenade refreshes
	public float Cooldown;
	public float ThrowForce;
	public Vector3 ThrowOffsetAngle;

	private float cooldownTimer;
	public float CooldownTimer {
		get { return cooldownTimer; }
		private set { cooldownTimer = value; }
	}

	void FixedUpdate () {
		CooldownTimer -= Time.deltaTime;
	}

	public void Throw (Vector3 position, Vector3 direction) {
		if (CooldownTimer <= 0) {
			CooldownTimer = Cooldown;
			Debug.Log("PUT GRENADE THROW ANIMATION HERE");
			var g = Instantiate(GrenadePrefab, position, Quaternion.identity);
			Vector3 forceDir = (Quaternion.Euler(ThrowOffsetAngle) * direction).normalized;
			g.AddForce(forceDir * ThrowForce, ForceMode.Impulse);
		}
	}

}
