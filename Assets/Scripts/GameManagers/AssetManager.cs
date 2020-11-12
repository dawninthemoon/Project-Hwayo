using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using Aroma;
using UnityEngine.U2D;

public class AssetManager : Singleton<AssetManager> {
    AssetBundle _objectBundle;
    AssetBundle _effectBundle;

    private static readonly string AssetBundlePath = "/AssetBundles";
    private static readonly string AssetBundleNameBase = "assetbundle_";

    public void Initalize() {
        _objectBundle = AssetBundle.LoadFromFile(Path.Combine(Application.streamingAssetsPath + AssetBundlePath, AssetBundleNameBase + "object"));
        _effectBundle = AssetBundle.LoadFromFile(Path.Combine(Application.streamingAssetsPath + AssetBundlePath, AssetBundleNameBase + "effect"));

        if (_objectBundle == null) {
            Debug.LogError("Failed to load object AssetBundle");
        }
        if (_effectBundle == null) {
            Debug.LogError("Failed to load effect AssetBundle");
        }
    }

    public T[] GetComponentsInObjectBundle<T>() where T : Component {
        if (_objectBundle == null) return null;
        GameObject[] prefabs = _objectBundle.LoadAllAssets<GameObject>();
        List<T> components = new List<T>();

        for (int i = 0; i < prefabs.Length; i++) {
            var component = prefabs[i].GetComponentNoAlloc<T>();
            if (component != null) {
                components.Add(component);
            }
        }
        T[] wholeObjects = _objectBundle.LoadAllAssets<T>();
        return wholeObjects;
    }

    public T GetAssetInObjectBundle<T>(string name) where T : Object {
        T obj = _objectBundle.LoadAsset<T>(name);
        return obj;
    }

    public T[] GetAssetsInEffectBundle<T>() where T : Object {
        T[] assets = _effectBundle.LoadAllAssets<T>();
        return assets;
    }
}
