using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class ReaperAnimationEvent : MonoBehaviour
{
    private ParticleSystem blastParticle;
    private ParticleSystem slashParticle;
    public ParticleSystem poisonParticle;
    //public BoxCollider hitCollider;
    //private ParticleSystem missileParticle;
    public AudioSource audioSource;
    private MonsterAudioClip audioClip;

    private void Start()
    {
        audioClip = GetComponent<MonsterAudioClip>();
        blastParticle = GameObject.Find("ShadowSphereBlast").GetComponent<ParticleSystem>();
        slashParticle = GameObject.Find("ShadowSlash").GetComponent<ParticleSystem>();
       // missileParticle = GameObject.Find("ShadowMissile").GetComponent<ParticleSystem>();
    }

    public void Blast()
    {
        blastParticle.Play();
    }
    public void Slash()
    {
        slashParticle.Play();
    }

    public void Missile()
    {
        //missileParticle.Play();
    }

    //public void OnCollider()
    //{
    //    hitCollider.enabled = true;
    //}
    //public void OffCollider()
    //{
    //    hitCollider.enabled = false;
    //}


    public void Dead()
    {
        poisonParticle.gameObject.SetActive(false);
    }

    public void HitGroundSound()
    {
        if(!audioSource.isPlaying)
        {
            audioSource.clip = audioClip.clip[2];
            audioSource.Play();
        }
    }

    public void AttackOne()
    {
        audioSource.clip = audioClip.clip[3];
        audioSource.Play();
    }
    public void AttackTwo()
    {
        audioSource.clip = audioClip.clip[4];
        audioSource.Play();
    }
    public void AttackThree()
    {
        audioSource.clip = audioClip.clip[5];
        audioSource.Play();
    }
}
