using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(CGrenadeThrower))]
public class WeaponManager : MonoBehaviour {

	// guns and other weapons that use left click controls
	public CShooty[] MainWeapons;
	// if true, reload automatically when out of ammo
	public bool AutoReload;
	// place relative to player model's center to place weapons
	public Vector3 WeaponLocation;
	// gun is instantiated here so that it moves with camera
	public Transform Head;

	// readonly states
	public CShooty ActiveWeapon { get; private set; }
	public CMagazine ActiveWeaponMagazine { get; private set; }
	public int ActiveWeaponSlot { get; private set; }
	public bool Swapping { get { return swapping; } }
	public bool Reloading { get { return ActiveWeaponMagazine?.Reloading ?? false; } }
	
	private CGrenadeThrower grenade;
	private Rigidbody rb;
	private int lastWeaponSlot = 1;
	private bool swapping = false;

	void Awake () {
		if (Head.parent != transform)
			throw new Exception("Head must be a child of this.transform");
		ActiveWeaponSlot = 0;
		grenade = GetComponent<CGrenadeThrower>();
		rb = GetComponent<Rigidbody>();
	}

	void Start () {
		pullOutWeapon();
	}

	public void Shoot() {
		if (!swapping) {
			if (ActiveWeaponMagazine == null || ActiveWeaponMagazine.Ammo > 0)
				ActiveWeapon.Shoot();
			else {
				if (AutoReload)
					Reload();
				else
					Debug.Log("CLICK");
			}
		}
	}

	public void ThrowGrenade() {
		Vector3 offset = new Vector3(0, 0, 0.6f);
		grenade.Throw(Head, offset, rb?.velocity);
	}

	public void Reload() {
		ActiveWeaponMagazine?.Reload();
	}

	public void SwapWeapon(int slot) {
		StartCoroutine(switchWeapon(slot));
	}

	public void QuickSwapWeapon() {
		StartCoroutine(switchWeapon(lastWeaponSlot));
	}

	private void putDownWeapon () {
		Debug.Log("PUT WEAPON LOWER ANIMATION HERE");
		Destroy(ActiveWeapon.gameObject);
	}

	private void pullOutWeapon () {
		Debug.Log("PUT WEAPON PULL ANIMATION HERE");
		ActiveWeapon = Instantiate(MainWeapons[ActiveWeaponSlot], Head.TransformPoint(WeaponLocation), Head.rotation, Head);
		ActiveWeaponMagazine = ActiveWeapon.GetComponent<CMagazine>();
	}

	private IEnumerator switchWeapon(int slot) {
		if (!(slot > -1 && slot < MainWeapons.Length) || swapping)
			yield break;

		swapping = true;
		putDownWeapon();

		CShooty nextWeapon = MainWeapons[slot];
		yield return new WaitForSeconds(nextWeapon.SwapTime);

		lastWeaponSlot = ActiveWeaponSlot;
		ActiveWeaponSlot = slot;
		pullOutWeapon();
		swapping = false;
	}
	
}
