using UnityEditor;
using UnityEngine;

public class CursorShow : MonoBehaviour
{

#if UNITY_EDITOR
    private void Start()
    {
        Cursor.visible = true;
        Cursor.SetCursor(PlayerSettings.defaultCursor, Vector2.zero, CursorMode.ForceSoftware);
    }

#endif
}

