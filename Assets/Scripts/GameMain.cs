using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameMain : MonoBehaviour
{
    [SerializeField] PlayerControl _player = null;
    [SerializeField] InputControl _input = null;

    void Start() {
        _player.Initalize();
        _input.Initalize(_player);
    }

    void Update() {
        _input.Progress();
        _player.Progress();
    }

    void FixedUpdate() {
        _player.FixedProgress();
    }
}
