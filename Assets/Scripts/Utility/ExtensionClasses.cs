using System.Collections;
using System.Collections.Generic;
using System.Text;
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

    public static class GameObjectExtensions {
        private static List<Component> _componentCache = new List<Component>();
        public static T GetComponentNoAlloc<T>(this GameObject obj) where T : Component {
            obj.GetComponents(typeof(T), _componentCache);
            var component = _componentCache.Count > 0 ? _componentCache[0] : null;
            _componentCache.Clear();
            return component as T;
        }
    }

    public static class CustomMath {
        public static float Floor(float value, int digits) {
            float c = Mathf.Pow(10f, digits);
            return Mathf.Floor(value * c) / c;
        }
    }

    public static class GridUtility {
        public static Vector3 GetWorldPosition(int r, int c, float cellSize, Vector3 originPosition) {
            return new Vector3(c, r) * cellSize + originPosition;
        }
        public static void GetXY(Vector3 worldPosition, out int r, out int c, float cellSize, Vector3 originPosition) {
            r = Mathf.FloorToInt((worldPosition - originPosition).y / cellSize);
            c = Mathf.FloorToInt((worldPosition - originPosition).x / cellSize);
        }
    }

    public static class Bezier {
        public static Vector3 GetPoint(Vector3 p0, Vector3 p1, Vector3 p2, float t)  {
            t = Mathf.Clamp01(t);
            float oneMinusT = 1f - t;
            return
                oneMinusT * oneMinusT * p0 +
                2f * oneMinusT * t * p1 +
                t * t * p2;
        }
    }

    public static class StringUtils {
        private static StringBuilder _stringBuilder = new StringBuilder(64);
        public static string MergeStrings(params string[] strList) {
            _stringBuilder.Clear();
            foreach (string str in strList) {
                _stringBuilder.Append(str);
            }
            return _stringBuilder.ToString();
        }
        public static string GetRandomString(params string[] stringList) {
            return stringList[Random.Range(0, stringList.Length)];
        }
    }
}