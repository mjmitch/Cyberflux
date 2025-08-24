using UnityEngine;
using UnityEngine.UI;

public class StarfieldMover : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    public RawImage image;
    public float scrollSpeedX;
    public float scrollSpeedY;

    private Rect uvRect;

    void Start()
    {
        if (image != null)
        {
            uvRect = image.uvRect;
        }
    }

    void Update()
    {
        if (image == null) return;

        // Move UVs
        uvRect.x += scrollSpeedX * Time.unscaledDeltaTime;
        uvRect.y += scrollSpeedY * Time.unscaledDeltaTime;

        // Loop forever by wrapping values between 0–1
        if (uvRect.x > 1) uvRect.x -= 1;
        if (uvRect.x < -1) uvRect.x += 1;
        if (uvRect.y > 1) uvRect.y -= 1;
        if (uvRect.y < -1) uvRect.y += 1;

        image.uvRect = uvRect;
    }
}
