using System.Collections;
using UnityEngine;

public class ScaleEffect : MonoBehaviour
{
    public float scaleDuration = 1.0f;
    public float maxScale = 2.0f;
    private Vector3 originalScale;
    public float StartDelay = 0f; // Maximum random delay before starting

    private void Start()
    {
        originalScale = transform.localScale;
        StartCoroutine(ScaleLoop());
    }

    private IEnumerator ScaleLoop()
    {

        yield return new WaitForSeconds(StartDelay);

        while (true)
        {
            // Scale up to max scale
            float startTime = Time.time;
            while (Time.time - startTime < scaleDuration)
            {
                float t = (Time.time - startTime) / scaleDuration;
                float scaleValue = Mathf.Lerp(originalScale.x, maxScale, t);
                transform.localScale = new Vector3(scaleValue, scaleValue, originalScale.z);
                yield return null;
            }

            // Scale back to original scale
            startTime = Time.time;
            while (Time.time - startTime < scaleDuration)
            {
                float t = (Time.time - startTime) / scaleDuration;
                float scaleValue = Mathf.Lerp(maxScale, originalScale.x, t);
                transform.localScale = new Vector3(scaleValue, scaleValue, originalScale.z);
                yield return null;
            }
        }
    }
}
