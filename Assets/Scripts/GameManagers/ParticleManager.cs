using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace CustomParticleSystem {
    public class ParticleManager : Singleton<ParticleManager>, ISetupable, ILoopable {
        ObjectPool<CustomParticle> _particlePool;
        List<CustomParticle> _activeParticles;
        Dictionary<string, MeshParticleSystem> _nameParticleSystemDic;
        public void Initalize() {
            _activeParticles = new List<CustomParticle>();
            _nameParticleSystemDic = new Dictionary<string, MeshParticleSystem>();
            _particlePool = new ObjectPool<CustomParticle>(
                100,
                () => new CustomParticle(),
                null,
                null
            );

            InitMeshParticleSystems();
        }

        public void Progress() {
            for (int i = 0; i < _activeParticles.Count; ++i) {
                var particle = _activeParticles[i];
                particle.Progress();

                if (particle.IsLifeTimeEnd()) {
                    _activeParticles.RemoveAt(i--);
                    _particlePool.ReturnObject(particle);
                    particle.DestroySelf();
                }
            }
        }

        public void LateProgress() {
            foreach (var particleSystem in _nameParticleSystemDic.Values) {
                particleSystem.LateProgress();
            }
        }

        public CustomParticle SpawnParticle(
                        Vector3 pos, 
                        string effectName, 
                        float angle, 
                        float angleRate, 
                        float moveDir, 
                        float speed, 
                        float gravity, 
                        float friction, 
                        float lifeTime, 
                        float scale = 1f, 
                        float scaleRate = 0f,
                        float animationSpeed = 1f) {
            var particle = _particlePool.GetObject();

            #region Initalize
            particle.position = pos;
            particle.angle = angle;
            particle.angleRate = angleRate;
            particle.speed = speed;
            particle.gravity = gravity;
            particle.friction = friction;
            particle.lifeTime = lifeTime;
            particle.scale = scale;
            particle.scaleRate = scaleRate;
            particle.animationSpeed = animationSpeed;
            #endregion
        
            if (_nameParticleSystemDic.TryGetValue(effectName, out MeshParticleSystem particleSystem)) {
                particle.Initalize(particleSystem, Vector2.one * 64f, 0.06f);
                particle.ChangeMoveDir(moveDir);
            }
            else Debug.LogError("Effect Not Exists");

            _activeParticles.Add(particle);

            return particle;
        }

        void InitMeshParticleSystems() {
            var materials = AssetManager.GetInstance().GetAssetsInEffectBundle<Material>();
            foreach (var material in materials) {
                var particleSystem = new GameObject().AddComponent<MeshParticleSystem>();
                particleSystem.Initalize(material, 64f);
                _nameParticleSystemDic.Add(material.name, particleSystem);
            }
        }
    }
}
