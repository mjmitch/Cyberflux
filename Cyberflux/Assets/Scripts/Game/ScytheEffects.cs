using UnityEngine;
using UnityEngine.UI;
using System.Collections;


public class ScytheEffects : MonoBehaviour
{
    [Header("Float / Bobbing")]
    public float floatSpeed = 1f;
    public float floatAmount = 10f;

    [Header("Tilt / Rotation")]
    public float tiltSpeed = 1f;
    public float tiltAmount = 2f;

    [Header("Glow Pulse")]
    public Image scytheImage;
    public Color baseColor = Color.white;
    public float glowSpeed = 2f;
    public float glowIntensity = 0.2f; // 0.2 = 20% brighter

    [Header("Glitch Flicker")]
    public float glitchIntervalMin = 5f;
    public float glitchIntervalMax = 10f;
    public float glitchDuration = 0.1f;
    public float glitchShakeAmount = 5f;

    private Vector3 startPos;
    private Quaternion startRot;
    private Color originalColor;

    void Start()
    {
        startPos = transform.localPosition;
        startRot = transform.localRotation;
        if (scytheImage != null) originalColor = scytheImage.color;

        StartCoroutine(GlitchFlicker());
    }

    void Update()
    {
        // Float
        float newY = Mathf.Sin(Time.time * floatSpeed) * floatAmount;
        transform.localPosition = startPos + new Vector3(0, newY, 0);

        // Tilt
        float tiltZ = Mathf.Sin(Time.time * tiltSpeed) * tiltAmount;
        transform.localRotation = startRot * Quaternion.Euler(0, 0, tiltZ);

        // Glow Pulse
        if (scytheImage != null)
        {
            float glow = 1 + Mathf.Sin(Time.time * glowSpeed) * glowIntensity;
            scytheImage.color = baseColor * glow;
        }
    }

    IEnumerator GlitchFlicker()
    {
        while (true)
        {
            float wait = Random.Range(glitchIntervalMin, glitchIntervalMax);
            yield return new WaitForSeconds(wait);

            Vector3 original = transform.localPosition;

            // jitter for glitchDuration
            float t = 0;
            while (t < glitchDuration)
            {
                transform.localPosition = original + (Vector3)Random.insideUnitCircle * glitchShakeAmount;
                if (scytheImage != null)
                {
                    scytheImage.color = Color.white * Random.Range(0.5f, 1.5f); // quick brightness shifts
                }
                t += Time.deltaTime;
                yield return null;
            }

            // reset
            transform.localPosition = original;
            if (scytheImage != null) scytheImage.color = originalColor;
        }
    }
}
