using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public partial class PlayerStateControl : MonoBehaviour
{
    static bool AleadyJumpAttacked = false;

    #region IDLE
    private void Idle_Enter() {
        _animator.ChangeAnimation("Idle_loop", true, 0.5f);
        AleadyJumpAttacked = false;
    }

    private void Idle_Update() {
        if (_jumpRequested) {
            _fsm.ChangeState(States.Jump);
        }/*
        else if (_playerAttack.RequestShoot) {
            _fsm.ChangeState(States.Shoot);
        }*/
        else if (_meleeAttackControl.RequestedAttackCount > 0) {
            _fsm.ChangeState(States.AttackA);
        }
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
        }*/
        else if (_meleeAttackControl.RequestedAttackCount > 0) {
            _fsm.ChangeState(States.AttackA);
        }
        else if (Mathf.Abs(_direction.x) < Mathf.Epsilon) {
            _fsm.ChangeState(States.Idle);
        }
    }

    #endregion

    #region Attack
    private void AttackA_Enter() {
        _direction.x = 0f;
        --_meleeAttackControl.RequestedAttackCount;
        _meleeAttackControl.AlreadyHitColliders.Clear();
        _animator.ChangeAnimation(
            "Attack_A", 
            false,
            1f,
            () => {
                States nextState = (_meleeAttackControl.RequestedAttackCount > 0) ? States.AttackB : States.AttackAOut;
                _fsm.ChangeState(nextState);
            }
        );
    }

    private void AttackA_Update() {
        if (_animator.SpriteIndex == 0) {
            float faceDir = Mathf.Sign(transform.localScale.x);
            _meleeAttackControl.EnableMeleeAttack(transform.position, faceDir);
        }
    }

    private void AttackB_Enter() {
        _meleeAttackControl.AlreadyHitColliders.Clear();
        --_meleeAttackControl.RequestedAttackCount;
        _animator.ChangeAnimation(
            "Attack_B", 
            false,
            1f,
            () => {
                States nextState = (_meleeAttackControl.RequestedAttackCount > 0) ? States.AttackA : States.Idle;
                _fsm.ChangeState(nextState);
            }
        );
    }

    private void AttackB_Update() {
        if (_animator.SpriteIndex == 0) {
            float faceDir = Mathf.Sign(transform.localScale.x);
            _meleeAttackControl.EnableMeleeAttack(transform.position, faceDir);
        }
    }

    private void JumpAttack_Enter() {
        AleadyJumpAttacked = true;
        --_meleeAttackControl.RequestedAttackCount;
        _meleeAttackControl.AlreadyHitColliders.Clear();
        _animator.ChangeAnimation(
            "JumpAttack",
            false,
            1.25f,
            () => {
                if(_direction.y == 0f) {
                    _fsm.ChangeState(States.Idle);
                }
                else if(_direction.y < 0f) {
                    _fsm.ChangeState(States.Jump);
                }
            }
        );
    }
    private void JumpAttack_Update() {
        if (_animator.SpriteIndex == 0) {
            float faceDir = Mathf.Sign(transform.localScale.x);
            _meleeAttackControl.EnableMeleeAttack(transform.position, faceDir);
        }
    }
    #endregion
/*
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

    private void JumpShoot_Enter() {
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
    private void JumpShoot_Update() {
        Shoot_Update();
    }

    #endregion*/

    #region JUMP
     static float FlightRange = 20f;

    private void Jump_Enter() {
        _jumpRequested = false;
        _animator.ChangeAnimation("Jump");
        if (State == States.JumpAttack) _animator.SpriteIndex = 2;
    }

    private void Jump_Update() {
        /*if (_playerAttack.RequestShoot) {
            _fsm.ChangeState(States.ShootAir);
        }
        else*/ if (!AleadyJumpAttacked && _meleeAttackControl.RequestedAttackCount > 0) {
            _fsm.ChangeState(States.JumpAttack);
        }
        else if (Mathf.Abs(_direction.y) < Mathf.Epsilon) {
            States nextState = (Mathf.Abs(_direction.x) < Mathf.Epsilon) ? States.Idle : States.Run;
            _fsm.ChangeState(nextState);
        }
        
        if (State == States.JumpAttack) return;
        if(_direction.y < -FlightRange) {
            _animator.SpriteIndex = 3;
        }
        else if (_direction.y > FlightRange) {
            _animator.SpriteIndex = 1;
        }
        else {
            _animator.SpriteIndex = 2;
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
