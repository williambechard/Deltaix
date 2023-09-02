using UnityEngine;

public class EventTrigger : MonoBehaviour
{
    public void TriggerEvent(string str)
    {
        EventManager.TriggerEvent(str, null);
    }
}
