using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChainLighting : _ObjectMakeBase
{
    public GameObject targetObject;
    public float m_startDelay;
    public int m_makeCount;
    public float m_makeDelay;
    public Vector3 m_randomPos;
    public Vector3 m_randomRot;
    float m_Time;
    float m_Time2;
    float m_delayTime;
    float m_count;

    private bool checkMonster = false;

    List<GameObject> Targetlist = new List<GameObject>();
    GameObject PreTartget = null;
    void Start()
    {
        m_Time = m_Time2 = Time.time;
        GameObject[] objectsWithTag1 = GameObject.FindGameObjectsWithTag("Enemy");
        GameObject[] objectsWithTag2 = GameObject.FindGameObjectsWithTag("WOLF");
        foreach (GameObject obj in objectsWithTag1)
        {
            Targetlist.Add(obj);
        }
        foreach (GameObject obj in objectsWithTag2)
        {
            Targetlist.Add(obj);
        }
    }

    void Update()
    {
        Vector3 center = transform.position;
        float radius = 10.0f;
        CheckObjectsInRadius(center, radius);
        if (Time.time >= m_Time + m_startDelay)
        {
            if (Time.time > m_Time2 + m_makeDelay && m_count < m_makeCount)
            {
                FindClosestObjectWithTag();
                if(targetObject != null)
                    transform.position = targetObject.transform.position;
                if (targetObject.transform.CompareTag("Untagged"))
                    return;
                PreTartget = targetObject;
                //Targetlist.Remove(targetObject);

                Vector3 m_pos = transform.position + GetRandomVector(m_randomPos);
                Quaternion m_rot = transform.rotation * Quaternion.Euler(GetRandomVector(m_randomRot));
                AudioManager.Instance.PlaySFX(12, 1f); //chainlightningSound
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
                if (m_count == m_makeCount)
                    Targetlist.Remove(gameObject);
                
                    
            }
        }

    }

    bool CheckObjectsInRadius(Vector3 center, float radius)
    {
        int countmonster = 0;
        Collider[] colliders = Physics.OverlapSphere(center, radius);
        for (int i = 0; i < colliders.Length; i++)
        {
            if (colliders[i].CompareTag("WOLF") || colliders[i].CompareTag("Enemy"))
                countmonster++;
        }
        if (countmonster > 0)
            checkMonster = true;
        else if (countmonster <= 0)
        {
            Targetlist.Remove(gameObject);
            Destroy(gameObject);
        }
        return checkMonster;
    }

    private void FindClosestObjectWithTag()
    {
        float closestDistance = 10.0f;

        foreach (GameObject obj in Targetlist)
        {
            if (PreTartget == obj || obj == null || obj.transform.CompareTag("Untagged"))
                continue;

            float distance = Vector3.Distance(transform.position, obj.transform.position);
            if (distance < closestDistance)
            {
                closestDistance = distance;
                targetObject = obj;
            }
        }
    }
}
