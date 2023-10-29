using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GolemSignal : MonoBehaviour
{
    public AudioSource bgmSource;

    private bool isReduce = false;

    public void AudioLerp()
    {
        if (isReduce)
            return;

        StartCoroutine(DecreaseVolume());
    }

    private IEnumerator DecreaseVolume()
    {
        isReduce = true;

        float timer = 0f;

        while (timer < 10f)
        {
            bgmSource.volume = Mathf.Lerp(1f, 0f, timer / 10f);
            timer += Time.deltaTime;
            yield return null;
        }
        if (bgmSource.volume >= 0)
        {
            bgmSource.Stop();
            isReduce = false;
        }
    }
}
