using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace CustomPhysics {
    public abstract class CustomCollider : MonoBehaviour {
        public string Tag { get; set; }

        protected virtual void Start() {
            CollisionManager.GetInstance().AddCollider(this);
        }

        public abstract bool IsCollision(CustomCollider collider);
        public abstract void OnCollision(CustomCollider collider);
    }
}
