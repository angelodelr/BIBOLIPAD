using UnityEngine;

public class CameraZoom : MonoBehaviour
{
    public float zoomSpeed = 5f;       
    public float keyZoomSpeed = 2f;     
    public float minZoom = 3f;          
    public float maxZoom = 10f;         

    private Camera cam;

    void Start()
    {
        cam = GetComponent<Camera>();
        if (cam == null)
        {
            Debug.LogError("CameraZoom script must be attached to a Camera!");
        }
    }

    void Update()
    {
        float zoomChange = 0f;

        float scroll = Input.GetAxis("Mouse ScrollWheel");
        zoomChange -= scroll * zoomSpeed;

        if (Input.GetKey(KeyCode.I))
        {
            zoomChange -= keyZoomSpeed * Time.deltaTime;
        }
        if (Input.GetKey(KeyCode.O))
        {
            zoomChange += keyZoomSpeed * Time.deltaTime;
        }

        if (zoomChange != 0f)
        {
            cam.orthographicSize += zoomChange;
            cam.orthographicSize = Mathf.Clamp(cam.orthographicSize, minZoom, maxZoom);
        }
    }
}
