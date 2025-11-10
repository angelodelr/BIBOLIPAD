using UnityEngine;

public class TreeSpawner : MonoBehaviour
{
    [Header("Spawner Settings")]
    public ParkArea parkArea;
    public GameObject treePrefab;
    public int treeCount = 20;
    public bool spawnOnStart = true;

    void Start()
    {
        if (spawnOnStart)
            SpawnTrees();
    }

    public void SpawnTrees()
    {
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

        Vector2 worldCenter = box.transform.TransformPoint(box.offset);
        Vector2 worldSize = Vector2.Scale(box.size, box.transform.lossyScale);

        float halfWidth = worldSize.x / 2f;
        float halfHeight = worldSize.y / 2f;

        for (int i = 0; i < treeCount; i++)
        {
            // Random spawn (may initially be outside due to floating point)
            float x = Random.Range(worldCenter.x - halfWidth, worldCenter.x + halfWidth);
            float y = Random.Range(worldCenter.y - halfHeight, worldCenter.y + halfHeight);

            // Clamp to exact rectangle
            x = Mathf.Clamp(x, worldCenter.x - halfWidth, worldCenter.x + halfWidth);
            y = Mathf.Clamp(y, worldCenter.y - halfHeight, worldCenter.y + halfHeight);

            Vector2 pos = new Vector2(x, y);

            GameObject tree = Instantiate(treePrefab, pos, Quaternion.identity, transform);
            tree.name = $"Tree_{i}";
        }
    }
}
