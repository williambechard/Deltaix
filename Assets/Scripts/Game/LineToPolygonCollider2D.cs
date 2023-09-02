using UnityEngine;


public class LineToPolygonCollider2D : MonoBehaviour
{
    public LineRenderer lineRenderer;
    public PolygonCollider2D polygonCollider;

    private void Start()
    {
        polygonCollider = GetComponentInParent<SatController>().radarTrigger as PolygonCollider2D;
        UpdatePolygonCollider();

    }

    private void UpdatePolygonCollider()
    {
        Vector2[] colliderPoints = new Vector2[lineRenderer.positionCount];

        for (int i = 0; i < lineRenderer.positionCount; i++)
        {
            colliderPoints[i] = lineRenderer.GetPosition(i);
        }

        polygonCollider.points = colliderPoints;
    }
}
