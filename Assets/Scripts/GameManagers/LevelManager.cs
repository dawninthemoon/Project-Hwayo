using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using CustomTilemap;
using CustomPhysics;
using Cinemachine;

public class LevelManager : SingletonWithMonoBehaviour<LevelManager>, ISetupable {
    public string CurrentLevelName { get; private set; }
    public CustomGrid<TileObject> TileGrid { get; private set; }
    public Dictionary<string, LevelInfo> LevelDictionary { get; private set; }
    ObjectPool<TileObject> _tileObjectPool;
    TilemapVisual _tilemapVisual;
    public PolygonCollider2D _cameraClampCollider;
    private CinemachineConfiner _confiner;
    static string CurrentLevelNameKey = "Key_LevelManager_CurrentLevelNameKey";
    public void Initalize() {
        LevelDictionary = new Dictionary<string, LevelInfo>();
        var levelInfo = JsonHelper.LoadJsonFile<LevelInfo>(Application.dataPath, "HwayoGameLevel");
        
        string defaultLevelName = null;
        foreach (var level in levelInfo) {
            LevelDictionary.Add(level.levelName, level);
            defaultLevelName = level.levelName;
        }
        CurrentLevelName = ES3.KeyExists(CurrentLevelNameKey) ? ES3.Load<string>(CurrentLevelNameKey) : defaultLevelName;

        InitTileGrid();
        InitCameraClamp();
    }

    public void LoadLevel(string name) {
        LevelInfo level = LevelDictionary[name];
        string tilesetName = LevelDictionary[CurrentLevelName].tilesets[0].tilesetName;

        TileGrid.ResetGrid(level, level.width, level.height, level.originPosition, _tilemapVisual.UpdateHeatMapVisual);
        var material = AssetManager.GetInstance().GetAssetInObjectBundle<Material>(tilesetName);
        _tilemapVisual.ChangeMaterial(material);

        UpdateCameraClampBounds(level);
        UpdateTileColliders(level);

        CurrentLevelName = name;

        EventManager.GetInstance().OnRoomChanged(level.entities);
    }

    void InitTileGrid() {
        var curLevel = LevelDictionary[CurrentLevelName];
        string tilesetName = curLevel.tilesets[0].tilesetName;

        _tileObjectPool = new ObjectPool<TileObject>(100 * 100, () => new TileObject(), null, null);
        TileGrid = new CustomGrid<TileObject>(16, curLevel.width, curLevel.height, curLevel.originPosition, _tileObjectPool.GetObject, _tileObjectPool.ReturnObject);
        
        var assetManager = AssetManager.GetInstance();
        var tilemapVisual = assetManager.GetAssetInObjectBundle<GameObject>("TilemapVisual").GetComponent<TilemapVisual>();
        var material = assetManager.GetAssetInObjectBundle<Material>(tilesetName);
        
        _tilemapVisual = Instantiate(tilemapVisual);
        _tilemapVisual.Initalize(TileGrid, material);
        TileGrid.ResetGrid(curLevel, curLevel.width, curLevel.height, curLevel.originPosition, _tilemapVisual.UpdateHeatMapVisual);
    }
    void InitCameraClamp() {
        _cameraClampCollider = new GameObject("CameraClampCollider").AddComponent<PolygonCollider2D>();
        _confiner = GameObject.Find("CM vcam1").GetComponent<CinemachineConfiner>();
        _confiner.m_BoundingShape2D = _cameraClampCollider;
    }
    void UpdateCameraClampBounds(LevelInfo level) {
        Vector2 p00 = TileGrid.GetWorldPosition(0, 0);
        Vector2 p10 = TileGrid.GetWorldPosition(level.width, 0);
        Vector2 p11 = TileGrid.GetWorldPosition(level.width, level.height);
        Vector2 p01 = TileGrid.GetWorldPosition(0, level.height);

        _cameraClampCollider.SetPath(0, new Vector2[] { p00, p10, p11, p01 });
    }
    void UpdateTileColliders(LevelInfo level) {
        CollisionManager.GetInstance().ClearTileColliders();
        foreach (var collider in level.colliders) {
            CollisionManager.GetInstance().AddTileCollider(collider.points);
        }
    }
}
