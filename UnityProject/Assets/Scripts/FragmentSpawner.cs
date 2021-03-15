using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

[Serializable]
public class FragmentsPatternSpawn
{
    public FragmentsPattern Pattern;
    public float Probability = 1;
}

public class FragmentSpawner : MonoBehaviour
{
    public static FragmentSpawner Instance;

    [SerializeField] private List<FragmentsPatternSpawn> m_Patterns;
    private List<FragmentSpawnInfo> m_FragmentsSpawnInfos;
    private FragmentSpawnInfo m_NextSpawn;

    [SerializeField] private FragmentList m_ActiveFragments;

    private float m_Timer = 0;
    public bool canSpawn = false;
    private void Awake()
    {
        Instance = this;
    }

    private FragmentsPattern GetRandomPattern()
    {
        float sum = 0;
        foreach(FragmentsPatternSpawn pattern in m_Patterns)
        {
            sum += pattern.Probability;
        }
        float rand = Random.Range(0, sum);

        sum = 0;
        foreach (FragmentsPatternSpawn pattern in m_Patterns)
        {
            sum += pattern.Probability;
            if (rand < sum)
                return pattern.Pattern;
        }
        return null;
    }

    private void GeneratePatternList()
    {
        m_FragmentsSpawnInfos = new List<FragmentSpawnInfo>();

        int availableFragmentsCount = m_ActiveFragments.CurrentItems.Count;
        while (availableFragmentsCount > 0)
        {
            FragmentsPattern pattern = GetRandomPattern();

            Fragment[] availableFragments = m_ActiveFragments.CurrentItems.ToArray();
            if (availableFragmentsCount < pattern.m_Fragments.Count)
            {
                continue;
            }
            else
            {
                for (int i = 0; i < pattern.m_Fragments.Count; ++i)
                {
                    m_FragmentsSpawnInfos.Add(pattern.m_Fragments[i]);
                    availableFragmentsCount--;
                }
            }
        }
    }

    private void Start()
    {
        GeneratePatternList();
        m_NextSpawn = m_FragmentsSpawnInfos[0];
    }


    void Update()
    {
        if (!canSpawn)
            return; 

        m_Timer += Time.deltaTime;
        if (m_Timer >= m_NextSpawn.m_ApparitionDelay)
        {
            m_Timer = 0f;
            Spawn();
        }
    }

    private void Spawn()
    {
        Fragment fragment = m_ActiveFragments.GetFragmentToSpawn();
        if (fragment != null && !fragment.IsPaused())
        {
            Vector3 spawnPoint;
            if (m_NextSpawn.m_SpawnPos == SpawnPos.Left)
            {
                spawnPoint = Vector3.right * -10;
            }
            else
            {
                spawnPoint = Vector3.right * 10;
            }

            fragment.BeginAttack(spawnPoint, m_NextSpawn.m_SpawnPos, m_NextSpawn.m_Speed);

            m_FragmentsSpawnInfos.RemoveAt(0);
            if (m_FragmentsSpawnInfos.Count > 0)
            {
                m_NextSpawn = m_FragmentsSpawnInfos[0];
            }
            else
            {
                enabled = false;
                Debug.Log("No more fragment to spawn");
                Invoke("EndGame", 10f);
            }
        }
    }

    void EndGame()
    {
        menuontroller.Instance.EndCinematic();
    }
}
