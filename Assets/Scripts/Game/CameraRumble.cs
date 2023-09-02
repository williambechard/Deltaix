using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraRumble : MonoBehaviour
{
    private Vector3 originalPosition;

    private void Start()
    {
        originalPosition = transform.localPosition;
        StartCoroutine(WaitForEventManager());
    }

    IEnumerator WaitForEventManager()
    {
        while (EventManager.Instance == null)
            yield return null;
        EventManager.StartListening("Rumble", Rumble);
    }

    private void Rumble(Dictionary<string, object> obj)
    {
        float intensity = (float)obj["intensity"];
        float duration = (float)obj["duration"];

        StartCoroutine(RumbleCoroutine(intensity, duration));
    }

    private void OnDestroy()
    {
        EventManager.StopListening("Rumble", Rumble);
    }

    private void OnDisable()
    {
        EventManager.StopListening("Rumble", Rumble);
    }



    private IEnumerator RumbleCoroutine(float intensity, float duration)
    {
        float elapsed = 0f;

        while (elapsed < duration)
        {
            Vector3 randomOffset = Random.insideUnitSphere * intensity;
            transform.localPosition = originalPosition + randomOffset;

            elapsed += Time.deltaTime;
            yield return null;
        }

        transform.localPosition = originalPosition;
    }
}