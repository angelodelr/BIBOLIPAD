using UnityEngine;

public class TestDoubleEFix
{
    public static void Execute()
    {
        Debug.Log("🔧 Testing Double E Key Fix:");
        Debug.Log("📝 Expected behavior:");
        Debug.Log("   1. Press E near object → Shows interaction prompt");
        Debug.Log("   2. Press E again → Shows confirmation dialog");
        Debug.Log("   3. Wait 0.2 seconds, then press E → Selects Yes");
        Debug.Log("   4. Or press R → Selects No");
        Debug.Log("");
        Debug.Log("✅ Fix implemented:");
        Debug.Log("   - Added 0.2 second delay after dialog opens");
        Debug.Log("   - E key input blocked during delay period");
        Debug.Log("   - Updated button text for clarity");
        
        InteractionUI ui = Object.FindFirstObjectByType<InteractionUI>();
        if (ui != null)
        {
            ui.ShowConfirmationDialog();
            Debug.Log("🎮 Confirmation dialog shown for testing!");
        }
    }
}