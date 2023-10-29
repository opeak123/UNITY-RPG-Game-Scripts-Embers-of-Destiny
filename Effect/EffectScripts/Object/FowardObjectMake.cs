using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FowardObjectMake : _ObjectMakeBase {

    public float m_objectSize;
    public float m_subtractYValue;
    public float m_makeCount;
    public float m_makeDelay;
    public bool m_isCrossMake;
    float m_Time;
    float m_count;

    void Update()
    {
        m_Time += Time.deltaTime;
        Vector3 addedPos = new Vector3(0,0,0);
        int crossMake = 0;

        if (m_Time > m_makeDelay && m_count < m_makeCount)
        {
            if (m_isCrossMake)
            {
                if (m_count % 2 == 0)
                    crossMake = 1;
                else
                    crossMake = -1;
            }

            addedPos = transform.forward * m_objectSize * m_count;
            Vector3 pos = transform.position - new Vector3(0, m_subtractYValue, 0) + addedPos + (transform.right  * crossMake);
            Quaternion rot = transform.rotation * Quaternion.Euler(-90, 0, 0);

            for(int i = 0; i <m_makeObjs.Length; i++)
            { 
                GameObject m_obj = Instantiate(m_makeObjs[i], pos, rot);
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
            m_Time = 0;
            m_count++;
        }
    }
}
