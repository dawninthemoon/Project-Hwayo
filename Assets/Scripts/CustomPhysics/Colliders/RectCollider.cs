#pragma warning disable 0649
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace CustomPhysics {
    [System.Serializable]
    public struct Rectangle {
        public Vector2 center;
        public float width;
        public float height;
        public float rotation;
        public Rectangle(float x, float y, float width, float height, float rotation = 0f) {
            this.center = new Vector2(x, y);
            this.width = width;
            this.height = height;
            this.rotation = rotation;
        }
        public Rectangle(Vector2 position, float width, float height, float rotation = 0f) {
            this.center = position;
            this.width = width;
            this.height = height;
            this.rotation = rotation;
        }
    }

    public class RectCollider : CustomCollider {
        [SerializeField] Rectangle _rect;
        protected override void Start() {
            base.Start();
        }
        public Rectangle GetBounds() {
            Rectangle newRectangle = _rect;
            newRectangle.center += (Vector2)transform.position;
            newRectangle.width *= Mathf.Abs(transform.localScale.x);
            newRectangle.height *= Mathf.Abs(transform.localScale.y);
            newRectangle.rotation += transform.localRotation.eulerAngles.z;
            return newRectangle;
        }

        public override bool IsCollision(CustomCollider other) {
            return IsCollision(other);
        }
        public bool IsCollision(RectCollider other) {
            return CollisionManager.GetInstance().IsCollision(this, other);
        }
        public bool IsCollision(CircleCollider other) {
            return CollisionManager.GetInstance().IsCollision(this, other);
        }
        public bool IsCollision(PolygonCollider other) => false;

        public override void OnCollision(CustomCollider collider) {

        }

        public Vector2 GetWidthVector() {
            Vector2 ret;
            ret.x = _rect.width * Mathf.Abs(transform.localScale.x) * Mathf.Cos(_rect.rotation) * 0.5f;
            ret.y = -_rect.width * Mathf.Abs(transform.localScale.x) * Mathf.Sin(_rect.rotation) * 0.5f;
            return ret;
        }
        public Vector2 GetHeightVector() {
            Vector2 ret;
            ret.x = _rect.height * transform.localScale.y * Mathf.Cos(_rect.rotation) * 0.5f;
            ret.y = -_rect.height * transform.localScale.y * Mathf.Sin(_rect.rotation) * 0.5f;
            return ret;
        }

        void OnDrawGizmos() {
            Vector2 cur = (Vector2)transform.position + _rect.center;
            float width = _rect.width * Mathf.Abs(transform.localScale.x);
            float height = _rect.height * Mathf.Abs(transform.localScale.y);
            Vector2 p00 = new Vector2(cur.x - width * 0.5f, cur.y + height * 0.5f);
            Vector2 p10 = new Vector2(cur.x + width * 0.5f, cur.y + height * 0.5f);
            Vector2 p11 = new Vector2(cur.x + width * 0.5f, cur.y - height * 0.5f);
            Vector2 p01 = new Vector2(cur.x - width * 0.5f, cur.y - height * 0.5f);
            
            float rotation = _rect.rotation + transform.localRotation.eulerAngles.z;
            float radian = rotation * Mathf.Deg2Rad;
            float radius = (p00 - cur).magnitude;
 
            RotatePoint(ref p00);
            RotatePoint(ref p10);
            RotatePoint(ref p11);
            RotatePoint(ref p01);

            Gizmos.color = Color.red;
            Gizmos.DrawLine(p00, p10);
            Gizmos.DrawLine(p10, p11);
            Gizmos.DrawLine(p11, p01);
            Gizmos.DrawLine(p01, p00);

            void RotatePoint(ref Vector2 p) {
                Vector2 diff = p - cur;
                float alteredRadian = radian + Mathf.Atan2(diff.y, diff.x);
                p = cur + radius * new Vector2(Mathf.Cos(alteredRadian), Mathf.Sin(alteredRadian));
            }
        }
    }
}
