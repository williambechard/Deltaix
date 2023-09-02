using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ShopItem : MonoBehaviour
{
    public GameObject prefab;
    public int purchaseCost;
    public int buildCost;
    private PauseHandler PH;
    public Image FlashImage;
    public int Index;

    public int Cost;
    public TextMeshProUGUI CostDisplay;

    public Image image;
    public Image imageBG;

    public Button Purchase;

    public bool isAvailable = false;
    public Image SelectedImage;
    public Build buildFX;
    private ItemManager itemManager;
    public PlaySoundOneShot playSound;
    private itemState _itemState;

    public GameObject ItemInfo;

    public itemState ItemState
    {
        get { return _itemState; }
        set
        {
            itemState _prevState = _itemState;
            _itemState = value;
            switch (value)
            {
                case itemState.UNPURCHASED:
                    Color tempColor = image.color;
                    tempColor.a = .1f;
                    image.color = tempColor;
                    break;
                case itemState.AVAILABLE:
                    if (_prevState == itemState.UNPURCHASED)
                        EventManager.StopListening("ResourcesChanged", ResourcesChanged);
                    // only need to do this once, if previously unpurchased
                    // now need to remove the purchase button
                    if (_prevState == itemState.UNPURCHASED)
                        Purchase.gameObject.SetActive(false);

                    image.color = new Color(image.color.r, image.color.g, image.color.b, 1);
                    CostDisplay.gameObject.SetActive(true);
                    SelectedImage.gameObject.SetActive(false);
                    break;
                case itemState.BUILDING:
                    buildFX.StartBuildEffect(buildCost);
                    CostDisplay.gameObject.SetActive(false);
                    break;
                case itemState.READY:
                    StartCoroutine(flashOnce());
                    break;
                case itemState.SELECTED:
                    itemManager.setSelectedItem(this);
                    SelectedImage.gameObject.SetActive(true);
                    break;
            }
        }
    }


    public void ShowInfo()
    {

        if (!PH.isPaused) ItemInfo.SetActive(true);
    }

    public void HideInfo()
    {
        if (!PH.isPaused) ItemInfo.SetActive(false);
    }

    IEnumerator flashOnce()
    {
        Color originalColor = FlashImage.color;
        float flashDuration = 0.1f; // Duration for each flash
        int numberOfFlashes = 1;
        AudioManager.Instance.PlaySFXOneShot(playSound.SoundFXName);
        for (int i = 0; i < numberOfFlashes; i++)
        {
            // Flash from 0 alpha to 0.5 alpha
            float currentTime = 0f;
            while (currentTime < flashDuration)
            {
                float alpha = Mathf.Lerp(0f, 0.5f, currentTime / flashDuration);
                FlashImage.color = new Color(originalColor.r, originalColor.g, originalColor.b, alpha);
                currentTime += Time.deltaTime;
                yield return null;
            }

            // Flash from 0.5 alpha back to 0 alpha
            currentTime = 0f;
            while (currentTime < flashDuration)
            {
                float alpha = Mathf.Lerp(0.5f, 0f, currentTime / flashDuration);
                FlashImage.color = new Color(originalColor.r, originalColor.g, originalColor.b, alpha);
                currentTime += Time.deltaTime;
                yield return null;
            }
        }

        // Restore the original color
        FlashImage.color = originalColor;



        //auto select
        ItemState = itemState.SELECTED;
    }


    public enum itemState
    {
        UNPURCHASED,
        AVAILABLE,
        BUILDING,
        READY,
        SELECTED
    }


    // Start is called before the first frame update
    void Start()
    {
        itemManager = GetComponentInParent<ItemManager>();
        CostDisplay.text = buildCost.ToString();
        PH = GetComponent<PauseHandler>();
        StartCoroutine(WaitForEventManager());
    }

    IEnumerator WaitForEventManager()
    {
        while (EventManager.Instance == null)
        {
            yield return new WaitForEndOfFrame();
        }
        EventManager.StartListening("ResourcesChanged", ResourcesChanged);
    }
    private void OnDisable()
    {
        EventManager.StopListening("ResourcesChanged", ResourcesChanged);
    }

    private void OnDestroy()
    {
        EventManager.StopListening("ResourcesChanged", ResourcesChanged);
    }

    // Based on listening for any Resource change, this is responsible for whether the Purchase button is interactable
    // determined by whether there is enough Resources to purchase the item or not
    private void ResourcesChanged(Dictionary<string, object> obj)
    {
        int modifiedResource = ItemManager.Resources + (int)obj["resources"];

        if (ItemState == itemState.UNPURCHASED)
        {
            if (modifiedResource >= purchaseCost)
                Purchase.interactable = true;
            else
                Purchase.interactable = false;
        }
    }

    public void PurchaseItem()
    {
        if (!PH.isPaused)
        {
            if (ItemState == itemState.UNPURCHASED)
            {

                //ItemManager.Resources -= purchaseCost;
                EventManager.TriggerEvent("ResourcesChanged", new Dictionary<string, object> { { "resources", -purchaseCost } });
                ItemState = itemState.AVAILABLE;
            }
        }
    }

    public void BuildRequest()
    {
        if (!PH.isPaused)
        {
            switch (ItemState)
            {
                case itemState.AVAILABLE:
                    if (!itemManager.isBuilding && itemManager.SelectedItem == null)
                    {
                        itemManager.isBuilding = true;
                        ItemState = itemState.BUILDING;
                    }
                    else
                    {
                        //could warn user
                        EventManager.TriggerEvent("Alert", new Dictionary<string, object>() { { "alert", "Already building a satellite. You can only build one at a time." } });
                    }
                    break;
                case itemState.READY:
                    ItemState = itemState.SELECTED;
                    break;
                case itemState.BUILDING:
                    buildFX.SetPause();
                    break;
            }

        }
    }
}
