﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.U2D;

public class SpriteAtlasAnimator {
    public delegate void OnAnimationEnd();
    static readonly float _defaultSpeed = 0.06f;
    float _animatonSpeed;
    string _prefix;
    float _indexTimer = 0f;
    int _spriteIndex = 1;
    string _animationName;
    bool _loop;
    public int SpriteIndex { set { _spriteIndex = value; } get { return _spriteIndex; } }
    OnAnimationEnd _animationEndCallback;
    SpriteRenderer _renderer;

    public SpriteAtlasAnimator(SpriteRenderer renderer, string prefix, string idleStateName, bool loop = false, float speed = 1f) {
        _renderer = renderer;
        _prefix = prefix;
        ChangeAnimation(idleStateName, loop, speed);
    }

    public void ChangeAnimation(string name, bool loop = false, float speed = 1f, OnAnimationEnd callback = null) {
        _indexTimer = _defaultSpeed;
        _spriteIndex = 1;
        _animatonSpeed = speed;
        _animationName = name;
        _loop = loop;
        _animationEndCallback = callback;
    }

    public void Progress(SpriteAtlas atlas) {
        _indexTimer += Time.deltaTime * _animatonSpeed;
        if (_indexTimer > _defaultSpeed) {
            _indexTimer = 0f;
            var currentFrame = GetSprite();
            if (currentFrame == null) {
                if (_loop) {
                    _spriteIndex = 1;
                    currentFrame = GetSprite();
                }
                else {
                    _animationEndCallback?.Invoke();
                }
            }

            if (currentFrame != null) {
                _renderer.sprite = currentFrame;
                ++_spriteIndex;
            }
        }

        Sprite GetSprite() {
            string spriteName = Aroma.StringUtils.MergeStrings(_prefix, _animationName, _spriteIndex.ToString());
            var currentFrame = atlas.GetSprite(spriteName);
            return currentFrame;
        }
    }
}