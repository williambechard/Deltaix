using System.Collections.Generic;
using UnityEngine;

public class ChangeColorOnMouseOver : MonoBehaviour
{
    public SpriteRenderer spriteRenderer;
    private Color originalColor;

    public PauseHandler PH;


    private void Start()
    {
        PH = GetComponent<PauseHandler>();
        originalColor = spriteRenderer.color;
    }




    private void OnMouseEnter()
    {
        if (!PH.isPaused)
        {
            spriteRenderer.color = new Color(spriteRenderer.color.r, spriteRenderer.color.g, spriteRenderer.color.b, spriteRenderer.color.a + .1f);
            EventManager.TriggerEvent("OrbitInfoChange", new Dictionary<string, object> { { "orbit", name }, { "show", true } });
        }

    }

    private void OnMouseExit()
    {
        if (!PH.isPaused)
        {
            spriteRenderer.color = originalColor;
            EventManager.TriggerEvent("OrbitInfoChange", new Dictionary<string, object> { { "orbit", name }, { "show", false } });
        }
    }
}
