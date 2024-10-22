using UnityEngine;

public class CameraBounds : MonoBehaviour
{
    public Camera mainCamera;
    private void Start()
    {
        
    }

    // Returns the left bound of the camera in world space
    public Vector3 LeftBound()
    {
        Vector3 leftBound = mainCamera.ViewportToWorldPoint(new Vector3(0, 0.5f, mainCamera.nearClipPlane));
        return leftBound;
    }

    // Returns the right bound of the camera in world space
    public Vector3 RightBound()
    {
        Vector3 rightBound = mainCamera.ViewportToWorldPoint(new Vector3(1, 0.5f, mainCamera.nearClipPlane));
        return rightBound;
    }

    // Returns the top bound of the camera in world space
    public Vector3 TopBound()
    {
        Vector3 topBound = mainCamera.ViewportToWorldPoint(new Vector3(0.5f, 1, mainCamera.nearClipPlane));
        return topBound;
    }

    // Returns the bottom bound of the camera in world space
    public Vector3 BottomBound()
    {
        Vector3 bottomBound = mainCamera.ViewportToWorldPoint(new Vector3(0.5f, 0, mainCamera.nearClipPlane));
        return bottomBound;
    }
}