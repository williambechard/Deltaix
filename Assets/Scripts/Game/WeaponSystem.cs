using System.Collections;
using UnityEngine;

public class WeaponSystem : MonoBehaviour
{
    public float interval = 2.0f; // Interval in seconds
    private Radar radar;
    public ExecuteAction action;
    public int radarRange;
    public PauseHandler ph;

    private void Start()
    {
        ph = GetComponent<PauseHandler>();
        radar = GetComponentInChildren<Radar>();
        radarRange = GetComponent<SatController>().radarRange;
        StartCoroutine(ExecuteActionEveryXSeconds());
    }

    private IEnumerator ExecuteActionEveryXSeconds()
    {
        while (true)
        {
            if (radar != null && radar.targets.Count > 0 && !ph.isPaused)
            {
                // Do something with the targets
                ExecuteAction();
                yield return new WaitForSeconds(interval);
            }

            yield return new WaitForEndOfFrame();
        }
    }

    public GameObject GetClosestObject()
    {
        GameObject closestObject = null;
        float closestDistance = Mathf.Infinity;

        foreach (GameObject obj in radar.targets)
        {
            float distance = Vector3.Distance(transform.position, obj.transform.position);

            if (distance < closestDistance)
            {
                closestDistance = distance;
                closestObject = obj;
            }
        }

        return closestObject;
    }


    public void ExecuteAction()
    {
        if (!ph.isPaused)
        {
            GameObject target = GetClosestObject();
            if (target != null)
            {
                action.Execute(target, this.transform.position, this.gameObject, radarRange);
            }
        }

    }
}
