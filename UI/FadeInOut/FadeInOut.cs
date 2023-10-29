using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class FadeInOut : MonoBehaviour
{
    public GameObject fadePannel;


    public IEnumerator FadeInStart()
    {
        fadePannel.SetActive(true);
        for (float f = 1f; f > 0; f -= 0.02f)
        {
            Color c = fadePannel.GetComponent<Image>().color;
            c.a = f;
            fadePannel.GetComponent<Image>().color = c;
            yield return new WaitForSeconds(0.02f);
        }
        fadePannel.SetActive(false);
    }
    //∆‰¿ÃµÂ æ∆øÙ
    public IEnumerator FadeOutStart()
    {
        fadePannel.SetActive(true);
        for (float f = 0f; f < 1; f += 0.02f)
        {
            Color c = fadePannel.GetComponent<Image>().color;
            c.a = f;
            fadePannel.GetComponent<Image>().color = c;
           
            yield return new WaitForSeconds(0.02f);
        }
        yield return null;

    }
    public IEnumerator FadeOutInStart()
    {
        fadePannel.SetActive(true);
        for (float f = 0f; f < 1; f += 0.02f)
        {
            Color c = fadePannel.GetComponent<Image>().color;
            c.a = f;
            fadePannel.GetComponent<Image>().color = c;
            yield return null;
        }
        yield return StartCoroutine(FadeInStart());

    }
}
