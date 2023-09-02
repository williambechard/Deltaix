using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PauseHandler : MonoBehaviour
{

    public bool isPaused = false;

    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(WaitForEventManager());
    }

    IEnumerator WaitForEventManager()
    {
        while (EventManager.Instance == null)
        {
            yield return new WaitForEndOfFrame();
        }
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

    public void Paused(Dictionary<string, object> obj) => isPaused = (bool)obj["paused"];

}
