using UnityEngine;

//scriptable object menu item
[CreateAssetMenu(fileName = "Info", menuName = "Info", order = 1)]
public class Info : ScriptableObject
{
    public string titleText;
    public string bodyText;
    public string footerText;
}
