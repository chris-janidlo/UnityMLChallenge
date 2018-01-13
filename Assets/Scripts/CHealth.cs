using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CHealth : MonoBehaviour {

	public float MaxHealth;
	[SerializeField] private float currentHealth;
	public virtual float Health {
		get { return currentHealth; }
		private set { currentHealth = value; }
	}

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

}
