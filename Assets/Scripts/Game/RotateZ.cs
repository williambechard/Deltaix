using UnityEngine;

public class RotateZ : MonoBehaviour
{
    public bool randomRotate = false;
    public float minRotationSpeed = 10.0f;
    public float maxRotationSpeed = 50.0f;
    private PauseHandler pauseHandler;
    public float rotationSpeed;

    private void Start()
    {

        if (randomRotate)
            rotationSpeed = Random.Range(minRotationSpeed, maxRotationSpeed);
        else rotationSpeed = minRotationSpeed;
    }

    private void Update()
    {

        transform.Rotate(0f, 0f, rotationSpeed * Time.deltaTime);
    }
}
