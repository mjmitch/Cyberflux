using UnityEngine;

public class UiGlitchTiker : MonoBehaviour
{
    public UIGlitchBurstUnscaled burst;
    public float minDelay = 0.8f;
    public float maxDelay = 2.0f;

    float timer, nextDelay;

    void Reset() { if (!burst) burst = GetComponent<UIGlitchBurstUnscaled>(); }
    void OnEnable() { timer = 0f; nextDelay = Random.Range(minDelay, maxDelay); }
    void Update()
    {
        if (!burst) return;
        timer += Time.unscaledDeltaTime;
        if (timer >= nextDelay)
        {
            timer = 0f;
            nextDelay = Random.Range(minDelay, maxDelay);
            burst.Trigger();
        }
    }
}
