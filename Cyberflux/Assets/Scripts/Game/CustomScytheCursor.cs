using UnityEngine;

public class CustomScytheCursor : MonoBehaviour
{
    [Header("Cursor Settings")]
    public Texture2D cursorTexture;
    public Vector2 hotspot = Vector2.zero;
    public CursorMode cursorMode = CursorMode.Auto;

    void Start()
    {
        // Apply your scythe cursor when the scene starts
        Cursor.visible = true;
        Cursor.lockState = CursorLockMode.None;
        Cursor.SetCursor(cursorTexture, hotspot, cursorMode);
    }

    // Optional helper methods:
    public void HideCursor()
    {
        Cursor.visible = false;
    }

    public void ShowCursor()
    {
        Cursor.visible = true;
        Cursor.SetCursor(cursorTexture, hotspot, cursorMode);
    }
}
