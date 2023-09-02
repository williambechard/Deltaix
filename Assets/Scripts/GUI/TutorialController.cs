using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TutorialController : MonoBehaviour
{
    public List<GameObject> TutorialCards = new List<GameObject>();
    public int index = 0;
    public float fadeDuration = 1.0f;

    private CanvasGroup currentCardCanvasGroup;
    private CanvasGroup nextCardCanvasGroup;

    // Start is called before the first frame update
    void Start()
    {
        index = -1;
        StartCoroutine(WaitForEventManager());
        StartCoroutine(FadeIn());
    }

    IEnumerator WaitForEventManager()
    {
        while (EventManager.Instance == null)
        {
            yield return new WaitForEndOfFrame();
        }
        Invoke("triggerGamePause", 0.1f);

    }

    void triggerGamePause()
    {
        EventManager.TriggerEvent("Paused", new Dictionary<string, object> { { "paused", true } });
    }

    public void TransitionNext()
    {
        StartCoroutine(FadeOut(() => StartCoroutine(FadeIn())));
    }

    private IEnumerator FadeOut(System.Action callback = null)
    {
        int targetIndex = index;
        if (targetIndex > TutorialCards.Count) yield break;
        currentCardCanvasGroup = TutorialCards[index].GetComponent<CanvasGroup>();
        float elapsedTime = 0.0f;
        while (elapsedTime < fadeDuration)
        {
            float normalizedTime = elapsedTime / fadeDuration;
            currentCardCanvasGroup.alpha = Mathf.Lerp(1.0f, 0.0f, normalizedTime);

            elapsedTime += Time.deltaTime;
            yield return null;
        }
        currentCardCanvasGroup.alpha = 0.0f;
        currentCardCanvasGroup.gameObject.SetActive(false);
        callback?.Invoke(); // Invoke the provided callback function
    }

    private IEnumerator FadeIn()
    {
        index++;
        if (index > TutorialCards.Count) yield break;
        CanvasGroup targetCard = TutorialCards[index].GetComponent<CanvasGroup>();
        targetCard.gameObject.SetActive(true);

        float elapsedTime = 0.0f;
        while (elapsedTime < fadeDuration)
        {
            float normalizedTime = elapsedTime / fadeDuration;
            targetCard.alpha = Mathf.Lerp(0.0f, 1.0f, normalizedTime);

            elapsedTime += Time.deltaTime;
            yield return null;
        }
        targetCard.alpha = 1.0f;
    }

    public void EndTutorial()
    {
        EventManager.TriggerEvent("Paused", new Dictionary<string, object> { { "paused", false } });

        StartCoroutine(FadeOut());
    }

    void Exit()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }
}
