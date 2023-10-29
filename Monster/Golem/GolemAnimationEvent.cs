using UnityEngine;

public class GolemAnimationEvent : MonoBehaviour
{
    //footStep
    public ParticleSystem leftFootParticle;
    public ParticleSystem rightFootParticle;
    public ParticleSystem leftBlastParticle;
    public ParticleSystem rightBlastParticle;

    //attack
    public ParticleSystem leftAttackParticle;
    public ParticleSystem rightAttackParticle;
    public ParticleSystem straightParticle;
    public ParticleSystem bothAttackParticle;
    public AudioSource[] audioSources = new AudioSource[2];
    private MonsterAudioClip audioClip;

    private void Start()
    {
        audioClip = GetComponent<MonsterAudioClip>();
    }
    public void FootStepParticle(int value)
    {

        if(value == 0)
        {
            audioSources[0].clip = audioClip.clip[0];
            audioSources[0].Play();
            rightFootParticle.Play();
            rightBlastParticle.Play();
        }
        else
        {
            audioSources[0].clip = audioClip.clip[0];
            audioSources[0].Play();
            leftFootParticle.Play();
            leftBlastParticle.Play();
        }
    }

    public void AttackEventParticle(int value)
    {
        if (value == 0)
        {
            audioSources[0].clip = audioClip.clip[1];
            audioSources[0].Play();
            audioSources[1].clip = audioClip.clip[3];
            audioSources[1].Play();
            leftAttackParticle.Play();
        }
        else if(value == 1)
        {
            audioSources[0].clip = audioClip.clip[1];
            audioSources[0].Play();
            audioSources[1].clip = audioClip.clip[3];
            audioSources[1].Play();
            rightAttackParticle.Play();
        }
        else//2
        {
            audioSources[1].clip = audioClip.clip[3];
            audioSources[1].Play();
            straightParticle.Play();
        }
    }

    public void Attack()
    {
        audioSources[0].clip = audioClip.clip[2];
        audioSources[0].Play();
    }

    public void AttackBothHand()
    {
        audioSources[1].clip = audioClip.clip[3];
        audioSources[1].Play();
    }
    public void AttackBothHandParticle()
    {
        bothAttackParticle.Play();
    }
}

