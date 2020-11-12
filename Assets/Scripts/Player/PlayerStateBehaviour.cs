using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public partial class PlayerStateControl : MonoBehaviour
{
    #region IDLE
    private void Idle_Enter() {
        _animator.ChangeAnimation("Idle_loop", true);
    }

    private void Idle_Update() {
        if (_jumpRequested) {
            _fsm.ChangeState(States.Jump);
        }/*
        else if (_playerAttack.RequestShoot) {
            _fsm.ChangeState(States.Shoot);
        }
        else if (_playerAttack.RequestedAttackCount > 0) {
            _fsm.ChangeState(States.AttackA);
        }*/
        else if (Mathf.Abs(_direction.x) > Mathf.Epsilon) {
            _fsm.ChangeState(States.Run);
        }
    }
    #endregion

    #region RUN

    private void Run_Enter() {
        _animator.ChangeAnimation("Run_loop",true);
    }

    private void Run_Update() {
        if (_jumpRequested) {
            _fsm.ChangeState(States.Jump);
        }/*
        else if (_playerAttack.RequestShoot) {
            _fsm.ChangeState(States.Shoot);
        }
        else if (_playerAttack.RequestedAttackCount > 0) {
            _fsm.ChangeState(States.AttackA);
        }*/
        else if (Mathf.Abs(_direction.x) < Mathf.Epsilon) {
            _fsm.ChangeState(States.Idle);
        }
    }

    #endregion
/*
    #region Attack
    private void AttackA_Enter() {
        _direction.x = 0f;
        --_playerAttack.RequestedAttackCount;
        _playerAttack.AlreadyHitColliders.Clear();
        _animator.ChangeAnimation(
            "AttackA", 
            false, 
            () => {
                States nextState = (_playerAttack.RequestedAttackCount > 0) ? States.AttackB : States.AttackAOut;
                _fsm.ChangeState(nextState);
            }
        );
    }

    private void AttackA_Update() {
        _player.CanDrawHitbox = true;
        if (_animator.SpriteIndex == 0) {
            _playerAttack.EnableMeleeAttack();
        }
    }

    private void AttackB_Enter() {
        _playerAttack.AlreadyHitColliders.Clear();
        --_playerAttack.RequestedAttackCount;
        _animator.ChangeAnimation(
            "AttackB", 
            false,
            () => {
                States nextState = (_playerAttack.RequestedAttackCount > 0) ? States.AttackA : States.IdleIn;
                _fsm.ChangeState(nextState);
            }
        );
    }

    private void AttackB_Update() {
        _player.CanDrawHitbox = true;
        if (_animator.SpriteIndex == 0) {
            _playerAttack.EnableMeleeAttack();
        }
    }

    private void AttackAOut_Enter() {
        _animator.ChangeAnimation(
            "AttackAOut", 
            false,
            () => {
                States nextState = (Mathf.Abs(_direction.x) < Mathf.Epsilon) ? States.Idle : States.Run;
                _fsm.ChangeState(nextState);
            }
        );
    }
    private void AttackAir_Enter() {
        --_playerAttack.RequestedAttackCount;
        _playerAttack.AlreadyHitColliders.Clear();
        _animator.ChangeAnimation(
            "AttackAir",
            false,
            () => {
                _fsm.ChangeState(States.Jump);
            }
        );
    }
    private void AttackAir_Update() {
        _player.CanDrawHitbox = true;
        if (_animator.SpriteIndex == 0) {
            _playerAttack.EnableMeleeAttack();
        }
    }
    #endregion

    #region Shoot

    private void Shoot_Enter() {
        _playerAttack.RequestShoot = false;

        _animator.ChangeAnimation(
            "Shoot", 
            false,
            () => {
                States nextState = (Mathf.Abs(_direction.x) < Mathf.Epsilon) ? States.IdleIn : States.Run;
                _fsm.ChangeState(nextState);
            }
        );
        _playerAttack.CanShoot = true;
    }

    private void Shoot_Update() {
        if (_playerAttack.CanShoot && _animator.SpriteIndex == 1) {
            _playerAttack.CanShoot = false;
            _playerAttack.ShootTalisman();
        }
    }

    private void ShootAir_Enter() {
        _playerAttack.RequestShoot = false;
        _animator.ChangeAnimation(
            "ShootAir",
            false,
            ()=> 
            {
                _fsm.ChangeState(States.Jump);
            }
        );
    }
    private void ShootAir_Update() {
        Shoot_Update();
    }

    #endregion*/

    #region JUMP
    private void Jump_Enter() {
        _jumpRequested = false;
        _animator.ChangeAnimation("Jump");
    }

    private void Jump_Update() {
        /*if (_playerAttack.RequestShoot) {
            _fsm.ChangeState(States.ShootAir);
        }
        else if (_playerAttack.RequestedAttackCount > 0) {
            _fsm.ChangeState(States.AttackAir);
        }
        else*/ if (Mathf.Abs(_direction.y) < Mathf.Epsilon) {
            States nextState = (Mathf.Abs(_direction.x) < Mathf.Epsilon) ? States.Idle : States.Run;
            _fsm.ChangeState(nextState);
        }
        if ((_animator.SpriteIndex > 2) && Mathf.Sign(_direction.y) > 0f) {
            --_animator.SpriteIndex;
        }
    }
    #endregion
/*
    #region HIT

    public void SetPlayerHit() => _fsm.ChangeState(States.Hit);

    private void Hit_Enter() {
        _direction.x = -Mathf.Sign(_direction.x)*3f;
        _direction.y = 2f;
        _animator.ChangeAnimation(
            "Hit",
            false,
            () => {
                States nextState = (Mathf.Abs(_direction.y) < Mathf.Epsilon) ? States.Idle : States.Jump;
                _fsm.ChangeState(nextState);
            }
        );
    }
    
    #endregion*/
}
