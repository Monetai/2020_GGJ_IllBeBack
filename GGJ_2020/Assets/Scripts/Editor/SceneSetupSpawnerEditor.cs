using UnityEngine;
using UnityEditor;
using System.Collections;
using System.Collections.Generic;
using System;

/// <summary>
/// Spawn all needed singleton on the active scene when entering play mode in editor
/// </summary>
[InitializeOnLoad]
public sealed class SceneSetupSpawnerEditor
{
    [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.BeforeSceneLoad)]
    static void InstantiatePrefabs()
    {
        List<SceneSetupConfig> list = EditorUtils.FindAssetsOffType<SceneSetupConfig>();

        if (list != null && list.Count > 0)
        {
            //use the firt SceneSetupConfig found
            list[0].InstantiatePrefabs();
        }
        else
        {
            Debug.LogError("No SceneSetupConfig found");
        }
    }
}
