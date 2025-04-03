using Unity.Cinemachine;
using UnityEngine;

public class CameraInitializerScript : MonoBehaviour
{
    // PUBLIC
    private CinemachineConfiner2D cameraConfiner;
    
    // PRIVATE
    private PolygonCollider2D cameraBounds;

    void Start()
    {
        // Get confiner component
        cameraConfiner = GetComponent<CinemachineConfiner2D>();

        // Get camera bounds in scene
        cameraBounds = GameObject.Find("Camera Bounds").GetComponent<PolygonCollider2D>();

        // Update confiner bounds
        if (cameraConfiner != null && cameraBounds != null)
        {
            cameraConfiner.BoundingShape2D = cameraBounds;
            cameraConfiner.InvalidateBoundingShapeCache();
        }
    }
}