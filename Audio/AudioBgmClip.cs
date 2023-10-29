using UnityEngine;

public class AudioBgmClip : MonoBehaviour
{
    public AudioClip[] bgmClips;

    void Start()
    {
        AudioManager.Instance.bgmClips = bgmClips;
    }
}
