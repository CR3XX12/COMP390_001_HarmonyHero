using UnityEngine;

public class UIButtonOrbit : MonoBehaviour
{
    public Transform centerPoint; // The object to orbit around (e.g., player)
    public float radius = 100f;   // Distance from the center
    public float speed = 30f;     // Rotation speed in degrees per second
    public float angleOffset = 0f; // Offset so buttons don’t overlap

    private RectTransform rectTransform;
    private float angle;

    void Start()
    {
        rectTransform = GetComponent<RectTransform>();
        angle = angleOffset;
    }

    void Update()
    {
        if (centerPoint == null) return;

        angle += speed * Time.deltaTime;
        float radians = angle * Mathf.Deg2Rad;

        // Calculate new position
        Vector3 offset = new Vector3(Mathf.Cos(radians), Mathf.Sin(radians), 0) * radius;
        rectTransform.position = centerPoint.position + offset;
    }
}
