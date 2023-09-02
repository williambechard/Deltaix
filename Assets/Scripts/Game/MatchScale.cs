using UnityEngine;

public class MatchScale : MonoBehaviour
{
    private Transform parentTransform;

    private void Start()
    {
        parentTransform = transform.parent;

        if (parentTransform == null)
        {
            Debug.LogWarning("MatchScale script requires a parent object with a transform.");
            enabled = false; // Disable the script if there's no parent transform.
            return;
        }

        transform.localScale = parentTransform.localScale;
    }
}
