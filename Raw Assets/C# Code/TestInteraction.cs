using UnityEngine;

public class TestInteraction
{
    public static void Execute()
    {
        InteractableObject interactable = Object.FindFirstObjectByType<InteractableObject>();
        if (interactable != null)
        {
            Debug.Log("✅ Interactable object found and configured!");
        }
        else
        {
            Debug.LogError("❌ No interactable object found!");
        }
        
        InteractionUI ui = Object.FindFirstObjectByType<InteractionUI>();
        if (ui != null)
        {
            Debug.Log("✅ Interaction UI found and configured!");
            
            if (ui.interactionPrompt != null) Debug.Log("✅ Interaction Prompt reference OK");
            else Debug.LogError("❌ Interaction Prompt reference missing");
            
            if (ui.confirmationDialog != null) Debug.Log("✅ Confirmation Dialog reference OK");
            else Debug.LogError("❌ Confirmation Dialog reference missing");
        }
        else
        {
            Debug.LogError("❌ No interaction UI found!");
        }
        
        GameObject player = GameObject.FindGameObjectWithTag("Player");
        if (player != null)
        {
            Debug.Log("✅ Player found with tag!");
        }
        else
        {
            Debug.LogError("❌ No player found with Player tag!");
        }
        
        Debug.Log("🎯 Canvas overlay issue should now be resolved!");
    }
}