using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AutoDestroy : MonoBehaviour
{
    [SerializeField] private float m_Duration = 3f;
    private float m_timer = 0f;

    void Update()
    {
        m_timer += Time.deltaTime;
        if (m_timer > m_Duration)
            GameObject.Destroy(gameObject);
    }
}
