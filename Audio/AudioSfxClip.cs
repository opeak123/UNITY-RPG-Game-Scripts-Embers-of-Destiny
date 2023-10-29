using UnityEngine;

public class AudioSfxClip : MonoBehaviour
{
    public AudioClip[] sfxClips;

    void Start()
    {
        AudioManager.Instance.sfxClips = sfxClips;
    }
}
