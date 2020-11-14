using UnityEngine;
using Aroma;

[RequireComponent (typeof (GroundController))]
public class PlayerControl : MonoBehaviour, ISetupable, ILoopable {
	GroundMovementControl _movementControl;
	PlayerStateControl _stateControl;
	PlayerMeleeAttack _meleeAttack;
	private Vector2 _directionalInput;

	public void Initalize() {
		var controller = GetComponent<GroundController>();
		var movementSetting = GetComponentInChildren<TBLGroundMovement>();
		var wallJumpSetting = GetComponentInChildren<TBLPlayerWallJump>();

		_movementControl = new GroundMovementControl(controller, movementSetting, wallJumpSetting);
		_movementControl.Initalize();

		_meleeAttack = new PlayerMeleeAttack();

		_stateControl = GetComponent<PlayerStateControl>();
		_stateControl.Initalize(_meleeAttack);
	}

	public void Progress() {
		_movementControl.CalculateVelocity(_directionalInput);
		_movementControl.HandleWallSliding(_directionalInput);
		_movementControl.MoveObject(_directionalInput);

		_movementControl.PostCalculateVelocity();

		_meleeAttack.Progress();

        _stateControl.ApplyAnimation(_directionalInput.x, _movementControl.Velocity.y);
		_stateControl.Progress();
		
	}

	public void SetInputX(float horizontal) {
		if (_meleeAttack.IsInAttackProgress)
			horizontal = 0f;
		if ((Mathf.Abs(_movementControl.Velocity.y) < Mathf.Epsilon) && _stateControl.State == PlayerStateControl.States.JumpAttack)
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
