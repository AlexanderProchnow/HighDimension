    using UnityEngine;

public class Pointer : MonoBehaviour
{
    [SerializeField] protected float defaultLength = 3.0f;

    private LineRenderer lineRenderer = null;

    private void Awake()
    {
        lineRenderer = GetComponent<LineRenderer>();
    }

    private void Update()
    {
        UpdateLength();
    }

    private void UpdateLength()
    { 
        lineRenderer.SetPosition(0, transform.position);
        lineRenderer.SetPosition(1, GetEnd());
    }

    protected virtual Vector3 GetEnd()
    {
        RaycastHit hit = CreateForwardRaycast();
        Vector3 endPosition = CalculateDefaultEnd(defaultLength);

        if (hit.collider) {
            endPosition = hit.point;
        }

        return endPosition;
    }

    private RaycastHit CreateForwardRaycast() {
        RaycastHit hit;
        Ray ray = new Ray(transform.position, transform.forward);

        Physics.Raycast(ray, out hit, defaultLength);
        return hit;
    }

    protected Vector3 CalculateDefaultEnd(float length)
    {
        return transform.position + (transform.forward * length);
    }
}
