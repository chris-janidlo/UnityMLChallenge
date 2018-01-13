using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(CGrenadeThrower))]
public class WeaponManager : MonoBehaviour {

	// guns and other weapons that use left click controls
	public CShooty[] MainWeapons;
	// place relative to player model's center to place weapons
	public Vector3 WeaponLocation;

	// readonly states
	public CShooty ActiveWeapon { get; private set; }
	public CMagazine ActiveWeaponMagazine { get; private set; }
	public int ActiveWeaponSlot { get; private set; }
	
	private CGrenadeThrower grenade;
	private int lastWeaponSlot = 1;
	private bool swapping = false;

	void Awake () {
		ActiveWeaponSlot = 0;
		grenade = GetComponent<CGrenadeThrower>();
	}

	void Start () {
		pullOutWeapon();
	}

	public void Shoot() {
		if (!swapping)
			ActiveWeapon.Shoot();
	}

	public void ThrowGrenade() {
		grenade.Throw(transform.position, transform.forward);
	}

	public void Reload() {
		if (ActiveWeaponMagazine != null)
			ActiveWeaponMagazine.Reload();
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
		ActiveWeapon = Instantiate(MainWeapons[ActiveWeaponSlot], WeaponLocation, Quaternion.identity, transform);
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
