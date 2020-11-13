using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.U2D;

public class EnemyBase : MonoBehaviour, ISetupable, ILoopable {
    [SerializeField] SpriteAtlas _modelAtlas = null;
    SpriteAtlasAnimator _animator;
    int _maxHp, _currentHp;
    public void Initalize() {
        string prefix = "";
        _animator = new SpriteAtlasAnimator(GetComponent<SpriteRenderer>(), prefix, "idle", true);
    }

    public void Progress() {
        _animator.Progress(_modelAtlas);
    }
}
