﻿#if UNITY_EDITOR
using UnityEditor;
#endif
using System.IO; 
using UnityEngine;

public class BuildAsssetBundles { 
    [MenuItem("Bundles/Build All AssetBundles")] 
    public static void BuildAllAssetBundles() { 
        string assetBundleDirectory = Application.streamingAssetsPath + "/AssetBundles";
        if (!Directory.Exists(assetBundleDirectory))
        {
            Directory.CreateDirectory(assetBundleDirectory);
        }
        BuildPipeline.BuildAssetBundles(assetBundleDirectory, BuildAssetBundleOptions.None, BuildTarget.StandaloneWindows);
    }

    [MenuItem("Bundles/Build Object AssetBundles")] 
    public static void BuildObjectAssetBundles() { 
        BuildNeedAssetBundle("assetbundle_object");
    }

    [MenuItem("Bundles/Build Effect AssetBundles")] 
    public static void BuildEffectAssetBundles() { 
        BuildNeedAssetBundle("assetbundle_effect");
    }

    private static void BuildNeedAssetBundle(string bundleName) {
        if (!Directory.Exists(Application.streamingAssetsPath + "/AssetBundles")) {
            Directory.CreateDirectory(Application.streamingAssetsPath + "/AssetBundles");
        }

        AssetBundleBuild[] buildBundles = new AssetBundleBuild[1];

        buildBundles[0].assetBundleName = bundleName;
        buildBundles[0].assetNames = AssetDatabase.GetAssetPathsFromAssetBundle(bundleName);

        BuildPipeline.BuildAssetBundles(Application.streamingAssetsPath + "/AssetBundles", buildBundles, BuildAssetBundleOptions.None, BuildTarget.StandaloneWindows);
    }
}