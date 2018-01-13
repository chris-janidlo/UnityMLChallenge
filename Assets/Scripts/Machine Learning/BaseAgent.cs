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

	public override void AgentStep (float[] act) {
		movement.Move(new Vector2(act[0], act[1]));
		if (act[2] == 1)
			movement.Jump();
	}

	public override void AgentReset () {

	}

}
