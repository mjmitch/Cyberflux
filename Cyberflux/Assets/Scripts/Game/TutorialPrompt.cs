using UnityEngine;
using TMPro;
using System.Collections;

public class TutorialPrompt : MonoBehaviour
{
    
    [SerializeField] private TMP_Text promptText;
    [SerializeField] private float defaultSeconds = 3f;
    [SerializeField] private float fadeDuration = 0.5f;

    private Coroutine co;
    private CanvasGroup cg;

    void Awake()
    {
        cg = GetComponent<CanvasGroup>();
        if (cg == null) cg = gameObject.AddComponent<CanvasGroup>();
        cg.alpha = 0f; // start invisible
        gameObject.SetActive(false);
    }

    public void Show(string msg, float seconds = -1f)
    {
        if (seconds <= 0f) seconds = defaultSeconds;

        if (co != null) StopCoroutine(co);
        promptText.text = msg;

        gameObject.SetActive(true);
        co = StartCoroutine(ShowRoutine(seconds));
    }

    private IEnumerator ShowRoutine(float seconds)
    {
        // Fade In
        yield return StartCoroutine(FadeTo(1f, fadeDuration));

        // Wait while visible
        yield return new WaitForSecondsRealtime(seconds);

        // Fade Out
        yield return StartCoroutine(FadeTo(0f, fadeDuration));

        gameObject.SetActive(false);
        co = null;
    }

    private IEnumerator FadeTo(float target, float duration)
    {
        float start = cg.alpha;
        float time = 0f;

        while (time < duration)
        {
            time += Time.unscaledDeltaTime; // works even if paused
            cg.alpha = Mathf.Lerp(start, target, time / duration);
            yield return null;
        }

        cg.alpha = target;
    }
}
