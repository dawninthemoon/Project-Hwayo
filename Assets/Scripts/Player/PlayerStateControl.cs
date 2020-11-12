using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.U2D;
using MonsterLove.StateMachine;

public partial class PlayerStateControl : MonoBehaviour {
    public enum States { 
        Idle, AttackA, AttackB, Ready, JumpAttack, Dead, Evade, Explode, 
        Hit, Jump, Run, Slide, Shoot, JumpShoot 
    };
    SpriteAtlas _spriteAtlas;
    StateMachine<States> _fsm;
    SpriteAtlasAnimator _animator;
    PlayerMeleeAttack _meleeAttackControl;

    #region Non-reference Fields
    Vector2 _direction;
    bool _jumpRequested;
    #endregion
    public States State { get => _fsm.State; }

    public void Initalize(PlayerMeleeAttack meleeAttack) {
        _spriteAtlas = Resources.Load<SpriteAtlas>("Atlas/CharacterAtlas1");
        _animator = new SpriteAtlasAnimator();
        _fsm = GetComponent<StateMachineRunner>().Initialize<States>(this);
        _meleeAttackControl = meleeAttack;

        _animator.Initalize(GetComponentInChildren<SpriteRenderer>(), "PLAYER_", "Idle_loop", true, 1f);
        _fsm.ChangeState(States.Idle);
    }

    public void Progress() {
        _animator.Progress(_spriteAtlas);
    }

    public void ApplyAnimation(float dirX, float velocityY) {
        _direction.x = dirX;
        _direction.y = velocityY;

        if (Mathf.Abs(dirX) > Mathf.Epsilon) {
            SetDirection(dirX);
        }
    }

    public void OnJumpButtonDown() {
        _jumpRequested = true;
    }

    public void SetDirection(float dirX) {
        Vector3 scaleVector = new Vector3(Mathf.Sign(dirX), 1f, 1f);
        transform.localScale = scaleVector;
    }
}
