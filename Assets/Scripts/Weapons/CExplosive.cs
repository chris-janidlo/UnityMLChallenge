using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class CExplosive : MonoBehaviour {

	// if true, explosion is triggered by impact. if false, it's triggered by a timer
	public bool Impact;
	// if impact, how long after impact to explode. if timer, how long after spawning to explode.
	public float Timer;
	// amount of damage dealt in the direct explosion zone
	public float SplashDamage;
	// radius of direct explosion zone sphere
	public float SplashRadius;
	// for every Unity unit outside of the direct radius, subtract Fallout from the damage done to any damageable
	public float Falloff;

	// maximum possible distance for any damage to occur
	public float MaxRadius { get { return SplashDamage / Falloff + SplashRadius; } }

	protected bool ticking = false;

	void Start () {
		if (!Impact)
			StartTimer();
	}

	void OnCollisionEnter () {
		if (Impact && !ticking)
			StartTimer();
	}

	public virtual void Explode () {
		Collider[] hits = Physics.OverlapSphere(transform.position, MaxRadius);
		for (int i = 0; i < hits.Length; i++) {
			var hit = hits[i];
			float distance = Vector3.Distance(hit.transform.position, transform.position);
			float damage = SplashDamage - Mathf.Max(0, Falloff * (distance - SplashRadius));
			hit.GetComponent<CHealth>()?.Damage(damage);
		}
		Debug.Log("EXPLOSION ANIMATION GOES HERE");
		Destroy(gameObject);
	}

	protected virtual IEnumerator startTimer () {
		ticking = true;
		Debug.Log("TICKING ANIMATION HERE");
		yield return new WaitForSeconds(Timer);
		Explode();
	}

	public void StartTimer () {
		StartCoroutine(startTimer());
	}

}
