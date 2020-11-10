using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Aroma {
    public static class VectorUtility {
        public static float Cross(this Vector2 vec, Vector2 other) {
            return vec.x * other.y - vec.y * other.x;
        }
        public static bool Compare(this Vector2 p1, Vector2 p2, bool includeEqual = false) {
            if (includeEqual) {
                if (p1.x >= p2.x) return true;
                else if ((Mathf.Abs(p1.x - p2.x) < Mathf.Epsilon) && (p1.y >= p2.y)) return true;
                return false;
            }
            if (p1.x > p2.x) return true;
            else if ((Mathf.Abs(p1.x - p2.x) < Mathf.Epsilon) && (p1.y > p2.y)) return true;
            return false;
        }
    }

    public static class CustomMath {
        public static float Floor(float value, int digits) {
            float c = Mathf.Pow(10f, digits);
            return Mathf.Floor(value * c) / c;
        }
    }
}