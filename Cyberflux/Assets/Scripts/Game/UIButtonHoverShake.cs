using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.Events;

[RequireComponent(typeof(RectTransform))]
public class UIButtonHoverShake : MonoBehaviour,
    IPointerEnterHandler, IPointerExitHandler, IPointerClickHandler,
    ISelectHandler, IDeselectHandler
{
    [Header("Targets")]
    public RectTransform target;                 // defaults to this RectTransform
    [Header("Hover Shake")]
    public float hoverAmplitude = 6f;            // pixels
    public float hoverFrequency = 30f;           // shakes per second feel
    public float settleSpeed = 15f;              // return-to-rest speed
    [Header("Click Punch")]
    public float clickPunchScale = 1.12f;        // 12% pop
    public float clickDuration = 0.09f;          // seconds
    [Header("Audio (optional)")]
    public AudioSource audioSource;
    public AudioClip hoverClip;
    public AudioClip clickClip;
    [Header("Accessibility")]
    public bool reduceMotion = false;            // per-button override
    public static bool GlobalReduceMotion = false;

    [Header("Flow")]
    public UnityEvent onClickAfterAnimation;
    public float postClickDelay = 0.02f; // tiny buffer

    Vector2 _restPos;
    Vector3 _restScale;
    bool _hovering;
    float _t;

    void Awake()
    {
        if (!target) target = transform as RectTransform;
        _restPos = target.anchoredPosition;
        _restScale = target.localScale;
    }

    void OnEnable()
    {
        _hovering = false;
        target.anchoredPosition = _restPos;
        target.localScale = _restScale;
    }

    void Update()
    {
        bool rm = reduceMotion || GlobalReduceMotion;
        if (_hovering && !rm)
        {
            // Perlin-based jitter so it feels organic
            _t += Time.unscaledDeltaTime * hoverFrequency;
            float jx = (Mathf.PerlinNoise(_t, 0f) - 0.5f) * 2f * hoverAmplitude;
            float jy = (Mathf.PerlinNoise(0f, _t) - 0.5f) * 2f * (hoverAmplitude * 0.6f);
            Vector2 targetPos = _restPos + new Vector2(jx, jy);
            target.anchoredPosition = Vector2.Lerp(target.anchoredPosition, targetPos, 0.5f);
        }
        else
        {
            // Smoothly return to rest
            target.anchoredPosition = Vector2.Lerp(target.anchoredPosition, _restPos, Time.unscaledDeltaTime * settleSpeed);
        }
    }

    public void OnPointerEnter(PointerEventData e)
    {
        _hovering = true;
        Play(hoverClip, 0.7f);
    }

    public void OnPointerExit(PointerEventData e)
    {
        _hovering = false;
    }

    public void OnPointerClick(PointerEventData e)
    {
        Play(clickClip, 1f);
        if (reduceMotion || GlobalReduceMotion || !isActiveAndEnabled || !gameObject.activeInHierarchy)
        {
            onClickAfterAnimation?.Invoke();
            return;
        }
        StartCoroutine(ClickFlow());
    }

    System.Collections.IEnumerator ClickFlow()
    {
        yield return Punch();                  // play the pop
        yield return new WaitForSecondsRealtime(postClickDelay);
        onClickAfterAnimation?.Invoke();       // now switch panels / load scene
    }

    System.Collections.IEnumerator Punch()
    {
        float t = 0f;
        Vector3 start = _restScale;
        Vector3 peak = start * clickPunchScale;

        while (t < clickDuration)
        {
            t += Time.unscaledDeltaTime;
            float a = Mathf.Clamp01(t / clickDuration);
            // triangle curve: up then down
            float k = a < 0.5f ? a * 2f : (1f - (a - 0.5f) * 2f);
            target.localScale = Vector3.Lerp(start, peak, k);
            yield return null;
        }
        target.localScale = start;
    }

    void Play(AudioClip clip, float vol)
    {
        if (clip && audioSource) audioSource.PlayOneShot(clip, vol);
    }

    // Keyboard / gamepad selection support
    public void OnSelect(BaseEventData e) { _hovering = true; }
    public void OnDeselect(BaseEventData e) { _hovering = false; }
}