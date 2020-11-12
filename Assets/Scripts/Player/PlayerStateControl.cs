using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.U2D;
using MonsterLove.StateMachine;

public partial class PlayerStateControl : MonoBehaviour {
    public enum States { 
        Idle, AttackA, AttackAOut, AttackB, AttackAir, Dead, Evade, Explode, 
        Hit, Jump, Run, Slide, Shoot, ShootAir 
    };
    private SpriteAtlas _spriteAtlas;
    public States State { get => _fsm.State; }
    private StateMachine<States> _fsm;
    private SpriteAtlasAnimator _animator;

    #region Non-reference Fields
    private Vector2 _direction;
    private bool _jumpRequested;
    #endregion

    public void Initalize() {
        _spriteAtlas = Resources.Load<SpriteAtlas>("Atlas/CharacterAtlas1");
        _animator = new SpriteAtlasAnimator();
        _fsm = GetComponent<StateMachineRunner>().Initialize<States>(this);

        _animator.Initalize(GetComponentInChildren<SpriteRenderer>(), "PLAYER_", "Idle_loop", true, 1f);
        _fsm.ChangeState(States.Idle);
    }

    public void Progress() {
        _animator.Progress(_spriteAtlas);
    }

    public void ApplyAnimation(float dirX, float velocityY) {
        _direction.x = dirX;
        _direction.y = (velocityY > 0f) ? 1f : ((velocityY < 0f) ? -1f : 0f);

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
