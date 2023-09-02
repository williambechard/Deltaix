using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using static ShopItem;

public class ItemManager : MonoBehaviour
{
    public ShopItem SelectedItem;
    public static int Resources = 0;
    public TextMeshProUGUI ResourcesCount;
    public int ResourcesMax = 9999;
    public bool isBuilding = false;

    private PlaySoundOneShot playSound;

    public void setSelectedItem(ShopItem shopItem)
    {
        SelectedItem = shopItem;
        EventManager.TriggerEvent("LaunchReady", new Dictionary<string, object>() { { "ShopItem", SelectedItem } });
    }

    public void Start()
    {
        playSound = GetComponent<PlaySoundOneShot>();
        StartCoroutine(WaitForEventManager());
    }

    public void initSetup()
    {
        //loop through all shop items in order to set them to unpurchased
        for (int i = 0; i < GetComponentsInChildren<ShopItem>().Length; i++)
        {
            if (i == 0) GetComponentInChildren<ShopItem>().ItemState = itemState.AVAILABLE;
            else
                GetComponentsInChildren<ShopItem>()[i].ItemState = itemState.UNPURCHASED;
        }
        initResourcesGain();
    }

    public void initResourcesGain()
    {
        //give init resouces
        EventManager.TriggerEvent("ResourcesChanged", new Dictionary<string, object>() { { "resources", 50 } });
    }

    public void OnDestroy()
    {
        EventManager.StopListening("ClearLaunch", ClearLaunch);
        EventManager.StopListening("ResourcesChanged", ResourcesChanged);
    }

    public void OnDisable()
    {
        EventManager.StopListening("ClearLaunch", ClearLaunch);
        EventManager.StopListening("ResourcesChanged", ResourcesChanged);
    }


    IEnumerator WaitForEventManager()
    {
        while (EventManager.Instance == null)
        {
            yield return new WaitForEndOfFrame();
        }
        EventManager.StartListening("ClearLaunch", ClearLaunch);
        EventManager.StartListening("ResourcesChanged", ResourcesChanged);

        initSetup();
    }

    private void ResourcesChanged(Dictionary<string, object> obj)
    {
        int resources = (int)obj["resources"];
        Resources += resources;

        if (Resources > ResourcesMax) Resources = ResourcesMax;
        if (resources == -9999999) Resources = 0;
        else if (Resources < 0) Resources = 0;
        ResourcesCount.text = Resources.ToString();
    }

    private void ClearLaunch(Dictionary<string, object> obj)
    {
        SelectedItem.ItemState = itemState.AVAILABLE;

        SelectedItem = null;
    }

    // Update is called once per frame
    void Update()
    {

    }
}
