using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class ColorUpdate : MonoBehaviour
{
    [SerializeField]
    private Image image;
    private Color originColor = new Color(0f, 1f, 1f);
    private Color targetColor = new Color(1f, 0f, 1f);
    private float timer = 5f;
    private bool colorChanged = false;
    private float startTime;

    void Start()
    {
        image = GetComponent<Image>();
        image.color = originColor;
    }

    void Update()
    {
        if (!colorChanged)
        {
            StartCoroutine(TransitionColor());
        }
    }

    IEnumerator TransitionColor()
    {
        colorChanged = true;
        startTime = Time.time;

        while (Time.time - startTime <= timer)
        {
            float t = (Time.time - startTime) / timer;
            image.color = Color.Lerp(originColor, targetColor, t);
            yield return null;
        }
        Color tempColor = originColor;
        originColor = targetColor;
        targetColor = tempColor;

        colorChanged = false;
    }
}
