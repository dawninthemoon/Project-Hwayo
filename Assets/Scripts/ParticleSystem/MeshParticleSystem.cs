using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MeshParticleSystem : MonoBehaviour
{
    private const int MAX_QUAD_AMOUNT = 15000;

    [System.Serializable]
    public struct ParticleUVPixels {
        public Vector2Int uv00Pixels;
        public Vector2Int uv11Pixels;
    }

    private struct UVCoords {
        public Vector2 uv00;
        public Vector2 uv11;
    }

    UVCoords[] _uvCoordsArray;

    Mesh _mesh;
    Vector3[] _vertices;
    Vector2[] _uv;
    int[] _triangles;
    int _quadIndex;

    bool _updateVertices;
    bool _updateUV;
    bool _updateTriangles;

    public void Initalize(Material material, float quadSize) {
        _mesh = new Mesh();

        _vertices = new Vector3[4 * MAX_QUAD_AMOUNT];
        _uv = new Vector2[4 * MAX_QUAD_AMOUNT];
        _triangles = new int[6 * MAX_QUAD_AMOUNT];

        _mesh.vertices = _vertices;
        _mesh.uv = _uv;
        _mesh.triangles = _triangles;
        _mesh.bounds = new Bounds(Vector3.zero, Vector3.one * 10000f);

        gameObject.AddComponent<MeshFilter>().mesh = _mesh;

        gameObject.AddComponent<MeshRenderer>().material = material;
        Texture mainTexture = material.mainTexture;
        int textureWidth = mainTexture.width;
        int textureHeight = mainTexture.height;

        int wCount = Mathf.FloorToInt(textureWidth / quadSize);
        int hCount = Mathf.FloorToInt(textureHeight / quadSize);

        List<UVCoords> _uvCoordsList = new List<UVCoords>();
        for (int i = 0; i < hCount; ++i) {
            for (int j = 0; j < wCount; ++j) {
                int k = hCount - i - 1;
                UVCoords uvCoords = new UVCoords {
                    uv00 = new Vector2(quadSize * j / textureWidth, quadSize * k / textureHeight),
                    uv11 = new Vector2(quadSize * (j + 1) / textureWidth, quadSize * (k + 1) / textureHeight)
                };
                _uvCoordsList.Add(uvCoords);
            }
        }
        _uvCoordsArray = _uvCoordsList.ToArray();
    }

    public int GetMaxIndex() {
        return _uvCoordsArray.Length;
    }

    public int AddQuad(Vector3 position, float rotation, Vector3 quadSize, bool skewed, int uvIndex) {
        if (_quadIndex >= MAX_QUAD_AMOUNT) return 0;

        UpdateQuad(_quadIndex, position, rotation, quadSize, skewed, uvIndex);

        int spawnedQuadIndex = _quadIndex;
        _quadIndex++;

        return spawnedQuadIndex;
    }

    public void UpdateQuad(int quadIndex, Vector3 position, float rotation, Vector3 quadSize, bool skewed, int uvIndex) {
        int vIndex = quadIndex * 4;
        int vIndex0 = vIndex;
        int vIndex1 = vIndex + 1;
        int vIndex2 = vIndex + 2;
        int vIndex3 = vIndex + 3;

        if (skewed) {
            _vertices[vIndex0] = position + Quaternion.Euler(0, 0, rotation) * new Vector3(-quadSize.x, -quadSize.y);
            _vertices[vIndex1] = position + Quaternion.Euler(0, 0, rotation) * new Vector3(-quadSize.x, +quadSize.y);
            _vertices[vIndex2] = position + Quaternion.Euler(0, 0, rotation) * new Vector3(quadSize.x, +quadSize.y);
            _vertices[vIndex3] = position + Quaternion.Euler(0, 0, rotation) * new Vector3(quadSize.x, -quadSize.y);
        }
        else {
            _vertices[vIndex0] = position + Quaternion.Euler(0, 0, rotation - 180) * quadSize;
            _vertices[vIndex1] = position + Quaternion.Euler(0, 0, rotation - 270) * quadSize;
            _vertices[vIndex2] = position + Quaternion.Euler(0, 0, rotation - 0) * quadSize;
            _vertices[vIndex3] = position + Quaternion.Euler(0, 0, rotation - 90) * quadSize;
        }

        UVCoords uvCoords = _uvCoordsArray[uvIndex];
        _uv[vIndex0] = uvCoords.uv00;
        _uv[vIndex1] = new Vector2(uvCoords.uv00.x, uvCoords.uv11.y);
        _uv[vIndex2] = uvCoords.uv11;
        _uv[vIndex3] = new Vector2(uvCoords.uv11.x, uvCoords.uv00.y);

        int tIndex = quadIndex * 6;

        _triangles[tIndex + 0] = vIndex0;
        _triangles[tIndex + 1] = vIndex1;
        _triangles[tIndex + 2] = vIndex2;

        _triangles[tIndex + 3] = vIndex0;
        _triangles[tIndex + 4] = vIndex2;
        _triangles[tIndex + 5] = vIndex3;

        _updateVertices = true;
        _updateUV = true;
        _updateTriangles = true;
    }

    public void DestroyQuad(int quadIndex) {
        int vIndex = quadIndex * 4;
        int vIndex0 = vIndex;
        int vIndex1 = vIndex + 1;
        int vIndex2 = vIndex + 2;
        int vIndex3 = vIndex + 3;

        _vertices[vIndex0] = Vector3.zero;
        _vertices[vIndex1] = Vector3.zero;
        _vertices[vIndex2] = Vector3.zero;
        _vertices[vIndex3] = Vector3.zero;

        _updateVertices = true;
    }

    public void LateProgress() {
        if (_updateVertices || _updateUV || _updateTriangles)
            _mesh.RecalculateBounds();

        if (_updateVertices) {
            _mesh.vertices = _vertices;
            _updateVertices = false;
        }
        if (_updateUV) {
            _mesh.uv = _uv;
            _updateUV = false;
        }
        if (_updateTriangles) {
            _mesh.triangles = _triangles;
            _updateTriangles = false;
        }
    }
}
