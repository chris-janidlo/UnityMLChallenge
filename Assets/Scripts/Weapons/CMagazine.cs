using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CMagazine : MonoBehaviour {

	// how much supply is left
	public virtual float Ammo {
		get { return ammo; }
		set { ammo = Mathf.Max(value, 0); }
	}

	// ammo at start and after reload
	public float BaseAmmo;

	// time it takes to reload
	public float ReloadTime;

	public bool Reloading { get; private set; }
	private float ammo;


	protected virtual void Start () {
		Ammo = BaseAmmo;
		Reloading = false;
	}

	public virtual void Reload() {
		StartCoroutine(reloadRoutine());
	}

	protected virtual IEnumerator reloadRoutine() {
		if (Reloading)
			yield break;
		Reloading = true;

		Debug.Log("RELOADING ANIMATION HERE");
		yield return new WaitForSeconds(ReloadTime);

		Ammo = BaseAmmo;
		Reloading = false;
	}

}
