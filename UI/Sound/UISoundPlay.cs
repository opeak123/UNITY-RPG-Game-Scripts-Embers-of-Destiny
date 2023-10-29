using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UISoundPlay : MonoBehaviour
{
    public static UISoundPlay Instance;

    public void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(this.gameObject);
            return;
        }
        Instance = this;
    }

    public void OpenUISound()
    {
        AudioManager.Instance.sfxSources[3].clip = AudioManager.Instance.sfxClips[21];
        AudioManager.Instance.sfxSources[3].Play();
    }
    public void CloseUISound()
    {
        AudioManager.Instance.sfxSources[3].clip = AudioManager.Instance.sfxClips[22];
        AudioManager.Instance.sfxSources[3].Play();
    }
    public void DeniedSound()
    {
        Debug.Log("HI");
        AudioManager.Instance.sfxSources[3].clip = AudioManager.Instance.sfxClips[23];
        AudioManager.Instance.sfxSources[3].Play();
        //AudioManager.Instance.PlaySFX(23, 1f);
    }
    public void PickUpSound()
    {
        AudioManager.Instance.sfxSources[3].clip = AudioManager.Instance.sfxClips[24];
        AudioManager.Instance.sfxSources[3].Play();
    }
    public void QuestDoneSound()
    {
        AudioManager.Instance.sfxSources[4].clip = AudioManager.Instance.sfxClips[25];
        AudioManager.Instance.sfxSources[4].Play();
    }
    public void BlackSmithSound()
    {
        AudioManager.Instance.sfxSources[4].clip = AudioManager.Instance.sfxClips[26];
        AudioManager.Instance.sfxSources[4].Play();
    }
    public void ForgingSound()
    {
        AudioManager.Instance.sfxSources[4].clip = AudioManager.Instance.sfxClips[27];
        AudioManager.Instance.sfxSources[4].Play();
    }
    public void OpenInventorySound()
    {
        AudioManager.Instance.sfxSources[3].clip = AudioManager.Instance.sfxClips[28];
        AudioManager.Instance.sfxSources[3].Play();
    }
    public void PurchaseSound()
    {
        AudioManager.Instance.sfxSources[4].clip = AudioManager.Instance.sfxClips[29];
        AudioManager.Instance.sfxSources[4].Play();
    }
    public void PortionSound()
    {
        AudioManager.Instance.sfxSources[4].clip = AudioManager.Instance.sfxClips[30];
        AudioManager.Instance.sfxSources[4].Play();
    }
    public void EquipArmorSound()
    {
        AudioManager.Instance.sfxSources[4].clip = AudioManager.Instance.sfxClips[31];
        AudioManager.Instance.sfxSources[4].Play();
    }
    public void EquipWeaponSound()
    {
        AudioManager.Instance.sfxSources[4].clip = AudioManager.Instance.sfxClips[32];
        AudioManager.Instance.sfxSources[4].Play();
    }
    public void ClickSound()
    {
        AudioManager.Instance.sfxSources[4].clip = AudioManager.Instance.sfxClips[40];
        AudioManager.Instance.sfxSources[4].Play();
    }
    public void LoginBGM()
    {
        AudioManager.Instance.bgmSources[0].clip = AudioManager.Instance.bgmClips[0];
        AudioManager.Instance.bgmSources[0].Play();
    }
}
