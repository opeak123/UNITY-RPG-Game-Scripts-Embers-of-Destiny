using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Material_Change : MonoBehaviour
{
    public Material m_inputMaterial;
    Material m_objectMaterial;
    MeshRenderer m_meshRenderer;
    public float m_timeToReduce;
    public float m_reduceFactor =1.0f;
    float m_time;
    float m_submitReduceFactor;
    float m_cutOutFactor;

    void Awake()
    {
        m_meshRenderer = gameObject.GetComponent<MeshRenderer>();
        m_meshRenderer.material = m_inputMaterial;
        m_objectMaterial = m_meshRenderer.material;
        m_submitReduceFactor = 0.0f;
        m_cutOutFactor = 0.0f;
    }

	void LateUpdate()
    {
        m_time += Time.deltaTime;
        if (m_time > m_timeToReduce)
        {
            m_cutOutFactor += m_submitReduceFactor;
            m_submitReduceFactor = Mathf.Lerp(m_submitReduceFactor, m_reduceFactor, Time.deltaTime / 50);
        }

        m_cutOutFactor = Mathf.Clamp01(m_cutOutFactor);
        if (m_cutOutFactor >= 1 && m_time > m_timeToReduce)
            Destroy(gameObject);
        m_objectMaterial.SetFloat("_CutOut", m_cutOutFactor);
	}

}
