using UnityEngine;

public class InteractableObject : MonoBehaviour
{
    [Header("Interaction Settings")]
    public float interactionRange = 2.0f;
    public KeyCode interactionKey = KeyCode.E;
    public string interactionText = "Press E to interact";
    
    [Header("References")]
    public Transform player;
    
    private InteractionUI interactionUI;
    private bool playerInRange = false;
    private bool isShowingPrompt = false;
    
    void Start()
    {
        if (player == null)
        {
            GameObject playerObj = GameObject.FindGameObjectWithTag("Player");
            if (playerObj != null)
                player = playerObj.transform;
        }
        
        interactionUI = FindObjectOfType<InteractionUI>();
        if (interactionUI == null)
        {
            Debug.LogWarning("InteractionUI not found in scene!");
        }
    }
    
    void Update()
    {
        if (player == null) return;
        
        float distance = Vector2.Distance(transform.position, player.position);
        bool inRange = distance <= interactionRange;
        
        if (inRange && !playerInRange)
        {
            OnPlayerEnterRange();
        }
        else if (!inRange && playerInRange)
        {
            OnPlayerExitRange();
        }
        
        playerInRange = inRange;
        
        if (playerInRange && isShowingPrompt && !IsConfirmationDialogActive())
        {
            if (Input.GetKeyDown(interactionKey) || (Input.GetMouseButtonDown(0) && IsMouseOverInteractionButton()))
            {
                OnInteract();
            }
        }
    }
    
    private void OnPlayerEnterRange()
    {
        if (interactionUI != null)
        {
            interactionUI.ShowInteractionPrompt(interactionText, transform.position);
            isShowingPrompt = true;
        }
    }
    
    private void OnPlayerExitRange()
    {
        if (interactionUI != null)
        {
            interactionUI.HideInteractionPrompt();
            interactionUI.HideConfirmationDialog();
            isShowingPrompt = false;
        }
    }
    
    private void OnInteract()
    {
        if (interactionUI != null)
        {
            interactionUI.ShowConfirmationDialog();
        }
    }
    
    private bool IsMouseOverInteractionButton()
    {
        if (interactionUI != null)
        {
            return interactionUI.IsMouseOverInteractionButton();
        }
        return false;
    }
    
    private bool IsConfirmationDialogActive()
    {
        if (interactionUI != null)
        {
            return interactionUI.IsConfirmationDialogActive();
        }
        return false;
    }
    
    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, interactionRange);
    }
}