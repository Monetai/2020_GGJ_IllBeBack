using UnityEngine;
using System.Collections.Generic;

/// <summary>
/// Spawn all needed singleton on the game beginning, must be while the splashscreen is running
/// </summary>

public sealed class SceneSetupSpawner : MonoBehaviour
{
    public SceneSetupConfig sceneConfig;

    [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.BeforeSceneLoad)]
    static void InstantiatePrefabs()
    {
        if (Application.isEditor)
            return;

        SceneSetupSpawner spawner = FindObjectOfType<SceneSetupSpawner>();

        if (spawner != null)
            spawner.sceneConfig.InstantiatePrefabs();
    }
}
