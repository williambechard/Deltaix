using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class nuke : MonoBehaviour
{
    private void Start()
    {
        StartCoroutine(WaitForEventManager());
    }
    private void OnDestroy()
    {
        EventManager.StopListening("Trigger", Trigger);
    }

    private void OnDisable()
    {
        EventManager.StopListening("Trigger", Trigger);
    }

    IEnumerator WaitForEventManager()
    {
        while (EventManager.Instance == null)
        {
            yield return null;
        }
        EventManager.StartListening("Trigger", Trigger);
    }

    public void Trigger(Dictionary<string, object> obj)
    {
        Debug.Log("nuke trigger");
        Collider2D collision = (Collider2D)obj["collision"];

        Damagable d = collision.gameObject.GetComponent<Damagable>();
        if (d != null)
        {
            Debug.Log("DAMAGE applied");
            d.Damage(100);
        }

    }
}
