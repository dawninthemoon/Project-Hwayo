using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace CustomPhysics {
    public enum ColliderLayerMask {
        Default,
        Ground,
        ObjectHitbox,
        AttackHitbox,
    }
    public abstract class CustomCollider : MonoBehaviour, IQuadTreeObject {
        [SerializeField] ColliderLayerMask _colliderLayer = ColliderLayerMask.Default;
        public ColliderLayerMask Layer { 
            get { return _colliderLayer; }
            set {
                if (_colliderLayer == value) return;
                _colliderLayer = value;
                InitalizeLayerMask();
            }
        }
        public string Tag { get; set; }
        int _layerMask;

        protected virtual void Start() {
            CollisionManager.GetInstance().AddCollider(this);
            InitalizeLayerMask();
        }
        void InitalizeLayerMask() {
            switch (_colliderLayer) {
            case ColliderLayerMask.Default:
                _layerMask = 1;
            break;
            case ColliderLayerMask.Ground:
                _layerMask = 0;
            break;
            case ColliderLayerMask.ObjectHitbox:
                AddBitMask(ColliderLayerMask.AttackHitbox);
            break;
            case ColliderLayerMask.AttackHitbox:
                _layerMask = 0;
            break;
            }
        }

        void AddBitMask(ColliderLayerMask targetMask) {
            _layerMask |= (1 << (int)targetMask);
        }
        void RemoveBitMask(ColliderLayerMask targetMask) {
            _layerMask &= ~(1 << (int)targetMask);
        }
        public bool CannotCollision(ColliderLayerMask other) {
            return (_layerMask & (1 << (int)other)) == 0;
        }
        public abstract bool IsCollision(CustomCollider collider);
        public abstract void OnCollision(CustomCollider collider);
        public abstract Rectangle GetBounds();
    }
}
