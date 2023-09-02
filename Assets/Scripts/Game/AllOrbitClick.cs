using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AllOrbitClick : MonoBehaviour
{
    public GameObject targetSatellite;
    private PlaySoundOneShot playSound;
    public GameObject repairSat;
    private PauseHandler pauseHandler;
    private void Start()
    {
        pauseHandler = GetComponent<PauseHandler>();
        playSound = GetComponent<PlaySoundOneShot>();
        StartCoroutine(WaitForEventManager());
    }

    private void OnDestroy()
    {
        EventManager.StopListening("LaunchReady", LaunchReady);
    }

    private void OnDisable()
    {
        EventManager.StopListening("LaunchReady", LaunchReady);
    }

    IEnumerator WaitForEventManager()
    {
        while (EventManager.Instance == null)
        {
            yield return new WaitForEndOfFrame();
        }
        EventManager.StartListening("LaunchReady", LaunchReady);
    }

    private void LaunchReady(Dictionary<string, object> obj)
    {

        Debug.Log("LaunchRead on AllOrbitClick");
        targetSatellite = ((ShopItem)obj["ShopItem"]).prefab;
    }


    public void OnOrbitClick(OrbitClick orbit)
    {
        if (targetSatellite != null && !pauseHandler.isPaused)
        {

            if (targetSatellite == repairSat)
            {

                if (orbit.name == "planetSprite")
                {
                    Instantiate(targetSatellite, new Vector3(0, 0, -10), Quaternion.identity);
                    targetSatellite = null;

                    EventManager.TriggerEvent("ClearLaunch", null);
                }
                else
                {
                    //wrong indicate to user
                    EventManager.TriggerEvent("Alert", new Dictionary<string, object>() { { "alert", "You cannot deploy this satellite to this orbit. You must select earth." } });
                }
            }
            else
            {
                if (orbit.name == "planetSprite")
                {
                    //wrong indicate to user
                    EventManager.TriggerEvent("Alert", new Dictionary<string, object>() { { "alert", "You cannot deploy this satellite to earth. You must select an orbit." } });
                }
                else
                {
                    AudioManager.Instance.PlaySFXOneShot(playSound.SoundFXName);
                    Vector2 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
                    //create the object
                    GameObject newObject = Instantiate(targetSatellite, new Vector3(0, 0, -10), Quaternion.identity);
                    LaunchSatellite launchObj = newObject.GetComponent<LaunchSatellite>();

                    // Calculate the direction from the object to the target
                    Vector3 directionToTarget = new Vector3(mousePosition.x, mousePosition.y, -12) - transform.position;

                    // Calculate the desired rotation angle in radians
                    float angle = Mathf.Atan2(directionToTarget.y, directionToTarget.x) * Mathf.Rad2Deg;
                    angle -= 45f;
                    // Set the rotation directly
                    launchObj.spriteObj.transform.rotation = Quaternion.Euler(new Vector3(0, 0, angle));


                    launchObj.tPosition = new Vector3(mousePosition.x, mousePosition.y, -12);
                    launchObj.duration = orbit.LaunchSpeed;
                    launchObj.radiusLimit = orbit.Radius;
                    launchObj.parent = orbit.gameObject;
                    targetSatellite = null;
                    EventManager.TriggerEvent("ClearLaunch", null);
                }
            }

        }
    }

}
