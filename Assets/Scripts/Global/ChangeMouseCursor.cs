using UnityEngine;

public class ChangeMouseCursor : MonoBehaviour
{
    public Texture2D customCursor; // Assign your custom cursor texture here

    void Start()
    {
        // Set the custom cursor as the active cursor
        Cursor.SetCursor(customCursor, Vector2.zero, CursorMode.Auto);
    }

    void OnDisable()
    {
        // Reset the cursor to the default cursor when the script is disabled
        Cursor.SetCursor(null, Vector2.zero, CursorMode.Auto);
    }
}