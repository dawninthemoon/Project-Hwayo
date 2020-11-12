﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using CustomPhysics;
using CustomParticleSystem;

public class GameMain : MonoBehaviour
{
    [SerializeField] PlayerControl _player = null;
    [SerializeField] InputControl _input = null;

    ParticleManager _particleManager;

    void Awake() {
        _particleManager = ParticleManager.GetInstance();
        var levelManager = LevelManager.GetInstance();

        AssetManager.GetInstance().Initalize();
        CollisionManager.GetInstance().Initalize();
        levelManager.Initalize();
        _particleManager.Initalize();

        EventCommand.SharedData sharedData = new EventCommand.SharedData(
            _player,
            levelManager.TileGrid,
            levelManager.LevelDictionary
        );
        EventManager.GetInstance().Initalize(sharedData);

        levelManager.LoadLevel(levelManager.CurrentLevelName);
    }

    void Start() {
        _player.Initalize();
        _input.Initalize(_player);
    }

    void Update() {
        _input.Progress();
        _player.Progress();
        _particleManager.Progress();
    }

    void LateUpdate() {
        _particleManager.LateProgress();
    }
}
