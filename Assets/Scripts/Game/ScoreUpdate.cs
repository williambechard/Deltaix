using System.Collections;
using UnityEngine;

public class ScoreUpdate : MonoBehaviour
{

    int score = 0;

    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(WaitForEventManager());
    }

    /*
    IEnumerator ScoreTick()
    {
        while (true)
        {
            yield return new WaitForSeconds(1);
            score++;
            EventManager.TriggerEvent("ScoreUpdate", new Dictionary<string, object> { { "score", score } });
        }
    }
    */
    IEnumerator WaitForEventManager()
    {
        while (EventManager.Instance == null)
        {
            yield return new WaitForEndOfFrame();
        }
        //StartCoroutine(ScoreTick());

        // EventManager.StartListening("ScoreUpdate", ScoreUpdate);
    }

    // public void ScoreUpdate( Dictionary<string, object> result)
    // {

    //  }

    // Update is called once per frame
    void Update()
    {

    }
}
