using UnityEngine;

public class SetAngle : MonoBehaviour
{
    private Vector3 target;

    private void Start()
    {
        target = new Vector3(0, 0, 0); // Assuming the camera is your main camera
    }

    private void Update()
    {
        Vector3 direction = transform.position - target;
        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;

        transform.rotation = Quaternion.Euler(0, 0, angle);
    }
}
