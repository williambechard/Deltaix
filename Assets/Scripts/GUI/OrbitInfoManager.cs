using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OrbitInfoManager : MonoBehaviour
{

    public GameObject LEOInfo;
    public GameObject MEOInfo;
    public GameObject HEOInfo;

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
        EventManager.StartListening("OrbitInfoChange", OrbitInfoChange);
    }

    private void OnDestroy()
    {
        EventManager.StopListening("OrbitInfoChange", OrbitInfoChange);
    }

    private void OnDisable()
    {
        EventManager.StopListening("OrbitInfoChange", OrbitInfoChange);
    }

    public void OrbitInfoChange(Dictionary<string, object> obj)
    {
        string orbit = (string)obj["orbit"];
        bool showInfo = (bool)obj["show"];

        switch (orbit)
        {
            case "orbit 1":
                LEOInfo.SetActive(showInfo);
                break;
            case "orbit 2":
                MEOInfo.SetActive(showInfo);
                break;
            case "orbit 3":
                HEOInfo.SetActive(showInfo);
                break;
        }
    }

    // Update is called once per frame
    void Update()
    {

    }
}
