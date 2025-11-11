using UnityEngine;
using System.Collections.Generic;

public class TreeSpawner : MonoBehaviour
{
    [Header("Spawner Settings")]
    public ParkArea parkArea;
    public GameObject treePrefab;
    public int treeCount = 20;
    public float minDistanceBetweenTrees = 2.0f;
    public float borderMargin = 1.0f; 
    public bool spawnOnStart = true;
    
    [Header("Debug")]
    public bool showDebugGizmos = true;

    private List<Vector2> spawnedPositions = new List<Vector2>();

    void Start()
    {
        if (spawnOnStart)
            SpawnTrees();
    }

    public void SpawnTrees()
    {
        ClearExistingTrees();
        
        if (parkArea == null || treePrefab == null)
        {
            Debug.LogWarning("TreeSpawner: Missing park area or tree prefab!");
            return;
        }

        BoxCollider2D box = parkArea.GetComponent<BoxCollider2D>();
        if (box == null)
        {
            Debug.LogError("TreeSpawner: ParkArea has no BoxCollider2D!");
            return;
        }

        Rect parkRect = GetParkBounds(box);
        
        List<Vector2> positions = GeneratePoissonDiskSampling(parkRect, minDistanceBetweenTrees, treeCount);
        
        for (int i = 0; i < positions.Count; i++)
        {
            GameObject tree = Instantiate(treePrefab, positions[i], Quaternion.identity, transform);
            tree.name = $"Tree";
        }
        
        spawnedPositions = positions;
        Debug.Log($"TreeSpawner: Spawned {positions.Count} trees out of {treeCount} requested.");
    }
    
    private void ClearExistingTrees()
    {
        for (int i = transform.childCount - 1; i >= 0; i--)
        {
            if (Application.isPlaying)
                Destroy(transform.GetChild(i).gameObject);
            else
                DestroyImmediate(transform.GetChild(i).gameObject);
        }
        spawnedPositions.Clear();
    }
    
    private Rect GetParkBounds(BoxCollider2D box)
    {
        Vector2 worldCenter = box.transform.TransformPoint(box.offset);
        Vector2 worldSize = Vector2.Scale(box.size, box.transform.lossyScale);
        
        float width = worldSize.x - (borderMargin * 2);
        float height = worldSize.y - (borderMargin * 2);
        
        return new Rect(
            worldCenter.x - width / 2f,
            worldCenter.y - height / 2f,
            width,
            height
        );
    }
    
    private List<Vector2> GeneratePoissonDiskSampling(Rect bounds, float minDistance, int maxSamples)
    {
        List<Vector2> points = new List<Vector2>();
        List<Vector2> activeList = new List<Vector2>();
        
        float cellSize = minDistance / Mathf.Sqrt(2);
        int gridWidth = Mathf.CeilToInt(bounds.width / cellSize);
        int gridHeight = Mathf.CeilToInt(bounds.height / cellSize);
        Vector2[,] grid = new Vector2[gridWidth, gridHeight];
        
        for (int x = 0; x < gridWidth; x++)
        {
            for (int y = 0; y < gridHeight; y++)
            {
                grid[x, y] = Vector2.negativeInfinity;
            }
        }
        
        Vector2 firstPoint = new Vector2(
            Random.Range(bounds.xMin, bounds.xMax),
            Random.Range(bounds.yMin, bounds.yMax)
        );
        
        points.Add(firstPoint);
        activeList.Add(firstPoint);
        
        Vector2Int gridPos = WorldToGrid(firstPoint, bounds, cellSize);
        if (IsValidGridPosition(gridPos, gridWidth, gridHeight))
        {
            grid[gridPos.x, gridPos.y] = firstPoint;
        }
        
        int attempts = 0;
        int maxAttempts = maxSamples * 30; 
        
        while (activeList.Count > 0 && points.Count < maxSamples && attempts < maxAttempts)
        {
            attempts++;
            int randomIndex = Random.Range(0, activeList.Count);
            Vector2 point = activeList[randomIndex];
            bool found = false;
            
            for (int i = 0; i < 30; i++) 
            {
                float angle = Random.Range(0f, 2f * Mathf.PI);
                float distance = Random.Range(minDistance, 2f * minDistance);
                Vector2 newPoint = point + new Vector2(Mathf.Cos(angle), Mathf.Sin(angle)) * distance;
                
                if (IsValidPoint(newPoint, bounds, grid, gridWidth, gridHeight, cellSize, minDistance))
                {
                    points.Add(newPoint);
                    activeList.Add(newPoint);
                    
                    Vector2Int newGridPos = WorldToGrid(newPoint, bounds, cellSize);
                    if (IsValidGridPosition(newGridPos, gridWidth, gridHeight))
                    {
                        grid[newGridPos.x, newGridPos.y] = newPoint;
                    }
                    
                    found = true;
                    break;
                }
            }
            
            if (!found)
            {
                activeList.RemoveAt(randomIndex);
            }
        }
        
        return points;
    }
    
    private Vector2Int WorldToGrid(Vector2 worldPos, Rect bounds, float cellSize)
    {
        return new Vector2Int(
            Mathf.FloorToInt((worldPos.x - bounds.xMin) / cellSize),
            Mathf.FloorToInt((worldPos.y - bounds.yMin) / cellSize)
        );
    }
    
    private bool IsValidGridPosition(Vector2Int gridPos, int gridWidth, int gridHeight)
    {
        return gridPos.x >= 0 && gridPos.x < gridWidth && gridPos.y >= 0 && gridPos.y < gridHeight;
    }
    
    private bool IsValidPoint(Vector2 point, Rect bounds, Vector2[,] grid, int gridWidth, int gridHeight, float cellSize, float minDistance)
    {
        if (!bounds.Contains(point))
            return false;
        
        Vector2Int gridPos = WorldToGrid(point, bounds, cellSize);
        
        for (int x = Mathf.Max(0, gridPos.x - 2); x <= Mathf.Min(gridWidth - 1, gridPos.x + 2); x++)
        {
            for (int y = Mathf.Max(0, gridPos.y - 2); y <= Mathf.Min(gridHeight - 1, gridPos.y + 2); y++)
            {
                Vector2 neighbor = grid[x, y];
                if (neighbor != Vector2.negativeInfinity)
                {
                    float distance = Vector2.Distance(point, neighbor);
                    if (distance < minDistance)
                        return false;
                }
            }
        }
        
        return true;
    }
    
    void OnDrawGizmos()
    {
        if (!showDebugGizmos || parkArea == null) return;
        
        BoxCollider2D box = parkArea.GetComponent<BoxCollider2D>();
        if (box == null) return;
        
        Rect parkRect = GetParkBounds(box);
        Gizmos.color = Color.green;
        Gizmos.DrawWireCube(parkRect.center, parkRect.size);
        

        Gizmos.color = Color.red;
        foreach (Vector2 pos in spawnedPositions)
        {
            Gizmos.DrawWireSphere(pos, minDistanceBetweenTrees / 2f);
        }
    }
}
