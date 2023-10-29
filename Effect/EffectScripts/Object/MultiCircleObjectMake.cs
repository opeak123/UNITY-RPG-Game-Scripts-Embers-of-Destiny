using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MultiCircleObjectMake : _ObjectMakeBase {

    public float m_interval;
    public int m_makeCount;
    public float m_makeDelay;
    public float m_startDelay;
    float m_Time;
    float m_Time2;
    float m_count;

    void Start()
    {
        m_Time2 = Time.time;
    }

	void Update ()
    {
        m_Time += Time.deltaTime;
        if (Time.time < m_Time2 + m_startDelay)
            return;
		
        if(m_Time > m_makeDelay && m_count < m_makeCount)
        {
            float Angle = 2.0f * Mathf.PI / m_makeCount * m_count;
            float pos_X = Mathf.Cos(Angle) * m_interval;
            float pos_Z = Mathf.Sin(Angle) * m_interval;

            m_Time = 0.0f;
            for(int i = 0; i < m_makeObjs.Length;i++)
            { 
                GameObject m_obj = Instantiate(m_makeObjs[i], transform.position + new Vector3(pos_X,0,pos_Z), Quaternion.LookRotation(new Vector3(pos_X,0,pos_Z)) * m_makeObjs[i].transform.rotation);
                m_obj.transform.parent =this.transform;

                if (m_movePos)
                {
                    if (m_obj.GetComponent<MoveToObject>())
                    {
                        MoveToObject m_script = m_obj.GetComponent<MoveToObject>();
                        m_script.m_movePos = m_movePos;
                    }
                }

            }
        m_count++;
        }
    }
}
