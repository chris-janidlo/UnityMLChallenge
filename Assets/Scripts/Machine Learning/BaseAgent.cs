using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(WeaponManager))]
[RequireComponent(typeof(CMovement))]
public class BaseAgent : Agent {

	private WeaponManager wm;
	private CMovement movement;

	public override void InitializeAgent () {
		wm = GetComponent<WeaponManager>();
		movement = GetComponent<CMovement>();
	}

	public override List<float> CollectState () {
		List<float> state = new List<float>();

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
