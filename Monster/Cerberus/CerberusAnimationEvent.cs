using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CerberusAnimationEvent : MonoBehaviour
{
    [SerializeField]
    ParticleSystem[] fireBreathe;
    [SerializeField]
    ParticleSystem[] fireCharge;
    [SerializeField]
    SphereCollider[] bitePos;

    private void Awake()
    {
        for (int i = 0; i < fireCharge.Length; i++)
        {
            fireCharge[i] = GameObject.Find("FireCharge" + (i + 1)).GetComponent<ParticleSystem>();
        }
        for (int i = 0; i < fireBreathe.Length; i++)
        {
            fireBreathe[i] = GameObject.Find("FireSpray" + (i + 1)).GetComponent<ParticleSystem>();
        }
        for (int i = 0; i < bitePos.Length; i++)
        {
            bitePos[i] = fireBreathe[i].transform.parent.GetComponent<SphereCollider>();
        }
    }

    public void FireCharge()
    {
        for (int i = 0; i < fireCharge.Length; i++)
        {
            fireCharge[i].Play();
        }
    }

    public void ChargeEnd()
    {
        for (int i = 0; i < fireCharge.Length; i++)
        {
            fireCharge[i].Stop();
        }
    }

    public void FireBreathe()
    {
        for (int i = 0; i < fireBreathe.Length; i++)
        {
            fireBreathe[i].Play();
        }
        ChargeEnd();
    }

    public void BreatheEnd()
    {
        for (int i = 0; i < fireBreathe.Length; i++)
        {
            fireBreathe[i].Stop();
        }
    }

    public void Bite()
    {
        for (int i = 0; i < bitePos.Length; i++)
        {
            bitePos[i].enabled = true;
        }
    }

    public void BiteEnd()
    {
        for (int i = 0; i < bitePos.Length; i++)
        {
            bitePos[i].enabled = false;
        }
    }

    public void StopAllAnimations()
    {
        BiteEnd();
        BreatheEnd();
        ChargeEnd();
    }
}
