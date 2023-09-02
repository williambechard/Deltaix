using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UserAlert : MonoBehaviour
{
    public TextMeshProUGUI alertBanner;
    public Image alertImage;
    public float showAlertDuration = 3.0f; // Duration to show the alert banner
    public PlaySoundOneShot playSound;

    // Start is called before the first frame update
    void Start()
    {
        playSound = GetComponent<PlaySoundOneShot>();
        StartCoroutine(WaitForEventManager());
    }


    IEnumerator WaitForEventManager()
    {
        while (EventManager.Instance == null)
        {
            yield return new WaitForEndOfFrame();
        }
        EventManager.StartListening("Alert", Alert);
    }

    IEnumerator ShowAlert(string txt)
    {
        playSound.playSound();
        // Set the text of the alert banner
        alertBanner.text = txt;

        // Show the alert banner
        alertBanner.gameObject.SetActive(true);
        alertImage.gameObject.SetActive(true);
        // Wait for the specified duration
        yield return new WaitForSeconds(showAlertDuration);

        // Hide the alert banner
        alertBanner.gameObject.SetActive(false);
        alertImage.gameObject.SetActive(false);
        alertBanner.text = "";
    }

    private void Alert(Dictionary<string, object> obj)
    {
        string alert = (string)obj["alert"];
        StopAllCoroutines();
        StartCoroutine(ShowAlert(alert));
    }

    // Update is called once per frame
    void Update()
    {

    }
}
