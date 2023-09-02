using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DisabledBtnOnPause : MonoBehaviour
{
    private Button button;

    // Start is called before the first frame update
    void Start()
    {
        button = GetComponent<Button>();
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

    public void Paused(Dictionary<string, object> obj)
    {
        bool isPaused = (bool)obj["paused"];
        //button.interactable = !isPaused;
    }
}
