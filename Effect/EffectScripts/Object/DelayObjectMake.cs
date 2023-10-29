using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DelayObjectMake : _ObjectMakeBase{

    public float m_startDelay;
    float m_Time;
    bool isMade = false;

    void Start(){
        m_Time = Time.time;
        Destroy(this.gameObject, 7.0f);
    }

    void Update(){
        if(Time.time > m_Time + m_startDelay && !isMade){
            isMade = true;
            for(int i = 0; i < m_makeObjs.Length; i++)
            { 
                GameObject m_obj = Instantiate(m_makeObjs[i], transform.position, transform.rotation);
                m_obj.transform.parent = this.transform;

                if (m_movePos){
                    if (m_obj.GetComponent<MoveToObject>()) {
                        MoveToObject m_script = m_obj.GetComponent<MoveToObject>();
                        m_script.m_movePos = m_movePos;
                    }
                }
            }
        }
    }
}
