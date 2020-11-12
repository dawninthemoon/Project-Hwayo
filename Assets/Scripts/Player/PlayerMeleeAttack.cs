using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using CustomPhysics;
using Aroma;

public class PlayerMeleeAttack : ILoopable {
    static readonly float InitalInputDelay = 0.15f;
    static readonly float InputDelayAfterCombo = 0.05f;
    static readonly int MaxRequestCount = 1;
    float _inputDelay;
    bool _attackPressedAtCurrentFrame;
    public int RequestedAttackCount { get; set; }
    public bool IsInAttackProgress { get; private set; }
    public List<CustomCollider> AlreadyHitColliders { get; private set; } = new List<CustomCollider>();

    public void Progress() {
        _inputDelay -= Time.deltaTime;

        if (_attackPressedAtCurrentFrame && (_inputDelay < 0f)) {
            _inputDelay = InputDelayAfterCombo;
            RequestedAttackCount = Mathf.Min(RequestedAttackCount + 1, MaxRequestCount);
        }
    }

    public void SetAttackInput(bool pressed) {
        _attackPressedAtCurrentFrame = pressed;
    }

    public void EnableMeleeAttack(Vector2 position, float faceDir) {
        Vector2 offset = Vector2.zero;
        //Vector2 offset = _meleeAttackOffset.ChangeXPos(_meleeAttackOffset.x * transform.localScale.x);
        //EnableHitbox(position + offset, _meleeAttackSize, _meleeAttackDamage, HitEffectName);
    }

    private void EnableHitbox(Vector2 position, Vector2 size, int damage, string hitEffectName) {
        /*
        Collider2D[] colliders = Physics2D.OverlapBoxAll(position, size, 0f, _attackableLayers);
        bool enemyHit = false;
        for (int i = 0; i < colliders.Length; i++) {
            if (!IsAlreadyExists(colliders[i])) {
                AlreadyHitColliders.Add(colliders[i]);
                EnemyBase enemy = colliders[i].gameObject.GetComponentNoAlloc<EnemyBase>();
                if (enemy != null) {
                    enemyHit = OnEnemyHit(enemy, damage, hitEffectName);
                }
                else {
                    var soul = colliders[i].gameObject.GetComponentNoAlloc<Soul>();
                    if (soul.Simulated)
                        soul.OnHit();
                }
            }
        }*/
        
        //Time.timeScale = enemyHit ? 0f : 1f;
    }

    public void AttackEnd() {
        _inputDelay = InitalInputDelay;
        IsInAttackProgress = false;
    }
}
