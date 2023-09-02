using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Build : MonoBehaviour
{
    public RectTransform BuildStatusImage;
    public bool isPaused = false;
    float startWidth = 100;
    float endWidth = 0;
    private PauseHandler PH;
    public int buildCost;

    public Image PauseImage;


    public float buildTime = 5;
    public bool isBuilding = false;

    private ShopItem shopItem;

    private ItemManager itemManager;

    public PlaySoundOneShot playSound;

    IEnumerator BuildEffect()
    {
        float elapsedTime = 0;
        float buildProgress = 0f;
        BuildStatusImage.sizeDelta = new Vector2(startWidth, BuildStatusImage.sizeDelta.y);
        int intBuildValue = 0;
        List<int> usedBuildValues = new List<int>();

        AudioManager.Instance.PlaySFXOneShot(playSound.SoundFXName);
        bool isSoundPaused = false;

        while (elapsedTime < buildTime)
        {

            if (!isPaused && ItemManager.Resources > 0 && !PH.isPaused)
            {

                if (isSoundPaused)
                {
                    AudioManager.Instance.sfxSource.UnPause();
                    isSoundPaused = false;
                }
                else if (AudioManager.Instance.sfxSource.isPlaying == false)
                {
                    AudioManager.Instance.PlaySFXOneShot(playSound.SoundFXName);
                }

                float t = elapsedTime / buildTime;
                float newWidth = Mathf.Lerp(startWidth, endWidth, t);
                buildProgress = Mathf.Lerp(0, buildCost, t);
                intBuildValue = Mathf.FloorToInt(buildProgress);
                // Check if buildProgress is an even number above 0
                if (intBuildValue > 0)
                {
                    if (!usedBuildValues.Contains(intBuildValue))
                    {
                        EventManager.TriggerEvent("ResourcesChanged", new Dictionary<string, object> { { "resources", -1 } });
                        usedBuildValues.Add(intBuildValue);
                    }
                }

                BuildStatusImage.sizeDelta = new Vector2(newWidth, BuildStatusImage.sizeDelta.y);

                elapsedTime += Time.deltaTime;
            }
            else
            {
                AudioManager.Instance.sfxSource.Pause();
                isSoundPaused = true;
            }

            yield return new WaitForEndOfFrame();
        }


        if (usedBuildValues.Count < buildCost)
            EventManager.TriggerEvent("ResourcesChanged", new Dictionary<string, object> { { "resources", -1 } });


        AudioManager.Instance.sfxSource.Stop();
        BuildStatusImage.sizeDelta = new Vector2(endWidth, BuildStatusImage.sizeDelta.y);
        shopItem.ItemState = ShopItem.itemState.READY;
        itemManager.isBuilding = false;

    }

    public void SetPause()
    {
        isPaused = !isPaused;
        PauseImage.gameObject.SetActive(isPaused);
    }

    public void StartBuildEffect(int cost)
    {

        buildCost = cost;
        StartCoroutine(BuildEffect());

    }


    private void Start()
    {
        PH = GetComponent<PauseHandler>();
        shopItem = GetComponent<ShopItem>();
        itemManager = GetComponentInParent<ItemManager>();
        playSound = GetComponent<PlaySoundOneShot>();
        //StartCoroutine(WaitForEventManager());
    }




}
