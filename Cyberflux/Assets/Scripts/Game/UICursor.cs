using UnityEngine;

public class UICursor : MonoBehaviour
{
    public RectTransform cursorImage;
    public float shakeDuration = 0.2f;   // how long it shakes
    public float shakeStrength = 10f;    // how strong the shake is

    private Vector3 originalPos;
    private float shakeTime;

    void Start()
    {
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.None;
        originalPos = cursorImage.localPosition;
    }

    void Update()
    {

        // force OS cursor hidden
        if (Cursor.visible) Cursor.visible = false;

        

        cursorImage.position = Input.mousePosition;

        // Handle shaking
        if (shakeTime > 0)
        {
            cursorImage.localPosition += (Vector3)Random.insideUnitCircle * shakeStrength;
            shakeTime -= Time.unscaledDeltaTime;
        }
    }

    // Call this when a button is pressed
    public void TriggerShake()
    {
        shakeTime = shakeDuration;
    }
}
