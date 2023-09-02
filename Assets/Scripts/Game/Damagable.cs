using System.Collections.Generic;
using UnityEngine;

public class Damagable : MonoBehaviour
{
    public int health;
    public int size;

    public GameObject endingExplosionPrefab;
    public GameObject damageExplosionPrefab;

    public GameObject objPrefab;

    public GameObject cluster3;
    public GameObject cluster2;

    public PlaySoundOneShot playSoundDamage;
    public PlaySoundOneShot playSoundExplosion;

    // Start is called before the first frame update
    void Start()
    {

    }


    // create a function that spread 3 points (with a radius of 1) around the center of this transform position without overlapping
    public Vector3[] GetRandomPoints(int numPoints)
    {
        Vector3[] spreadPoints = new Vector3[numPoints];
        float angleIncrement = 360f / numPoints; // Spread evenly in a circle

        for (int i = 0; i < numPoints; i++)
        {
            float angle = i * angleIncrement;
            float radians = angle * Mathf.Deg2Rad;

            float xOffset = Mathf.Cos(radians) * 1f; // Scale by radius (1)
            float yOffset = Mathf.Sin(radians) * 1f; // Scale by radius (1)

            Vector3 offset = new Vector3(xOffset, yOffset, 0);

            spreadPoints[i] = offset;
        }

        return spreadPoints;
    }



    public void Damage(int damageAmount)
    {
        health -= damageAmount;
        if (health <= 0)
        {
            switch (size)
            {
                case 3:
                    GameObject g = Instantiate(cluster3, transform.position, Quaternion.identity);
                    foreach (MoveTo asteroid in g.GetComponentsInChildren<MoveTo>())
                    {
                        asteroid.targetPosition = new Vector3(0, 0, 0);
                    }
                    EventManager.TriggerEvent("ResourcesChanged", new Dictionary<string, object>() { { "resources", 20 } });
                    break;
                case 5:
                    GameObject g2 = Instantiate(cluster2, transform.position, Quaternion.identity);
                    foreach (MoveTo asteroid in g2.GetComponentsInChildren<MoveTo>())
                    {
                        asteroid.targetPosition = new Vector3(0, 0, 0);
                    }
                    EventManager.TriggerEvent("ResourcesChanged", new Dictionary<string, object>() { { "resources", 40 } });
                    break;
                case 1:
                    EventManager.TriggerEvent("ResourcesChanged", new Dictionary<string, object>() { { "resources", 10 } });
                    break;
            }
            Debug.Log("Size " + size);
            DestroyObject();
        }
        else
        {
            AudioManager.Instance.PlaySFXOneShot(playSoundDamage.SoundFXName);
            Instantiate(damageExplosionPrefab, new Vector3(transform.position.x, transform.position.y, -15), Quaternion.identity);
        }
    }

    public void DestroyObject()
    {
        EventManager.TriggerEvent("ScoreUpdate", new Dictionary<string, object>() { { "score", (size * 10) } });
        AudioManager.Instance.PlaySFXOneShot(playSoundExplosion.SoundFXName);
        Instantiate(endingExplosionPrefab, transform.position, Quaternion.identity);
        Destroy(gameObject);
    }

    // Update is called once per frame
    void Update()
    {

    }
}
