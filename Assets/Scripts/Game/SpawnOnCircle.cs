using System.Collections;
using UnityEngine;

public class SpawnOnCircle : MonoBehaviour
{
    public GameObject objectToSpawn;
    public float spawnInterval = 2.0f;
    public float circleRadius = 5.0f;

    public GameObject targetObject;

    float[] sizes = new float[3] { .5f, .75f, 1 };
    float[] speeds = new float[3] { 5, 3, 1 };

    private int currentQuadrant = 0;
    private bool[] usedQuadrants = new bool[4];

#if UNITY_EDITOR
    private void OnDrawGizmos()
    {
        Gizmos.color = Color.cyan;
        Gizmos.DrawWireSphere(transform.position, circleRadius);
    }
#endif

    private void Start()
    {
        StartCoroutine(SpawnObjects());
    }

    private IEnumerator SpawnObjects()
    {
        while (true)
        {
            int nextQuadrant = GetNextQuadrant();
            Vector3 spawnPosition = GetRandomPositionInQuadrant(nextQuadrant);

            GameObject spawnedObj = Instantiate(objectToSpawn, spawnPosition, Quaternion.identity);
            spawnedObj.GetComponent<MoveTo>().target = targetObject.transform;

            int rndIndex = Random.Range(0, 3);

            float rndSize = sizes[rndIndex];

            spawnedObj.transform.localScale = new Vector3(rndSize, rndSize, 1);
            spawnedObj.GetComponent<MoveTo>().speed = speeds[rndIndex];

            yield return new WaitForSeconds(spawnInterval);
        }
    }

    private int GetNextQuadrant()
    {
        int nextQuadrant = currentQuadrant;

        int unusedCount = 0;
        for (int i = 0; i < usedQuadrants.Length; i++)
        {
            if (!usedQuadrants[i])
            {
                unusedCount++;
            }
        }

        if (unusedCount == 0)
        {
            ResetUsedQuadrants();
            nextQuadrant = 0;
        }
        else
        {
            while (usedQuadrants[nextQuadrant])
            {
                nextQuadrant = (nextQuadrant + 1) % 4;
            }
        }

        usedQuadrants[nextQuadrant] = true;
        currentQuadrant = nextQuadrant;

        return nextQuadrant;
    }

    private void ResetUsedQuadrants()
    {
        for (int i = 0; i < usedQuadrants.Length; i++)
        {
            usedQuadrants[i] = false;
        }
    }

    private Vector3 GetRandomPositionInQuadrant(int quadrant)
    {
        float angle = Random.Range(quadrant * 90.0f, (quadrant + 1) * 90.0f);
        Vector3 position = Quaternion.Euler(0, 0, angle) * Vector3.right * circleRadius;

        return position;
    }
}
