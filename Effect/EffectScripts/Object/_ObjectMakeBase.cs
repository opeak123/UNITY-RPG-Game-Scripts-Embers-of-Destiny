using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class _ObjectMakeBase : MonoBehaviour {

    public GameObject[] m_makeObjs;
    public Transform m_movePos;

    public float GetRandomValue(float value){
        return Random.Range(-value,value);
    }

    public Vector3 GetRandomVector(Vector3 value){
        Vector3 result;
        result.x = GetRandomValue(value.x);
        result.y = GetRandomValue(value.y);
        result.z = GetRandomValue(value.z);
        return result;
    }

}
