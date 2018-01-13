using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// general use code for guns, without stats
public class CShooty : MonoBehaviour {

	public CBullet Bullet;
	public float ShotDelay;
	public float SwapTime;
	
	private CMagazine magazine;
	private float shootTimer;

	protected virtual void Start () {
		magazine = GetComponent<CMagazine>();
	}

	protected virtual void FixedUpdate() {
		shootTimer -= Time.deltaTime;
	}

	public virtual void Shoot() {
		if (shootTimer <= 0) {
			shootTimer = ShotDelay;

			if (magazine?.Ammo <= 0 || (magazine?.Reloading == true)) {
				Debug.Log("click");
				return;
			}

			Debug.Log("SHOOTING ANIMATION HERE");
			
			Vector3? target = hitPoint();
			CBullet b = Instantiate(Bullet, transform.position, transform.rotation);
			if (target != null)
				b.Initialize(((Vector3)target - transform.position).normalized);
			else
				b.Initialize(transform.forward.normalized);

			if (magazine != null)
				magazine.Ammo--;
		}
	}	

	//point in world space hit by crosshair, assuming crosshair is in middle of screen
	//returns null if nothing was hit
	protected Vector3? hitPoint() {
		RaycastHit hit;
		Ray ray = Camera.main.ViewportPointToRay(new Vector3(0.5f, 0.5f));
		if (Physics.Raycast(ray, out hit))
			return hit.point;
		else
			return null;
	}

}
