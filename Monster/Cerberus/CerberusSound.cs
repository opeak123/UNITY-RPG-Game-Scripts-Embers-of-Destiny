using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CerberusSound : MonoBehaviour
{
    public AudioClip[] cerberusSounds;
    public AudioClip[] bgm;
    public AudioSource[] source;
    public Animator anim;

    public void Bark1()
    {
        source[0].clip = cerberusSounds[0];
        source[0].Play();
    }
    public void Bark2()
    {
        source[0].clip = cerberusSounds[1];
        source[0].Play();
    }
    public void Bark3()
    {
        source[0].clip = cerberusSounds[2];
        source[0].Play();
    }
    public void Bark4()
    {
        source[0].clip = cerberusSounds[3];
        source[0].Play();
    }
    public void FootStep()
    {
        source[1].clip = cerberusSounds[4];
        source[1].Play();
    }
    public void JumpAttack()
    {
        source[0].clip = cerberusSounds[5];
        source[0].Play();
    }
    public void BiteSound()
    {
        source[1].clip = cerberusSounds[6];
        source[1].Play();
    }
    public void FireBreatheSound()
    {
        source[1].clip = cerberusSounds[7];
        source[1].Play();
    }
    public void BGM()
    {
        source[3].clip = bgm[0];
        source[3].Play();
    }
    public void Damage()
    {
        source[0].clip = cerberusSounds[8];
        source[0].Play();
    }
    public void DeadSound()
    {
        anim.speed = 0.5f;
        source[0].clip = cerberusSounds[9];
        source[0].Play();
    }

}
