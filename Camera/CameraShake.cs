using UnityEngine;
using System.Collections;

public class CameraShake : MonoBehaviour
{
    public float shakeDuration = 0.5f;
    public float shakeMagnitude = 0.1f;

    private Vector3 originalPosition;

    private void Start()
    {
        originalPosition = Camera.main.transform.localPosition;
    }

    public void ShakeCamera()
    {
        shakeDuration = 0.5f;
        StartCoroutine(Shake());
    }

    public void HitShake()
    {
        shakeDuration = 0.3f;
        StartCoroutine(Shake());
    }

    private IEnumerator Shake()
    {
        float elapsedTime = 0f;

        while (elapsedTime < shakeDuration)
        {
            // Generate random offset for the camera position
            Vector3 randomOffset = Random.insideUnitSphere * shakeMagnitude;

            // Apply the offset to the camera's position
            originalPosition = Camera.main.transform.localPosition;
            Camera.main.transform.localPosition += randomOffset;

            elapsedTime += Time.deltaTime;

            yield return null;
        }

        // Reset the camera position
        Camera.main.transform.localPosition = originalPosition;
    }
}
