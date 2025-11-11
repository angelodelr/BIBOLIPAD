using UnityEngine;
using UnityEngine.UI;

public class InteractionUI : MonoBehaviour
{
    [Header("UI References")]
    public Canvas uiCanvas;
    public GameObject interactionPrompt;
    public Text interactionText;
    public Button interactionButton;
    public GameObject confirmationDialog;
    public Button yesButton;
    public Button noButton;
    
    private Camera mainCamera;
    private bool isMouseOverButton = false;
    private bool isConfirmationDialogActive = false;
    private float confirmationDialogOpenTime = 0f;
    private const float INPUT_DELAY = 0.2f; 
    
    void Start()
    {
        mainCamera = Camera.main;
        
        if (interactionButton != null)
        {
            interactionButton.onClick.AddListener(OnInteractionButtonClick);
        }
        
        if (yesButton != null)
        {
            yesButton.onClick.AddListener(OnYesButtonClick);
        }
        
        if (noButton != null)
        {
            noButton.onClick.AddListener(OnNoButtonClick);
        }
        HideInteractionPrompt();
        HideConfirmationDialog();
    }
    
    void Update()
    {
        if (isConfirmationDialogActive && Time.time > confirmationDialogOpenTime + INPUT_DELAY)
        {
            if (Input.GetKeyDown(KeyCode.E))
            {
                OnYesButtonClick();
            }
            else if (Input.GetKeyDown(KeyCode.R))
            {
                OnNoButtonClick();
            }
        }
    }
    
    public void ShowInteractionPrompt(string text, Vector3 worldPosition)
    {
        if (interactionPrompt == null) return;
        
        interactionPrompt.SetActive(true);
        
        if (interactionText != null)
        {
            interactionText.text = text;
        }
        
        Vector3 screenPosition = mainCamera.WorldToScreenPoint(worldPosition + Vector3.up * 1.5f);
        interactionPrompt.transform.position = screenPosition;
    }
    
    public void HideInteractionPrompt()
    {
        if (interactionPrompt != null)
        {
            interactionPrompt.SetActive(false);
        }
    }
    
    public void ShowConfirmationDialog()
    {
        HideInteractionPrompt();
        
        if (confirmationDialog != null)
        {
            confirmationDialog.SetActive(true);
            isConfirmationDialogActive = true;
            confirmationDialogOpenTime = Time.time; 
        }
    }
    
    public void HideConfirmationDialog()
    {
        if (confirmationDialog != null)
        {
            confirmationDialog.SetActive(false);
            isConfirmationDialogActive = false;
        }
    }
    
    public bool IsMouseOverInteractionButton()
    {
        return isMouseOverButton;
    }
    
    public bool IsConfirmationDialogActive()
    {
        return isConfirmationDialogActive;
    }
    
    private void OnInteractionButtonClick()
    {
        ShowConfirmationDialog();
    }
    
    private void OnYesButtonClick()
    {
        Debug.Log("Yes button clicked!");
        HideConfirmationDialog();
    }
    
    private void OnNoButtonClick()
    {
        Debug.Log("No button clicked!");
        HideConfirmationDialog();
    }
    
    public void OnInteractionButtonEnter()
    {
        isMouseOverButton = true;
    }
    
    public void OnInteractionButtonExit()
    {
        isMouseOverButton = false;
    }
}