using UnityEngine;

public class TestKeyboardShortcuts
{
    public static void Execute()
    {
        InteractionUI ui = Object.FindFirstObjectByType<InteractionUI>();
        if (ui != null)
        {
            Debug.Log("✅ Testing keyboard shortcuts...");
            
            ui.ShowConfirmationDialog();
            
            Debug.Log("🎮 Confirmation dialog shown!");
            Debug.Log("📝 Instructions:");
            Debug.Log("   - Press E to select Yes");
            Debug.Log("   - Press R to select No");
            Debug.Log("   - Or click the buttons with mouse");
        }
        else
        {
            Debug.LogError("❌ InteractionUI not found!");
        }
    }
}