using UnityEngine;

public class MinimapToggle : MonoBehaviour
{
    public KeyCode toggleKey = KeyCode.M;
    private Camera minimapCam;
    private bool isExpanded = false;

    private Rect smallRect = new Rect(0.75f, 0.75f, 0.25f, 0.25f); 
    private Rect largeRect = new Rect(0f, 0f, 1f, 1f);         
    void Start()
    {
        minimapCam = GetComponent<Camera>();
        minimapCam.rect = smallRect;
    }

    void Update()
    {
        if (Input.GetKeyDown(toggleKey))
        {
            isExpanded = !isExpanded;
            minimapCam.rect = isExpanded ? largeRect : smallRect;
        }
    }
}
