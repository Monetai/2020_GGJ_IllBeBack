using UnityEngine;
using System.Collections.Generic;
using UnityEditor;

public sealed class EditorUtils
{
    public static List<T> FindAssetsOffType<T>() where T : UnityEngine.Object
    {
        List<T> assets = new List<T>();

        string[] guids = AssetDatabase.FindAssets("t:" + typeof(SceneSetupConfig).Name);

        for (int i = 0; i < guids.Length; i++)
        {
            string assetPath = AssetDatabase.GUIDToAssetPath(guids[i]);
            T asset = AssetDatabase.LoadAssetAtPath<T>(assetPath);

            if (asset != null)
            {
                assets.Add(asset);
            }
        }

        return assets;
    }
}
