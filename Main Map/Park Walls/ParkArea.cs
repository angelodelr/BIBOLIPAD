using UnityEngine;

[RequireComponent(typeof(BoxCollider2D))]
public class ParkArea : MonoBehaviour
{
    private BoxCollider2D box;

    void Awake()
    {
        box = GetComponent<BoxCollider2D>();
    }

    public Rect GetWorldRect()
    {
        Vector3 worldCenter = box.transform.TransformPoint(box.offset);
        Vector3 worldSize = Vector3.Scale(box.size, box.transform.lossyScale);
        return new Rect(
            worldCenter.x - worldSize.x / 2f,
            worldCenter.y - worldSize.y / 2f,
            worldSize.x,
            worldSize.y
        );
    }
}
