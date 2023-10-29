using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MultiObjectMake : _ObjectMakeBase {
    public float m_startDelay;
    public int m_makeCount;
    public float m_makeDelay;
    public Vector3 m_randomPos;
    public Vector3 m_randomRot;
    float m_Time;
    float m_Time2;
    float m_delayTime;
    float m_count;

	void Start () {
        m_Time = m_Time2 = Time.time;
    }
	
	void Update () {
        if (Time.time >= m_Time + m_startDelay)
        {
            if (Time.time > m_Time2 + m_makeDelay && m_count < m_makeCount)
            {
                Vector3 m_pos = transform.position + GetRandomVector(m_randomPos);
                Quaternion m_rot = transform.rotation * Quaternion.Euler(GetRandomVector(m_randomRot));

                for (int i = 0; i < m_makeObjs.Length; i++)
                {
                    GameObject m_obj = Instantiate(m_makeObjs[i], m_pos, m_rot);
                    m_obj.transform.parent = this.transform;

                    if (m_movePos)
                    {
                        if (m_obj.GetComponent<MoveToObject>())
                        {
                            MoveToObject m_script = m_obj.GetComponent<MoveToObject>();
                            m_script.m_movePos = m_movePos;
                        }
                    }
                }

                m_Time2 = Time.time;
                m_count++;
            }
        }

    }
}
