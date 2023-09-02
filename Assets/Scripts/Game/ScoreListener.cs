using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class ScoreListener : MonoBehaviour
{

    public TextMeshProUGUI scoreText;
    PlaySoundOneShot playSoundOneShot;


    // Start is called before the first frame update
    void Start()
    {
        playSoundOneShot = GetComponent<PlaySoundOneShot>();
        StartCoroutine(WaitForEventManager());
    }

    IEnumerator WaitForEventManager()
    {
        while (EventManager.Instance == null)
        {
            yield return new WaitForEndOfFrame();
        }
        EventManager.StartListening("ScoreUpdate", ScoreUpdate);
    }

    private void OnDestroy()
    {
        EventManager.StopListening("ScoreUpdate", ScoreUpdate);
    }

    public void ScoreUpdate(Dictionary<string, object> result)
    {
        Debug.Log("ScoreUpdate: " + result["score"]);
        scoreText.text = "Score: " + result["score"].ToString();
        playSoundOneShot.playSound();
    }

    // Update is called once per frame
    void Update()
    {

    }
}
