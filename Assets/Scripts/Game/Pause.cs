using System.Collections.Generic;
using UnityEngine;

public class Pause : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        EventManager.TriggerEvent("Paused", new Dictionary<string, object> { { "paused", true } });
    }

    public void Reset()
    {
        EventManager.TriggerEvent("ResourcesChanged", new Dictionary<string, object>() { { "resources", -9999999 } });
    }
}
