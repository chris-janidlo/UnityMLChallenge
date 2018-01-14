using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(WeaponManager))]
[RequireComponent(typeof(CMovement))]
[RequireComponent(typeof(CHealth))]
[RequireComponent(typeof(CGrenadeThrower))]
public class BaseAgent : Agent {

	private WeaponManager wm;
	private CMovement movement;
	private CHealth health;
	private CGrenadeThrower grenadeThrower;

	public override void InitializeAgent () {
		wm = GetComponent<WeaponManager>();
		movement = GetComponent<CMovement>();
		health = GetComponent<CHealth>();
		grenadeThrower = GetComponent<CGrenadeThrower>();
	}

	// state map
	//	0-2: world position
	//		x,y,z in order
	//	3-5: rotation
	//		Euler angles, x,y,z in order
	//	6: health
	//	7: current weapon slot
	//	8: current weapon ammo
	//		-1 if infinite
	//	9: weapon status
	//		0: good to fire
	//		1: reloading
	//		2: swapping
	//	10: grenade cooldown
	//	11+: game states, other agents
	//		unmapped
	public override List<float> CollectState () {
		List<float> state = new List<float>(new float[] {
			transform.position.x,
			transform.position.y,
			transform.position.z,
			transform.rotation.eulerAngles.x,
			transform.rotation.eulerAngles.y,
			transform.rotation.eulerAngles.z,
			health.Health,
			wm.ActiveWeaponSlot,
			wm.ActiveWeaponMagazine == null ? -1 : wm.ActiveWeaponMagazine.Ammo,
			wm.Swapping ? 2 : (wm.Reloading ? 1 : 0),
			grenadeThrower.CooldownTimer,
		});

		return state;
	}

	// action map
	//	0,1: movement
	//		0 is forward movement, 1 is strafing. range -1, 1
	//	2,3: look
	//		2 is horizontal, 3 is vertical. range -1, 1
	//	4: jump
	//		1-hot
	//	5: shoot
	//		1-hot
	//	6: reload
	//		1-hot
	//	7: quick swap weapon
	//		1-hot
	//	8: throw grenade
	//		1-hot
	public override void AgentStep (float[] act) {
		// actions
		movement.Move(new Vector2(act[0], act[1]));
		movement.MoveEyes(new Vector2(act[2], act[3]));
		if (act[4] == 1)
			movement.Jump();
		if (act[5] == 1)
			wm.Shoot();
		if (act[6] == 1)
			wm.Reload();
		if (act[7] == 1)
			wm.QuickSwapWeapon();
		if (act[8] == 1)
			wm.ThrowGrenade();


		// rewards
		float rewardIncrement = 0;

		// TODO: implement rewards

		reward += rewardIncrement;
	}

	public override void AgentReset () {

	}

}
