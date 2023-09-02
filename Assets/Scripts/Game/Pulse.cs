using System.Collections;
using UnityEngine;

public class Pulse : MonoBehaviour
{
    public SpriteRenderer sr;
    public GameObject objectToExpand;
    public Vector2 targetScale = new Vector2(2.0f, 2.0f); // The final scale you want to reach
    public float totalExpandDuration = 6.0f; // Total time for all pulses combined
    public int pulseCount = 3; // Number of pulses
    public PlanetImpact planetImpact;
    public PlaySoundOneShot playSound;
    private PauseHandler pauseHandler;

    private void Start()
    {
        pauseHandler = GetComponent<PauseHandler>();
        playSound = GetComponent<PlaySoundOneShot>();
        planetImpact = GameObject.FindFirstObjectByType<PlanetImpact>();
        StartCoroutine(ExpandObjectPulses());
    }

    private IEnumerator ExpandObjectPulses()
    {
        for (int i = 0; i < pulseCount; i++)
        {
            yield return StartCoroutine(ExpandPulse());
        }

        // Destroy the GameObject after all pulses
        Destroy(transform.parent.gameObject);
    }

    private IEnumerator ExpandPulse()
    {
        playSound.playSound();
        float pulseDuration = totalExpandDuration / pulseCount;

        Vector2 initialScale = objectToExpand.transform.localScale;
        float elapsedTime = 0.0f;

        while (elapsedTime < pulseDuration)
        {
            if (pauseHandler.isPaused)
            {
                yield return null;
            }
            float normalizedTime = elapsedTime / pulseDuration;
            Vector2 currentScale = Vector2.Lerp(initialScale, targetScale, normalizedTime);

            objectToExpand.transform.localScale = currentScale;
            // Calculate the alpha value based on the current normalized time
            float alpha = 1 - normalizedTime;

            // Set the alpha value for the SpriteRenderer color
            Color spriteColor2 = sr.color;
            spriteColor2.a = alpha;
            sr.color = spriteColor2;

            elapsedTime += Time.deltaTime;
            yield return null;
        }

        // Ensure the final scale is set precisely
        objectToExpand.transform.localScale = targetScale;

        /*
        // Reverse the expansion
        elapsedTime = 0.0f;

        while (elapsedTime < pulseDuration)
        {
            float normalizedTime = elapsedTime / pulseDuration;
            Vector2 currentScale = Vector2.Lerp(targetScale, initialScale, normalizedTime);

            objectToExpand.transform.localScale = currentScale;
            // Calculate the alpha value based on the current normalized time
            float alpha = 1.0f - normalizedTime;

            // Set the alpha value for the SpriteRenderer color
            Color spriteColor = sr.color;
            spriteColor.a = alpha;
            sr.color = spriteColor;

            elapsedTime += Time.deltaTime;
            yield return null;
        }
        */
        // Ensure the original scale is set precisely
        objectToExpand.transform.localScale = initialScale;
        Color spriteColor = sr.color;
        spriteColor.a = 1;
        sr.color = spriteColor;
        planetImpact.adjustHealth(2);

    }
}
