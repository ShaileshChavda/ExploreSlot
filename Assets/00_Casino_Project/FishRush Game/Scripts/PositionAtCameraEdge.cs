using UnityEngine;

public class PositionAtCameraEdge : MonoBehaviour
{
    public Camera mainCamera;
    public float distanceFromEdgeX = 0.1f; // Distance from the camera's edge
    public float distanceFromEdgeY = 0.1f; // Distance from the camera's edge

    private SpriteRenderer spriteRenderer;
    private CameraBounds cameraBounds;

    private void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        cameraBounds = mainCamera.GetComponent<CameraBounds>();

        if (spriteRenderer == null || cameraBounds == null || mainCamera == null)
        {
            Debug.LogError("Make sure to assign the required components in the inspector.");
            enabled = false; // Disable the script if components are not properly assigned
        }
        else
        {
            PositionAtEdge();
        }
    }

    private void PositionAtEdge()
    {
        Vector2 spriteSize = spriteRenderer.bounds.size;
        Vector2 viewportPosition = new Vector2(1, 1); // Default position (top-right corner)

        // Choose the edge closest to the sprite's current position
        if (transform.position.x < cameraBounds.LeftBound().x)
        {
            viewportPosition.x = 0; // Left edge
        }
        else if (transform.position.x > cameraBounds.RightBound().x)
        {
            viewportPosition.x = 1; // Right edge
        }

        if (transform.position.y < cameraBounds.BottomBound().y)
        {
            viewportPosition.y = 0; // Bottom edge
        }
        else if (transform.position.y > cameraBounds.TopBound().y)
        {
            viewportPosition.y = 1; // Top edge
        }

        // Convert viewport position to world space
        Vector3 worldPosition = mainCamera.ViewportToWorldPoint(viewportPosition);

        // Adjust position considering the sprite's size and the distance from the edge
        worldPosition.x += (spriteSize.x / 2 + distanceFromEdgeX) * (viewportPosition.x == 0 ? 1 : -1);
        worldPosition.y += (spriteSize.y / 2 + distanceFromEdgeY) * (viewportPosition.y == 0 ? 1 : -1);

        transform.position = new Vector3(worldPosition.x, worldPosition.y,0f);
    }
}