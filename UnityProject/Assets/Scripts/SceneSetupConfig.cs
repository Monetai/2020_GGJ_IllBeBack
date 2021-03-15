using UnityEngine;
using System.Collections.Generic;

[CreateAssetMenu(fileName = "SceneSetup", menuName = "Custom/SceneSetup", order = 1)]
public sealed class SceneSetupConfig : ScriptableObject
{
    public List<GameObject> prefabsToInstantiate;

    public void InstantiatePrefabs()
    {
        foreach (GameObject obj in prefabsToInstantiate)
        {
            Instantiate(obj);
        }
    }
}
