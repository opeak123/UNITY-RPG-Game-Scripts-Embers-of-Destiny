using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveToTag : MonoBehaviour{

    GameObject m_movePos;
    public string m_tag;
    public float m_startDelay;
    public float m_durationTime;
    public float m_lerpValue;
    public float m_lookValue;

    bool m_isRunning = false;
    float m_Time;

	void Start (){
        m_movePos = GameObject.FindGameObjectWithTag(m_tag);
        m_Time = Time.time;
	}
	
	void Update (){
		if(Time.time > m_Time + m_startDelay || m_isRunning){
            m_isRunning = true;
            if (Time.time < m_Time + m_durationTime){
                transform.position = Vector3.Lerp(transform.position, m_movePos.transform.position,Time.deltaTime * m_lerpValue);
                if (Vector3.Distance(transform.position, m_movePos.transform.position) > 1){ 
                    Quaternion lookPos = Quaternion.LookRotation(transform.position - m_movePos.transform.position);
                    transform.rotation = Quaternion.Slerp(transform.rotation, (lookPos), Time.deltaTime * m_lookValue);
                }
            }
        }
	}
}
