using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectRotation : MonoBehaviour
{
    public float m_minXRotation = 0f;
    public float m_maxXRotation = 45f;
    public float m_minYRotation = 0f;
    public float m_maxYRotation = 45f;
    public float m_minZRotation = 0f;
    public float m_maxZRotation = 45f;

    private float m_XRotation, m_YRotation, m_ZRotation;

    void Start()
    {
        m_XRotation = Random.Range(m_minXRotation, m_maxXRotation);
        m_YRotation = Random.Range(m_minYRotation, m_maxYRotation);
        m_ZRotation = Random.Range(m_minZRotation, m_maxZRotation);
    }

    void Update()
    {
        Vector3 angles = transform.rotation.eulerAngles;

        angles = new Vector3(angles.x + m_XRotation * Mathf.Deg2Rad * Time.deltaTime, angles.y + m_YRotation * Mathf.Deg2Rad * Time.deltaTime, angles.z + m_ZRotation * Mathf.Deg2Rad * Time.deltaTime);
        transform.rotation = Quaternion.Euler(angles);
    }
}
