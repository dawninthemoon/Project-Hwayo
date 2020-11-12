using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovementControl : ISetupable {
    float _maxJumpHeight;
	float _minJumpHeight;
	float _timeToJumpApex;
    float _moveSpeed;

    Vector2 _wallJumpClimb;
	Vector2 _wallJumpOff;
	Vector2 _wallLeap;

	float _wallSlideSpeedMax;
	float _wallStickTime;
    float _timeToWallUnstick;

    Vector3 _velocity;
	float _gravity;
	float _maxJumpVelocity;
	float _minJumpVelocity;

	bool _wallSliding;
	int _wallDirX;

    GroundController _controller;
    public PlayerMovementControl(GroundController controller, TBLPlayerMovement movementSetting, TBLPlayerWallJump wallJumpSetting) {
        _controller = controller;

        _maxJumpHeight = movementSetting.maxJumpHeight;
        _minJumpHeight = movementSetting.minJumpHeight;
        _moveSpeed = movementSetting.moveSpeed;
        _timeToJumpApex = movementSetting.timeToJumpApex;

        _wallJumpClimb = wallJumpSetting.wallJumpClimb;
        _wallJumpOff = wallJumpSetting.wallJumpOff;
        _wallLeap = wallJumpSetting.wallLeap;
        _wallSlideSpeedMax = wallJumpSetting.wallSlideSpeedMax;
        _wallStickTime = wallJumpSetting.wallStickTime;
    }

    public void Initalize() {
        _controller.Initalize();

		_gravity = -(2 * _maxJumpHeight) / Mathf.Pow (_timeToJumpApex, 2);
		_maxJumpVelocity = Mathf.Abs(_gravity) * _timeToJumpApex;
		_minJumpVelocity = Mathf.Sqrt(2 * Mathf.Abs (_gravity) * _minJumpHeight);
    }

    public void MovePlayer(Vector2 dir) {
        _controller.Move(_velocity * Time.deltaTime, dir);
    }

    public void CalculateVelocity(Vector2 dir) {
        float targetVelocityX = dir.x * _moveSpeed;
		_velocity.x = targetVelocityX;
		_velocity.y += _gravity * Time.deltaTime;
    }

    public void HandleWallSliding(Vector2 dir) {
        _wallDirX = (_controller.collisions.left) ? -1 : 1;
		_wallSliding = false;
		if ((_controller.collisions.left || _controller.collisions.right) && !_controller.collisions.below && _velocity.y < 0f) {
			_wallSliding = true;

			if (_velocity.y < -_wallSlideSpeedMax) {
				_velocity.y = -_wallSlideSpeedMax;
			}

			if (_timeToWallUnstick > 0f) {
				_velocity.x = 0f;

				if (dir.x != _wallDirX && dir.x != 0f) {
					_timeToWallUnstick -= Time.deltaTime;
				}
				else {
					_timeToWallUnstick = _wallStickTime;
				}
			}
			else {
				_timeToWallUnstick = _wallStickTime;
			}

		}
    }

    public void PostCalculateVelocity() {
        if (_controller.collisions.above || _controller.collisions.below) {
			if (_controller.collisions.slidingDownMaxSlope) {
				_velocity.y += _controller.collisions.slopeNormal.y * -_gravity * Time.deltaTime;
			} else {
				_velocity.y = 0f;
			}
		}
    }

    public void OnJumpInputDown(Vector2 dir) {
        if (_wallSliding) {
			if (_wallDirX == dir.x) {
				_velocity.x = -_wallDirX * _wallJumpClimb.x;
				_velocity.y = _wallJumpClimb.y;
			}
			else if (dir.x == 0f) {
				_velocity.x = -_wallDirX * _wallJumpOff.x;
				_velocity.y = _wallJumpOff.y;
			}
			else {
				_velocity.x = -_wallDirX * _wallLeap.x;
				_velocity.y = _wallLeap.y;
			}
		}
		if (_controller.collisions.below) {
			if (_controller.collisions.slidingDownMaxSlope) {
				if (dir.x != -Mathf.Sign (_controller.collisions.slopeNormal.x)) { 
					_velocity.y = _maxJumpVelocity * _controller.collisions.slopeNormal.y;
					_velocity.x = _maxJumpVelocity * _controller.collisions.slopeNormal.x;
				}
			} else {
				_velocity.y = _maxJumpVelocity;
			}
		}
    }

    public void OnJumpInputUp() {
		if (_velocity.y > _minJumpVelocity) {
			_velocity.y = _minJumpVelocity;
		}
	}
}
