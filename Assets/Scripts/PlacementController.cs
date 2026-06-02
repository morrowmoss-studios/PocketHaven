using UnityEngine;
using UnityEngine.InputSystem;

// First slice placement: pick up a test decoration, drag it with the
// pointer, drop it, snap to the grid, and bake its sort order by y.
// Pointer.current covers mouse and touch with one path, which is the
// shared input model we want. Catalog selection, layers, save/load, and
// the mobile nudge mode all come later and bolt onto this.
public class PlacementController : MonoBehaviour
{
    [Header("Test Setup")]
    [Tooltip("The decoration to place while we test. Later this comes from catalog selection.")]
    public DecorationData testDecoration;

    [Header("Grid")]
    [Tooltip("World units per grid cell. Bigger = coarser snapping.")]
    public float cellSize = 0.5f;

    [Tooltip("Your scene camera. Falls back to Camera.main if left empty.")]
    public Camera sceneCamera;

    // The item being dragged before it gets dropped.
    private GameObject heldItem;
    private SpriteRenderer heldRenderer;

    private void Awake()
    {
        if (sceneCamera == null)
        {
            sceneCamera = Camera.main;
        }
    }

    private void Update()
    {
        // No pointer device, nothing to do.
        if (Pointer.current == null)
        {
            return;
        }

        Vector2 screenPos = Pointer.current.position.ReadValue();
        Vector3 worldPos = ScreenToWorld(screenPos);

        // Press: start dragging a fresh copy of the test decoration.
        if (Pointer.current.press.wasPressedThisFrame)
        {
            BeginPlacement(worldPos);
        }

        // Hold: the held item follows the pointer unsnapped, so it feels
        // responsive while moving.
        if (heldItem != null && Pointer.current.press.isPressed)
        {
            heldItem.transform.position = worldPos;
        }

        // Release: snap to grid, bake sort order, let go.
        if (heldItem != null && Pointer.current.press.wasReleasedThisFrame)
        {
            DropPlacement(worldPos);
        }
    }

    private void BeginPlacement(Vector3 worldPos)
    {
        if (testDecoration == null || testDecoration.sprite == null)
        {
            return;
        }

        heldItem = new GameObject(testDecoration.displayName);
        heldRenderer = heldItem.AddComponent<SpriteRenderer>();
        heldRenderer.sprite = testDecoration.sprite;

        // Keep the dragging item on top so it stays visible while moving.
        heldRenderer.sortingOrder = 9999;
        heldItem.transform.position = worldPos;
    }

    private void DropPlacement(Vector3 worldPos)
    {
        Vector3 snapped = SnapToGrid(worldPos);
        heldItem.transform.position = snapped;

        // Sort on place: lower on screen (smaller y) draws in front. We
        // compute this once, on drop, since placed items do not move after.
        // Scaled by 100 to leave headroom between items.
        heldRenderer.sortingOrder = Mathf.RoundToInt(-snapped.y * 100f);

        heldItem = null;
        heldRenderer = null;
    }

    private Vector3 ScreenToWorld(Vector2 screenPos)
    {
        // Orthographic camera, so we pass the camera distance and flatten z.
        float distance = Mathf.Abs(sceneCamera.transform.position.z);
        Vector3 world = sceneCamera.ScreenToWorldPoint(new Vector3(screenPos.x, screenPos.y, distance));
        world.z = 0f;
        return world;
    }

    private Vector3 SnapToGrid(Vector3 worldPos)
    {
        // Basic square-grid snap for now. Iso-accurate diamond snapping is
        // a later refinement once we have a real floor to align to.
        float x = Mathf.Round(worldPos.x / cellSize) * cellSize;
        float y = Mathf.Round(worldPos.y / cellSize) * cellSize;
        return new Vector3(x, y, 0f);
    }
}