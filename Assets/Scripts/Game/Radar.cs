using System.Collections.Generic;
using UnityEngine;

public class Radar : MonoBehaviour
{
    public List<GameObject> targets = new List<GameObject>();

    private void OnTriggerEnter2D(Collider2D collision)
    {

        if (collision.gameObject.CompareTag("Targets"))
        {
            Debug.Log("ADD TARGET");
            targets.Add(collision.gameObject);
        }

    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Targets"))
        {

            if (targets.Contains(collision.gameObject))
                targets.Remove(collision.gameObject);
        }
    }
}
