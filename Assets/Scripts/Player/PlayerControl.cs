using UnityEngine;
using Aroma;

[RequireComponent (typeof (GroundController))]
public class PlayerControl : MonoBehaviour, ISetupable, ILoopable {
	PlayerMovementControl _movementControl;
	PlayerStateControl _stateControl;
	private Vector2 _directionalInput;

	public void Initalize() {
		var controller = GetComponent<GroundController>();
		var movementSetting = GetComponentInChildren<TBLPlayerMovement>();
		var wallJumpSetting = GetComponentInChildren<TBLPlayerWallJump>();

		_movementControl = new PlayerMovementControl(controller, movementSetting, wallJumpSetting);
		_movementControl.Initalize();

		_stateControl = GetComponent<PlayerStateControl>();
		_stateControl.Initalize();
	}

	public void Progress() {
		_movementControl.CalculateVelocity(_directionalInput);
		_movementControl.HandleWallSliding(_directionalInput);

		_movementControl.MovePlayer(_directionalInput);

		_movementControl.PostCalculateVelocity();
        _stateControl.ApplyAnimation(_directionalInput.x, _movementControl.Velocity.y);

		_stateControl.Progress();
	}

	public void SetInputX(float horizontal) {
		// IF Player is in attack progress, set horizontal to zero.
		_directionalInput.x = horizontal;
	}

	public void SetInputY(float vertical) {
		// IF Player is in attack progress, set vertical to zero.
		_directionalInput.y = vertical;
	}

	public void OnJumpInputDown() {
		_movementControl.OnJumpInputDown(_directionalInput);
		_stateControl.OnJumpButtonDown();
	}

	public void OnJumpInputUp() {
		_movementControl.OnJumpInputUp();
	}
}
