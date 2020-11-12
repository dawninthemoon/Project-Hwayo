using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using Aroma;
using UnityEngine.U2D;

public class AssetManager : Singleton<AssetManager>
{
    private AssetBundle _objectBundle;

    private static readonly string AssetBundlePath = "/AssetBundles";
    private static readonly string AssetBundleNameBase = "assetbundle_";

    public void Initalize() {
        _objectBundle = AssetBundle.LoadFromFile(Path.Combine(Application.streamingAssetsPath + AssetBundlePath, AssetBundleNameBase + "object"));

        if (_objectBundle == null) {
            Debug.LogError("Failed to load AssetBundle");
            return;
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

    public T GetComponentInObjectBundle<T>(string name) where T : Object {
        if (_objectBundle == null) return null;
        T obj = _objectBundle.LoadAsset<T>(name);
        return obj;
    }
}
