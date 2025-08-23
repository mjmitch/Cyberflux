using UnityEngine;
using UnityEngine.UI;

public class StarfieldScroller : MonoBehaviour
{

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
        uvRect.x += scrollSpeedX * Time.deltaTime;
        uvRect.y += scrollSpeedY * Time.deltaTime;

        // Loop forever by wrapping values between 0–1
        if (uvRect.x > 1) uvRect.x -= 1;
        if (uvRect.x < -1) uvRect.x += 1;
        if (uvRect.y > 1) uvRect.y -= 1;
        if (uvRect.y < -1) uvRect.y += 1;

        image.uvRect = uvRect;
    }
}
