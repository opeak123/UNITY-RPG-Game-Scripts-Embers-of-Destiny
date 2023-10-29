using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TranslateMove : MonoBehaviour {

    public float m_power;
    public float m_reduceTime;
    public bool m_fowardMove;
    public bool m_rightMove;
    public bool m_upMove;
    float m_Time;

    void Start()
    {
        m_Time = Time.time;
    }

	void Update () {
        if(m_fowardMove)
            transform.Translate(transform.forward * m_power);
        if (m_rightMove)
            transform.Translate(transform.right * m_power);
        if (m_upMove)
            transform.Translate(transform.up * m_power);

        if (m_Time + m_reduceTime < Time.time && m_reduceTime != 0)
        {
            m_power -= Time.deltaTime/10;
            m_power = Mathf.Clamp01(m_power);
        }
    }
}
