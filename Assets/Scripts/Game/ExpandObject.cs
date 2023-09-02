using System.Collections;
using UnityEngine;

public class ExpandObject : MonoBehaviour
{
    public GameObject objectToExpand;
    public Vector2 targetScale = new Vector2(2.0f, 2.0f); // The final scale you want to reach
    public float expandDuration = 2.0f; // Time it takes to reach the target scale
    public float fadeOutDuration = 1.0f; // Time it takes to fade out
    public SpriteRenderer sr;

    private void Start()
    {
        StartCoroutine(ExpandObjectScale());
    }

    private IEnumerator ExpandObjectScale()
    {
        Vector2 initialScale = objectToExpand.transform.localScale;
        float elapsedTime = 0.0f;

        while (elapsedTime < expandDuration)
        {
            float normalizedTime = elapsedTime / expandDuration;
            Vector2 currentScale = Vector2.Lerp(initialScale, targetScale, normalizedTime);

            objectToExpand.transform.localScale = currentScale;

            elapsedTime += Time.deltaTime;
            yield return null;
        }

        // Ensure the final scale is set precisely
        objectToExpand.transform.localScale = targetScale;
        // Start fading out
        float fadeStartTime = Time.time;
        Color startColor = sr.color;

        while (Time.time - fadeStartTime < fadeOutDuration)
        {
            float normalizedTime = (Time.time - fadeStartTime) / fadeOutDuration;
            Color newColor = startColor;
            newColor.a = Mathf.Lerp(startColor.a, 0.0f, normalizedTime);
            sr.color = newColor;

            yield return null;
        }

        // Ensure the final alpha value is set precisely
        Color finalColor = sr.color;
        finalColor.a = 0.0f;
        sr.color = finalColor;

        Destroy(gameObject);
    }
}
