using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnOnRectangle : MonoBehaviour
{
    public GameObject objectToSpawn;
    public float spawnInterval = 2.0f;
    public float rectangleWidth = 10.0f;
    public float rectangleHeight = 5.0f;
    private PauseHandler PH;
    public GameObject targetObject;

    float[] sizes = new float[3] { .5f, .75f, 1 };
    float[] speeds = new float[3] { 5, 3, 2 };
    int[] health = new int[3] { 1, 3, 5 };
    public int level = 1;
    private int currentQuadrant = 0;
    private bool[] usedQuadrants = new bool[4];

#if UNITY_EDITOR
    private void OnDrawGizmos()
    {
        Gizmos.color = Color.cyan;
        Vector3 halfSize = new Vector3(rectangleWidth * 0.5f, rectangleHeight * 0.5f, 0);
        Gizmos.DrawWireCube(transform.position, new Vector3(rectangleWidth, rectangleHeight, 0));
    }
#endif

    private void Start()
    {
        StartCoroutine(WaitForEventManager());
        PH = GetComponent<PauseHandler>();
        Invoke("BeginSpawn", 1.0f);
    }

    IEnumerator WaitForEventManager()
    {
        while (EventManager.Instance == null)
        {
            yield return null;
        }
        EventManager.StartListening("levelUpdate", LevelUpdate);
    }
    private void OnDestroy()
    {
        EventManager.StopListening("levelUpdate", LevelUpdate);
    }
    private void OnDisable()
    {
        EventManager.StopListening("levelUpdate", LevelUpdate);
    }

    public void LevelUpdate(Dictionary<string, object> obj)
    {
        level = (int)obj["level"];
    }

    void BeginSpawn()
    {
        StartCoroutine(SpawnObjects());
    }

    private IEnumerator SpawnObjects()
    {
        while (true)
        {
            if (!PH.isPaused)
            {
                int nextQuadrant = GetNextQuadrant();
                Vector3 spawnPosition = GetRandomBorderPosition(nextQuadrant);

                GameObject spawnedObj = Instantiate(objectToSpawn, spawnPosition, Quaternion.identity);
                spawnedObj.GetComponent<MoveTo>().target = targetObject.transform;

                int rndIndex = Random.Range(0, 3);

                float rndSize = sizes[rndIndex];

                spawnedObj.GetComponent<Damagable>().health = (int)health[rndIndex] + (level - 1);
                spawnedObj.GetComponent<Damagable>().size = (int)health[rndIndex];

                spawnedObj.transform.localScale = new Vector3(rndSize, rndSize, 1);
                spawnedObj.GetComponent<MoveTo>().speed = speeds[rndIndex] + level;
                spawnInterval = Mathf.Clamp((spawnInterval / level), .1f, spawnInterval);
            }
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

    private Vector3 GetRandomBorderPosition(int quadrant)
    {
        Vector3 position = Vector3.zero;

        switch (quadrant)
        {
            case 0:
                position = new Vector3(Random.Range(-rectangleWidth * 0.5f, rectangleWidth * 0.5f), rectangleHeight * 0.5f, 0);
                break;
            case 1:
                position = new Vector3(Random.Range(-rectangleWidth * 0.5f, rectangleWidth * 0.5f), -rectangleHeight * 0.5f, 0);
                break;
            case 2:
                position = new Vector3(-rectangleWidth * 0.5f, Random.Range(-rectangleHeight * 0.5f, rectangleHeight * 0.5f), 0);
                break;
            case 3:
                position = new Vector3(rectangleWidth * 0.5f, Random.Range(-rectangleHeight * 0.5f, rectangleHeight * 0.5f), 0);
                break;
        }

        return position;
    }
}
