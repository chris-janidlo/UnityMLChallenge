using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CHealth : MonoBehaviour {

	public float MaxHealth;
	public virtual float Health { get; private set; }

	protected virtual void Awake () {
		Health = MaxHealth;
	}

	public virtual void Damage (float damage) {
		Health -= damage;
		if (Health <= 0) {
			Debug.Log("DEATH ANIMATION HERE");
			Destroy(gameObject);
		}
	}

	// tries to deal damage to the game object by finding a CHealth. if successful, returns true; if not, returns false
	public static bool TryToDamage (GameObject target, float damage) {
		CHealth comp = target.GetComponent<CHealth>();
		if (comp == null)
			return false;
		comp.Damage(damage);
		return true;
	}

}
