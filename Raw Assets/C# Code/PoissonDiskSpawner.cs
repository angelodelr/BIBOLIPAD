using System.Collections.Generic;
using UnityEngine;

public class ParkObjectSpawner : MonoBehaviour
{
    [Header("Spawn Settings")]
    public GameObject prefab;     
    public int numPoints = 50;
    public float radius = 1f;       
    public int maxAttempts = 30;

    [Header("Park Bounds")]
    public Transform park;           
    public float xRadius = 5f;     
    public float yRadius = 3f;       

    private List<Vector2> points = new List<Vector2>();

    void Start()
    {
        GeneratePoints();
        foreach (Vector2 p in points)
        {
            GameObject obj = Instantiate(prefab, p, Quaternion.identity, transform);
            obj.name = "ParkObject";

            ChargeRepulsionOval charge = obj.AddComponent<ChargeRepulsionOval>();
            charge.parkBox = park;
            charge.xRadius = xRadius;
            charge.yRadius = yRadius;
            charge.charge = 10f;
        }
    }

    void GeneratePoints()
    {
        points.Clear();
        List<Vector2> activeList = new List<Vector2>();

        Vector2 first = (Vector2)park.position + RandomPointInEllipse();
        points.Add(first);
        activeList.Add(first);

        while (activeList.Count > 0 && points.Count < numPoints)
        {
            int idx = Random.Range(0, activeList.Count);
            Vector2 point = activeList[idx];
            bool found = false;

            for (int i = 0; i < maxAttempts; i++)
            {
                float angle = Random.Range(0f, Mathf.PI * 2f);
                float distance = Random.Range(radius, 2f * radius);
                Vector2 candidate = point + new Vector2(Mathf.Cos(angle), Mathf.Sin(angle)) * distance;

                if (IsInsideEllipse(candidate) && IsFarEnough(candidate))
                {
                    points.Add(candidate);
                    activeList.Add(candidate);
                    found = true;
                    break;
                }
            }

            if (!found)
                activeList.RemoveAt(idx);
        }
    }

    Vector2 RandomPointInEllipse()
    {
        float t = 2 * Mathf.PI * Random.value;
        float u = Random.value + Random.value;
        float r = (u > 1) ? 2 - u : u;
        return new Vector2(r * Mathf.Cos(t) * xRadius, r * Mathf.Sin(t) * yRadius);
    }

    bool IsInsideEllipse(Vector2 point)
    {
        Vector2 local = point - (Vector2)park.position;
        float value = (local.x * local.x) / (xRadius * xRadius) + (local.y * local.y) / (yRadius * yRadius);
        return value <= 1f;
    }

    bool IsFarEnough(Vector2 candidate)
    {
        foreach (Vector2 p in points)
            if (Vector2.Distance(candidate, p) < radius)
                return false;
        return true;
    }
}
