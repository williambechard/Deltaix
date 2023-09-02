using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Laser : MonoBehaviour
{
    public LineRenderer laserLineRenderer;
    public Gradient colorGradient; // Gradient for laser color and alpha
    public float beamWidth = 2.0f;
    public float fadeDuration = 1.0f;
    OverlayDamage OD;
    public Transform laserEnd;

    private void Start()
    {
        StartCoroutine(WaitForEventManager());
        laserLineRenderer.startWidth = 0.0f;
        laserLineRenderer.endWidth = 0.0f;

        laserEnd = transform.parent.GetComponent<SatController>().laserEnd.transform;

        OD = transform.parent.GetComponent<SatController>().radarTrigger.GetComponent<OverlayDamage>();
        OD.isLaserOn = true;

        FireLaser();
    }


    IEnumerator WaitForEventManager()
    {
        while (EventManager.Instance == null)
        {
            yield return new WaitForEndOfFrame();
        }
        EventManager.StartListening("Trigger", Trigger);
    }

    private void OnDestroy()
    {
        EventManager.StopListening("Trigger", Trigger);
    }

    private void OnDisable()
    {
        EventManager.StopListening("Trigger", Trigger);
    }

    public void Trigger(Dictionary<string, object> obj)
    {
        Collider2D collision = (Collider2D)obj["collision"];

        Damagable d = collision.gameObject.GetComponent<Damagable>();
        if (d != null)
        {
            Debug.Log("DAMAGE applied");
            d.Damage(1);
        }

    }

    public void setTargetPosition(Vector3 tPosition)
    {
        // targetObj.transform.position = tPosition;
        // targetObj.transform.SetParent(parentOrbit);
    }

    public void FireLaser()
    {
        StartCoroutine(AnimateLaserBeam());
    }

    private IEnumerator AnimateLaserBeam()
    {
        float elapsedTime = 0.0f;

        while (elapsedTime < fadeDuration)
        {
            float normalizedTime = elapsedTime / fadeDuration;
            laserLineRenderer.startWidth = Mathf.Lerp(0.0f, beamWidth - (beamWidth * .60f), normalizedTime);
            laserLineRenderer.endWidth = Mathf.Lerp(0.0f, beamWidth, normalizedTime);

            // Calculate the current alpha value using the normalized time
            float alpha = 1.0f - normalizedTime;

            // Set the color gradient with adjusted alpha value
            //laserLineRenderer.colorGradient = ModifyAlpha(colorGradient, alpha);

            laserLineRenderer.SetPosition(0, transform.position);
            laserLineRenderer.SetPosition(1, laserEnd.position); // ((targetPosition - transform.position) * radius)

            elapsedTime += Time.deltaTime;
            yield return null;
        }

        elapsedTime = 0.0f;
        while (elapsedTime < fadeDuration)
        {
            float normalizedTime = elapsedTime / fadeDuration;
            laserLineRenderer.startWidth = Mathf.Lerp(beamWidth - (beamWidth * .60f), 0.0f, normalizedTime);
            laserLineRenderer.endWidth = Mathf.Lerp(beamWidth, 0.0f, normalizedTime);

            // Calculate the current alpha value using the normalized time
            float alpha = 1.0f - normalizedTime;

            // Set the color gradient with adjusted alpha value
            //laserLineRenderer.colorGradient = ModifyAlpha(colorGradient, alpha);

            laserLineRenderer.SetPosition(0, transform.position);
            laserLineRenderer.SetPosition(1, laserEnd.position);

            elapsedTime += Time.deltaTime;
            yield return null;
        }

        laserLineRenderer.startWidth = 0.0f;
        laserLineRenderer.endWidth = 0.0f;
        OD.isLaserOn = false;


        Destroy(gameObject);
    }


    private Gradient ModifyAlpha(Gradient gradient, float alpha)
    {
        Gradient modifiedGradient = new Gradient();
        GradientColorKey[] colorKeys = gradient.colorKeys;

        // Adjust alpha for each color key
        for (int i = 0; i < colorKeys.Length; i++)
        {
            colorKeys[i].color = new Color(colorKeys[i].color.r, colorKeys[i].color.g, colorKeys[i].color.b, alpha);
        }

        modifiedGradient.SetKeys(colorKeys, gradient.alphaKeys);
        return modifiedGradient;
    }
}
