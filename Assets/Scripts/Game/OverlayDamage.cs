using System.Collections.Generic;
using UnityEngine;

public class OverlayDamage : MonoBehaviour
{
    public int damageAmount;
    public float tickCount = 1f;

    public bool isLaserOn = false;
    private float lastEvaluationTime = 0f;
    public float evaluationInterval = 1f; // Adjust this value to set the evaluation interval

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Targets") && isLaserOn)
        {
            Debug.Log("Explosion from nuke");
            EventManager.TriggerEvent("Trigger", new Dictionary<string, object>() { { "collision", collision } });
        }
    }


    private void OnTriggerStay2D(Collider2D collision)
    {

        if (collision.gameObject.CompareTag("Targets") && isLaserOn)
        {
            float currentTime = Time.time;

            // Check if the time difference since the last evaluation is greater than the interval
            if (currentTime - lastEvaluationTime >= evaluationInterval)
            {
                lastEvaluationTime = currentTime;

                EventManager.TriggerEvent("Trigger", new Dictionary<string, object>() { { "collision", collision } });
            }
        }

    }








}
