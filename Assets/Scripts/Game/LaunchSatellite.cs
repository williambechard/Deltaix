using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LaunchSatellite : MonoBehaviour
{
    public float duration;
    public float radiusLimit;
    public AnimationCurve easeCurve; // Assign an ease curve in the Inspector
    public Vector3 tPosition;
    public GameObject spriteObj;

    //public PolygonCollider2D radarTrigger;
    public PauseHandler ph;
    public GameObject parent;

    private float elapsedTime = 0.0f;
    private Vector3 initialPosition;

    // Start is called before the first frame update
    void Start()
    {
        ph = GetComponent<PauseHandler>();
        initialPosition = transform.position;
        //radarTrigger = GetComponentInChildren<PolygonCollider2D>();
        EventManager.TriggerEvent("Rumble", new Dictionary<string, object>() { { "intensity", .25f }, { "duration", .5f } });
        StartCoroutine(MoveToTarget());
    }

    private IEnumerator MoveToTarget()
    {
        float initialDistance = Vector3.Distance(initialPosition, tPosition);
        float targetMagnitude = radiusLimit;

        //Vector3 directionRadar = (tPosition - initialPosition).normalized;
        //float angle = Mathf.Atan2(directionRadar.y, directionRadar.x) * Mathf.Rad2Deg;
        //radarTrigger.transform.rotation = Quaternion.Euler(0, 0, angle);

        while (elapsedTime < 1.0f) // Progress from 0 to 1
        {
            if (!ph.isPaused)
            {

                float easedT = easeCurve.Evaluate(elapsedTime); // Evaluate the ease curve

                Vector3 direction = (tPosition - initialPosition).normalized;
                float magnitude = Mathf.Lerp(0, targetMagnitude, easedT);

                Vector3 newPosition = initialPosition + direction * magnitude;

                transform.position = newPosition;
                elapsedTime += Time.deltaTime / duration; // Use elapsedTime to progress from 0 to 1
            }
            yield return new WaitForEndOfFrame();


        }

        transform.parent = parent.transform;
    }
}
