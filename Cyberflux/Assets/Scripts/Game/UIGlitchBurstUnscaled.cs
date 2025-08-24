using UnityEngine;

public class UIGlitchBurstUnscaled : MonoBehaviour
{
    [Header("Targets")]
    public RectTransform target;  // main (AIEye)
    public RectTransform redRT;   // AIEye_R
    public RectTransform blueRT;  // AIEye_B

    [Header("Behavior")]
    public bool autoOnEnable = true;
    public float duration = 0.15f;    // total burst time
    public float maxPosJitter = 4f;   // px
    public float maxScaleJitter = 0.08f; // +/- percent
    public float maxRotJitter = 3f;   // degrees
    public float rgbOffset = 2f;      // base split (px)

    Vector2 t0, r0, b0;
    Vector3 s0;
    float z0;
    float t;
    bool active;

    void Awake()
    {
        if (!target) target = GetComponent<RectTransform>();
        if (!redRT || !blueRT) return;
        t0 = target.anchoredPosition;
        r0 = redRT.anchoredPosition;
        b0 = blueRT.anchoredPosition;
        s0 = target.localScale;
        z0 = target.localRotation.eulerAngles.z;
    }

    void OnEnable()
    {
        if (autoOnEnable) Trigger();
    }

    public void Trigger()
    {
        if (!target || !redRT || !blueRT) return;
        // reset
        target.anchoredPosition = t0;
        redRT.anchoredPosition = r0;
        blueRT.anchoredPosition = b0;
        target.localScale = s0;
        target.localRotation = Quaternion.Euler(0, 0, z0);
        t = 0f;
        active = true;
    }

    void Update()
    {
        if (!active) return;
        t += Time.unscaledDeltaTime;
        float p = Mathf.Clamp01(t / duration);
        // stronger at start, easing out
        float strength = 1f - p;

        // main jitter
        Vector2 jitter = new Vector2(
            Random.Range(-maxPosJitter, maxPosJitter),
            Random.Range(-maxPosJitter, maxPosJitter)
        ) * strength;

        float scaleJ = 1f + Random.Range(-maxScaleJitter, maxScaleJitter) * strength;
        float rotJ = Random.Range(-maxRotJitter, maxRotJitter) * strength;

        target.anchoredPosition = t0 + jitter;
        target.localScale = new Vector3(s0.x * scaleJ, s0.y * scaleJ, 1f);
        target.localRotation = Quaternion.Euler(0, 0, z0 + rotJ);

        // RGB split jitter
        Vector2 off = new Vector2(
            Random.Range(-rgbOffset, rgbOffset),
            Random.Range(-rgbOffset, rgbOffset)
        ) * strength;

        redRT.anchoredPosition = r0 + off;
        blueRT.anchoredPosition = b0 - off;

        if (t >= duration)
        {
            // restore
            target.anchoredPosition = t0;
            redRT.anchoredPosition = r0;
            blueRT.anchoredPosition = b0;
            target.localScale = s0;
            target.localRotation = Quaternion.Euler(0, 0, z0);
            active = false;
        }
    }
}
