using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Delete : MonoBehaviour
{
    public float deleteTime;

    void Start()
    {
        Destroy(gameObject,deleteTime);
    }
}
