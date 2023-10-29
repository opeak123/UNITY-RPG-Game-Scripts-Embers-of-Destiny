using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SlashCol : MonoBehaviour
{
    Rigidbody rig;

    void Start()
    {
        rig = GetComponent<Rigidbody>();
        rig.AddForce(transform.forward * 2000);
        Invoke("SpeedChange", 0.27f);
        Destroy(this.gameObject, 0.525f);
    }

    void SpeedChange()
    {
        rig.AddForce(transform.forward * (-1000));
    }
}
