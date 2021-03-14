using System;
using System.Collections.Generic;
using UnityEngine;

public enum SpawnPos
{
    Left,
    Right
}

[Serializable]
public class FragmentSpawnInfo
{
    public SpawnPos m_SpawnPos;
    public float m_ApparitionDelay = 0.2f;
    public float m_Speed = 4f;
}


[CreateAssetMenu(fileName = "FragmentsPattern", menuName = "Custom/FragmentsPattern", order = 2)]
public class FragmentsPattern : ScriptableObject
{
    public List<FragmentSpawnInfo> m_Fragments;
}
