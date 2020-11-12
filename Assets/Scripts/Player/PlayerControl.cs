using UnityEngine;
using Aroma;

[RequireComponent (typeof (GroundController))]
public class PlayerControl : MonoBehaviour, ISetupable, ILoopable {
	PlayerMovementControl _movementControl;
	PlayerStateControl _stateControl;
	PlayerMeleeAttack _meleeAttack;
	private Vector2 _directionalInput;

	public void Initalize() {
		var controller = GetComponent<GroundController>();
		var movementSetting = GetComponentInChildren<TBLPlayerMovement>();
		var wallJumpSetting = GetComponentInChildren<TBLPlayerWallJump>();

		_movementControl = new PlayerMovementControl(controller, movementSetting, wallJumpSetting);
		_movementControl.Initalize();

		_meleeAttack = new PlayerMeleeAttack();

		_stateControl = GetComponent<PlayerStateControl>();
		_stateControl.Initalize(_meleeAttack);
	}

	public void Progress() {
		_movementControl.CalculateVelocity(_directionalInput);
		_movementControl.HandleWallSliding(_directionalInput);
		_movementControl.MovePlayer(_directionalInput);

		_movementControl.PostCalculateVelocity();

		_meleeAttack.Progress();

        _stateControl.ApplyAnimation(_directionalInput.x, _movementControl.Velocity.y);
		_stateControl.Progress();
	}

	public void SetInputX(float horizontal) {
		if (_meleeAttack.IsInAttackProgress)
			horizontal = 0f;
		_directionalInput.x = horizontal;
	}

	public void SetInputY(float vertical) {
		if (_meleeAttack.IsInAttackProgress)
			vertical = 0f;
		_directionalInput.y = vertical;
	}

	public void SetAttack(bool attackPressed) {
		_meleeAttack.SetAttackInput(attackPressed);
	}

	public void OnJumpInputDown() {
		_movementControl.OnJumpInputDown(_directionalInput);
		_stateControl.OnJumpButtonDown();
	}

	public void OnJumpInputUp() {
		_movementControl.OnJumpInputUp();
	}
}
