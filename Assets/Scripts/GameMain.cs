using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using CustomPhysics;

public class GameMain : MonoBehaviour
{
    [SerializeField] PlayerControl _player = null;
    [SerializeField] InputControl _input = null;

    void Awake() {
        var levelManager = LevelManager.GetInstance();
        AssetManager.GetInstance().Initalize();
        CollisionManager.GetInstance().Initalize();
        levelManager.Initalize();

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
    }
}
