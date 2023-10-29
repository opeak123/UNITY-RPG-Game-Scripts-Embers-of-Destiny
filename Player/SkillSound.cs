using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkillSound : MonoBehaviour
{
    AudioSource skillsound;
    public int skillsoundNum;

    void Start()
    {
        skillsound = GetComponent<AudioSource>();
        skillsound.clip = AudioManager.Instance.sfxClips[skillsoundNum];
        skillsound.Play();
    }
}
