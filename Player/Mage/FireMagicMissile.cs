using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FireMagicMissile : MonoBehaviour
{
    Rigidbody rig;
    AudioSource energyarrowSound;

    void Start()
    {
        rig = GetComponent<Rigidbody>();
    }

    private void FixedUpdate()
    {
        rig.AddForce(transform.forward * 50);
    }
}
