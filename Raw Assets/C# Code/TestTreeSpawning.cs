using UnityEngine;

public class TestTreeSpawning
{
    public static void Execute()
    {
        TreeSpawner spawner = Object.FindObjectOfType<TreeSpawner>();
        if (spawner != null)
        {
            spawner.SpawnTrees();
            Debug.Log("Trees respawned!");
        }
        else
        {
            Debug.LogError("No TreeSpawner found in scene!");
        }
    }
}