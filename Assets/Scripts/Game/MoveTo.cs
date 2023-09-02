using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveTo : MonoBehaviour
{
    public Transform target; // The object to move towards
    public float speed = 5.0f; // Speed of movement
    public Vector3 targetPosition;
    bool isPaused = false;
    public bool updatePosition = false;

    private void Start()
    {
        StartCoroutine(WaitForEventManager());
    }

    IEnumerator WaitForEventManager()
    {
        while (EventManager.Instance == null)
            yield return null;

        if (target != null)
        {
            targetPosition = target.position;
        }
        else Destroy(this.gameObject);

        StartCoroutine(MoveToTarget());
        EventManager.StartListening("Paused", Paused);
    }

    private void OnDestroy()
    {
        EventManager.StopListening("Paused", Paused);
    }

    private void OnDisable()
    {
        EventManager.StopListening("Paused", Paused);
    }

    public void Paused(Dictionary<string, object> obj)
    {
        isPaused = (bool)obj["paused"];
    }


    private IEnumerator MoveToTarget()
    {
        Vector3 currentPosition = transform.position;
        Vector3 origPosition = currentPosition;

        if (targetPosition == null) Destroy(this.gameObject);
        Vector3 direction = (targetPosition - origPosition).normalized;

        while (true)
        {
            if (!isPaused)
            {
                Vector3 movement = direction.normalized * speed * Time.deltaTime;
                transform.position += movement;
            }
            yield return null;
        }
    }

    public void DestroyObj()
    {
        Destroy(this.gameObject);
    }
}
