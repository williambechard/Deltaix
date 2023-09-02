using System.Collections.Generic;
using UnityEngine;

public class SatelliteCollide : MonoBehaviour
{

    private PlaySoundOneShot playSound;
    public bool satCollide = true;
    public bool targetCollide;
    public GameObject targetCollidePrefab;

    private void Start()
    {
        playSound = GetComponent<PlaySoundOneShot>();

    }
    private void OnCollisionEnter2D(Collision2D collision)
    {
        Debug.Log("Collision");
        if (collision.gameObject.CompareTag("Satellite"))
        {
            EventManager.TriggerEvent("Rumble", new Dictionary<string, object>() { { "intensity", .25f }, { "duration", .25f } });
            AudioManager.Instance.PlaySFXOneShot(playSound.SoundFXName);
            Instantiate(GetComponentInParent<SatController>().explosionPrefab, transform.position, Quaternion.identity);

            //destroy both
            Destroy(collision.transform.parent.gameObject);
            Destroy(transform.parent.gameObject);
        }
        else if (collision.gameObject.CompareTag("Targets"))
        {
            if (targetCollidePrefab != null)
            {
                EventManager.TriggerEvent("Rumble", new Dictionary<string, object>() { { "intensity", .5f }, { "duration", 1f } });
                Destroy(transform.parent.gameObject);
                Instantiate(targetCollidePrefab, new Vector3(transform.position.x, transform.position.y, 0), Quaternion.identity);
            }
        }
    }
}
