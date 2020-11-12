using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace CustomParticleSystem {
    public class CustomParticle : ILoopable {
        #region Public Fields
        public Vector3 position;
        public string effectName; 
        public float angle, angleRate; 
        public float speed; 
        public float gravity; 
        public float friction; 
        public float lifeTime; 
        public float scale, scaleRate;
        #endregion
        MeshParticleSystem _meshParticleSystem;
        Vector2 _defaultQuadSize;
        int _quadIndex;
        int _uvIndex;
        int _maxIndex;
        float _uvIndexTimer;
        float _uvIndexTimerMax;
        Vector3 _direction;

        public void Initalize(MeshParticleSystem system, Vector2 quadSize, float timerMax) {
            _meshParticleSystem = system;
            _defaultQuadSize = quadSize;
            _uvIndexTimerMax = timerMax;
            _maxIndex = system.GetMaxIndex();
            _uvIndex = 0;

            _quadIndex = _meshParticleSystem.AddQuad(position, 0f, quadSize, false, _uvIndex);
            _meshParticleSystem.UpdateQuad(_quadIndex, position, angle, _defaultQuadSize * scale, false, _uvIndex);
        }

        public void ChangeMoveDir(float dir) {
            float radian = dir * Mathf.Deg2Rad;
            _direction = new Vector3(Mathf.Cos(radian), Mathf.Sin(radian)).normalized;
        }

        public void Progress() {
            UpdateFields();
            _uvIndexTimer += Time.deltaTime;

            if (_uvIndexTimer >= _uvIndexTimerMax) {
                _uvIndexTimer -= _uvIndexTimerMax;
                _uvIndex = Mathf.Min(_uvIndex + 1, _maxIndex - 1);
                
                _meshParticleSystem.UpdateQuad(
                                        _quadIndex, 
                                        position, 
                                        angle,
                                        _defaultQuadSize * scale, 
                                        false, 
                                        _uvIndex);
            }
        }

        void UpdateFields() {
            lifeTime -= Time.deltaTime;

            float currentFriction = (Mathf.Abs(speed) < Mathf.Epsilon) ? 0f : friction;
            speed -= currentFriction * Time.deltaTime;
            if (speed < Mathf.Epsilon)
                speed = 0f;

            position += _direction * speed * Time.deltaTime;
            position += Vector3.down * gravity * Time.deltaTime;
            angle += angleRate;

            scale += scaleRate;
            if (scale < Mathf.Epsilon) {
                scale = 0f;
            }
        }

        public bool IsLifeTimeEnd() {
            return lifeTime <= 0f;
        }
        public void DestroySelf() {
            _meshParticleSystem.DestroyQuad(_quadIndex);
        }
    }
}
