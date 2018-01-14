using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CGrenadeThrower : MonoBehaviour {

	public Rigidbody GrenadePrefab;
	// how long it takes before grenade refreshes
	public float Cooldown;
	public float ThrowForce;
	public float VerticalAngle;

	private float cooldownTimer;
	public float CooldownTimer {
		get { return cooldownTimer; }
		private set { cooldownTimer = value; }
	}

	void FixedUpdate () {
		CooldownTimer -= Time.deltaTime;
	}

	public void Throw (Transform spawningTransform, Vector3 offset) {
		if (CooldownTimer <= 0) {
			CooldownTimer = Cooldown;
			Debug.Log("PUT GRENADE THROW ANIMATION HERE");
			var g = Instantiate(GrenadePrefab, spawningTransform.TransformPoint(offset), Quaternion.identity);
			Vector3 forceDir = (Quaternion.AngleAxis(VerticalAngle, spawningTransform.right) * spawningTransform.forward).normalized;
			g.AddForce(forceDir * ThrowForce, ForceMode.Impulse);
		}
	}

}
