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

		var particleManager = CustomParticleSystem.ParticleManager.GetInstance();
        float r = Random.Range(0f, 360f);
        particleManager.SpawnParticle(transform.position, "EFFECT_Fire_strip3", r, 3f, 90f + Random.Range(-30f, 30f), Random.Range(20f, 30f), -10f, 40f, 4f, 0.2f, -0.001f);
		
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
