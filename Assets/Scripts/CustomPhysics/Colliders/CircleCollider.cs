#pragma warning disable 0649
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace CustomPhysics {
    [System.Serializable]
    public struct Circle {
        public Vector2 center;
        public float radius;
        public Circle(Vector2 center, float radius) {
            this.center = center;
            this.radius = radius;
        }
    }

    public class CircleCollider : CustomCollider {
        [SerializeField] Circle _circle;
        public Circle CircleShape { get { return _circle; } }
        protected override void Start() {
            base.Start();
        }
        public override Rectangle GetBounds() {
            Rectangle bounds = new Rectangle(_circle.center, _circle.radius * 2f, _circle.radius * 2f);
            bounds.center += (Vector2)transform.position;
            return bounds;
        }
        public override bool IsCollision(CustomCollider other) {
            return IsCollision(other);
        }
        public bool IsCollision(RectCollider other) {
            return CollisionManager.GetInstance().IsCollision(other, this);
        }
        public bool IsCollision(CircleCollider other) {
            return CollisionManager.GetInstance().IsCollision(other, this);
        }
        void OnDrawGizmos() {
            Vector2 position = (Vector2)transform.position + _circle.center;
            Vector2 cur = position + _circle.radius * new Vector2(Mathf.Cos(0f), Mathf.Sin(0f));
            Vector2 prev = cur;
            
            Gizmos.color = Color.red;
            for (float theta = 0.1f; theta < Mathf.PI * 2f; theta += 0.1f) {
                cur = position + _circle.radius * new Vector2(Mathf.Cos(theta), Mathf.Sin(theta));
                Gizmos.DrawLine(cur, prev);
                prev = cur;
            }
            cur = position + _circle.radius * new Vector2(Mathf.Cos(0f), Mathf.Sin(0f));
            Gizmos.DrawLine(cur, prev);
        }
    }
}
