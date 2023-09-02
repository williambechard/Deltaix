using UnityEngine;

public class RandomRotation : MonoBehaviour
{
    public float minRotationSpeed = 10.0f;
    public float maxRotationSpeed = 50.0f;

    private float rotationSpeed;

    private void Start()
    {
        rotationSpeed = Random.Range(minRotationSpeed, maxRotationSpeed);
    }

    private void Update()
    {
        transform.Rotate(0f, 0f, rotationSpeed * Time.deltaTime);
    }
}
