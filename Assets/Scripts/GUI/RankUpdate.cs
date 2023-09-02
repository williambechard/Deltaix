using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class RankUpdate : MonoBehaviour
{
    public List<Texture2D> allRanks = new List<Texture2D>();
    public List<Info> allInfo = new List<Info>();

    public int targetIndex = 0;
    public Image rankImage;
    public Image rankShadow;
    public float flipDuration = 1.0f; // Duration of the flip animation
    private PlaySoundOneShot playSound;
    private Coroutine flipCoroutine;
    public GameObject RankEffect;
    public InfoBox infoBox;

    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(WaitForEventManager());
        playSound = GetComponent<PlaySoundOneShot>();

    }

    IEnumerator WaitForEventManager()
    {
        while (EventManager.Instance == null)
        {
            yield return new WaitForEndOfFrame();
        }
        EventManager.StartListening("RankUpdate", RankUpdateEvent);
    }

    private void OnDestroy()
    {
        EventManager.StopListening("RankUpdate", RankUpdateEvent);
    }

    private void OnDisable()
    {
        EventManager.StopListening("RankUpdate", RankUpdateEvent);
    }

    public void RankUpdateEvent(Dictionary<string, object> obj) => TriggerFlip();


    private IEnumerator FlipRank(bool forward)
    {
        GameObject effect = Instantiate(RankEffect, transform);
        effect.name = "RankEFFECT!";

        Vector3 originalScale = new Vector3(1.0f, 1.0f, 1.0f);
        Vector3 flippedScale = new Vector3(0.0f, 1.0f, 1.0f);
        playSound.playSound();
        float elapsedTime = 0.0f;

        while (elapsedTime < flipDuration)
        {
            float t = elapsedTime / flipDuration;
            rankImage.transform.localScale = Vector3.Lerp(originalScale, flippedScale, t);
            rankShadow.transform.localScale = Vector3.Lerp(originalScale, flippedScale, t);

            elapsedTime += Time.deltaTime;
            yield return null;
        }

        if (forward)
        {
            // Update targetIndex and clamp it
            targetIndex = Mathf.Clamp(targetIndex + 1, 0, allRanks.Count - 1);
        }
        else
        {
            // Update targetIndex and clamp it
            targetIndex = Mathf.Clamp(targetIndex - 1, 0, allRanks.Count - 1);
        }

        // Set new images
        rankImage.sprite = Sprite.Create(allRanks[targetIndex], new Rect(0, 0, allRanks[targetIndex].width, allRanks[targetIndex].height), Vector2.one * 0.5f);
        rankShadow.sprite = Sprite.Create(allRanks[targetIndex], new Rect(0, 0, allRanks[targetIndex].width, allRanks[targetIndex].height), Vector2.one * 0.5f);

        elapsedTime = 0.0f;
        while (elapsedTime < flipDuration)
        {
            float t = elapsedTime / flipDuration;
            rankImage.transform.localScale = Vector3.Lerp(flippedScale, originalScale, t);
            rankShadow.transform.localScale = Vector3.Lerp(flippedScale, originalScale, t);

            elapsedTime += Time.deltaTime;
            yield return null;
        }

        // Ensure the original scale is set
        rankImage.transform.localScale = originalScale;
        rankShadow.transform.localScale = originalScale;
        infoBox.info = allInfo[targetIndex];
        infoBox.UpdateInfo();
        flipCoroutine = null;
    }

    public void TriggerFlip()
    {
        if (targetIndex < allRanks.Count - 1 && flipCoroutine == null)
        {
            // Trigger the flip animation forward
            flipCoroutine = StartCoroutine(FlipRank(true));
        }
    }

}
