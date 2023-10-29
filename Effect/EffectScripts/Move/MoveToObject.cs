using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveToObject : MonoBehaviour{

    public Transform m_movePos;
    public float m_startDelay;
    public float m_durationTime;
    public float m_lerpValue;
    public float m_lookValue;

    float m_Time;

    void Start (){
        m_Time = Time.time;
    }
	
	void Update (){
		if(Time.time > m_Time + m_startDelay)
        {
            if (Time.time < m_Time + m_durationTime+m_startDelay)
            {
                transform.position = Vector3.Lerp(transform.position, m_movePos.position,Time.deltaTime * m_lerpValue);
                if (Vector3.Distance(transform.position,m_movePos.position) > 1)
                { 
                    Quaternion lookPos = Quaternion.LookRotation(transform.position - m_movePos.position);
                    transform.rotation = Quaternion.Slerp(transform.rotation, (lookPos), Time.deltaTime * m_lookValue);
                }
            }
        }
	}
}
