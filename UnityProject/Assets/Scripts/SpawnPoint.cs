using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "SpawnPoint", menuName = "Custom/SpawnPoint", order = 2)]
public class SpawnPoint : ScriptableObject
{
    public Vector3 Position;
    public Vector3 Direction;
}
