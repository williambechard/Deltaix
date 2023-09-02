using UnityEngine;

public class OrbitClick : MonoBehaviour
{
    public float LaunchSpeed = 5.0f;
    public float Radius;
    private PauseHandler pauseHandler;
    private AllOrbitClick Parent;

    private void Start()
    {
        pauseHandler = GetComponent<PauseHandler>();
        Parent = GetComponentInParent<AllOrbitClick>();
        Radius = GetComponent<CircleCollider2D>().radius - 7.5f;

    }


    private void OnMouseDown()
    {
        if (pauseHandler == null) pauseHandler = GetComponent<PauseHandler>();
        if (!pauseHandler.isPaused)
            Parent.OnOrbitClick(this);
    }
}

