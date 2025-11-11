using UnityEngine;

public class TestEAndRKeys
{
    public static void Execute()
    {
        Debug.Log("🔧 Testing E and R Key Fix:");
        Debug.Log("📝 Expected behavior:");
        Debug.Log("   1. InteractableObject stops listening to E when dialog is open");
        Debug.Log("   2. InteractionUI handles E and R keys for confirmation");
        Debug.Log("   3. Both keys should work after 0.2 second delay");
        Debug.Log("");
        
        InteractionUI ui = Object.FindFirstObjectByType<InteractionUI>();
        if (ui != null)
        {
            ui.ShowConfirmationDialog();
            Debug.Log("🎮 Confirmation dialog shown!");
            Debug.Log("✅ Dialog state: " + ui.IsConfirmationDialogActive());
            Debug.Log("⏱️ Wait 0.2 seconds, then:");
            Debug.Log("   - Press E → Should select Yes");
            Debug.Log("   - Press R → Should select No");
        }
        else
        {
            Debug.LogError("❌ InteractionUI not found!");
        }
        
        InteractableObject interactable = Object.FindFirstObjectByType<InteractableObject>();
        if (interactable != null)
        {
            Debug.Log("✅ InteractableObject found - should ignore E when dialog is active");
        }
        else
        {
            Debug.LogError("❌ InteractableObject not found!");
        }
    }
}