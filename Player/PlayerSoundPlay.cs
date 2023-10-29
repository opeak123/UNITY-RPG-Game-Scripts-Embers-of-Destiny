using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerSoundPlay : MonoBehaviour
{
    public void WhirlWindSound()
    {
        AudioManager.Instance.sfxSources[2].clip = AudioManager.Instance.sfxClips[0];
        AudioManager.Instance.sfxSources[2].Play();
    }
    
    public void WhirlWindPowerSlash()
    {
        AudioManager.Instance.sfxSources[2].clip = AudioManager.Instance.sfxClips[2];
        AudioManager.Instance.sfxSources[2].Play();
    }
    public void PlayerShortSound_Male()
    {
        AudioManager.Instance.sfxSources[0].clip = AudioManager.Instance.sfxClips[3];
        AudioManager.Instance.sfxSources[0].Play();
    }
    public void PlayerBigSound1_Male()
    {
        AudioManager.Instance.sfxSources[0].clip = AudioManager.Instance.sfxClips[4];
        AudioManager.Instance.sfxSources[0].Play();
    }
    public void PlayerBigSound_Male()
    {
        AudioManager.Instance.sfxSources[0].clip = AudioManager.Instance.sfxClips[5];
        AudioManager.Instance.sfxSources[0].Play();
    }
    public void WarriorDodge()
    {
        AudioManager.Instance.sfxSources[0].clip = AudioManager.Instance.sfxClips[6];
        AudioManager.Instance.sfxSources[0].Play();
    }
    public void MaleDie()
    {
        AudioManager.Instance.sfxSources[0].clip = AudioManager.Instance.sfxClips[7];
        AudioManager.Instance.sfxSources[0].Play();
    }
    public void SkillCoolTime()
    {
        AudioManager.Instance.sfxSources[2].clip = AudioManager.Instance.sfxClips[8];
        AudioManager.Instance.sfxSources[2].Play();
    }
    public void StepSound()
    {
        AudioManager.Instance.sfxSources[0].clip = AudioManager.Instance.sfxClips[9];
        AudioManager.Instance.sfxSources[0].Play();
    }
    public void LightSwordSound()
    {
        AudioManager.Instance.sfxSources[2].clip = AudioManager.Instance.sfxClips[10];
        AudioManager.Instance.sfxSources[2].Play();
    }
    public void SwordJudgementExplode()
    {
        AudioManager.Instance.sfxSources[2].clip = AudioManager.Instance.sfxClips[11];
        AudioManager.Instance.sfxSources[2].Play();
    }
    public void ShootMagicMissile()
    {
        AudioManager.Instance.sfxSources[2].clip = AudioManager.Instance.sfxClips[13];
        AudioManager.Instance.sfxSources[2].Play();
    }
    public void Teleport()
    {
        AudioManager.Instance.sfxSources[2].clip = AudioManager.Instance.sfxClips[15];
        AudioManager.Instance.sfxSources[2].Play();
    }
    public void BowSound()
    {
        AudioManager.Instance.sfxSources[0].clip = AudioManager.Instance.sfxClips[16];
        AudioManager.Instance.sfxSources[0].Play();
    }
    public void ShootArrow()
    {
        AudioManager.Instance.sfxSources[1].clip = AudioManager.Instance.sfxClips[17];
        AudioManager.Instance.sfxSources[1].Play();
    }
    public void ShootEnergyArrowSound()
    {
        AudioManager.Instance.sfxSources[2].clip = AudioManager.Instance.sfxClips[18];
        AudioManager.Instance.sfxSources[2].Play();
    }
    public void playerJumpSound_Female()
    {
        AudioManager.Instance.sfxSources[0].clip = AudioManager.Instance.sfxClips[19];
        AudioManager.Instance.sfxSources[0].Play();
    }
    public void playersound_female_attack()
    {
        AudioManager.Instance.sfxSources[0].clip = AudioManager.Instance.sfxClips[20];
        AudioManager.Instance.sfxSources[0].Play();
    }
    public void playersound_female_dodge()
    {
        AudioManager.Instance.sfxSources[0].clip = AudioManager.Instance.sfxClips[33];
        AudioManager.Instance.sfxSources[0].Play();
    }
    public void playercound_female_die()
    {
        AudioManager.Instance.sfxSources[0].clip = AudioManager.Instance.sfxClips[34];
        AudioManager.Instance.sfxSources[0].Play();
    }
    public void ArcherMultiShot()
    {
        AudioManager.Instance.sfxSources[2].clip = AudioManager.Instance.sfxClips[35];
        AudioManager.Instance.sfxSources[2].Play();
    }
    public void ArrowVolleyFire()
    {
        AudioManager.Instance.sfxSources[2].clip = AudioManager.Instance.sfxClips[36];
        AudioManager.Instance.sfxSources[2].Play();
    }
    public void ArrowOneShot()
    {
        AudioManager.Instance.sfxSources[1].clip = AudioManager.Instance.sfxClips[37];
        AudioManager.Instance.sfxSources[1].Play();
    }
    public void ArcherCharge()
    {
        AudioManager.Instance.sfxSources[2].clip = AudioManager.Instance.sfxClips[38];
        AudioManager.Instance.sfxSources[2].Play();
    }
    public void ArcherChargeEnd()
    {
        AudioManager.Instance.sfxSources[2].clip = AudioManager.Instance.sfxClips[38];
        AudioManager.Instance.sfxSources[2].Stop();
    }
    public void playersound_female_skillcooltime()
    {
        AudioManager.Instance.PlaySFX(39, 1.0f);
    }
}
