using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.U2D;
using CustomPhysics;

public abstract class EnemyBase : MonoBehaviour, ISetupable, ILoopable {
    [SerializeField] SpriteAtlas _modelAtlas = null;
    [SerializeField] RectCollider _chaseRange = null, _attackRange = null;
    protected SpriteAtlasAnimator _animator;
    protected int _maxHp, _currentHp;
    public virtual void Initalize() {
        
        _chaseRange.OnCollisionEvent.AddListener(OnChaseDetected);
        _attackRange.OnCollisionEvent.AddListener(OnAttackDetected);
    }
    protected void InitalizeAnimator(string prefix, string stateName) {
        _animator = new SpriteAtlasAnimator(GetComponent<SpriteRenderer>(), prefix, stateName, true);
    }

    public virtual void Progress() {
        _animator.Progress(_modelAtlas);
    }

    public void Reset() {
        _currentHp = _maxHp;
        gameObject.SetActive(false);
    }

    protected abstract void OnChaseDetected();
    protected abstract void OnAttackDetected();
}
