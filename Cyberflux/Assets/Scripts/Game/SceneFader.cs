using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneFader : MonoBehaviour
{


    [SerializeField] float fadeDuration = 0.5f;
    CanvasGroup cg;

    void Awake()
    {
        cg = GetComponent<CanvasGroup>();
        DontDestroyOnLoad(gameObject); // stays through scene changes
        StartCoroutine(FadeIn());
    }

    public void FadeToScene(int buildIndex)
    {
        StartCoroutine(FadeOutIn(buildIndex));
    }

    IEnumerator FadeIn()
    {
        float t = 0;
        while (t < fadeDuration)
        {
            t += Time.unscaledDeltaTime;
            cg.alpha = 1 - (t / fadeDuration);
            yield return null;
        }
        cg.alpha = 0;
    }

    IEnumerator FadeOutIn(int buildIndex)
    {
        // Fade out
        float t = 0;
        while (t < fadeDuration)
        {
            t += Time.unscaledDeltaTime;
            cg.alpha = t / fadeDuration;
            yield return null;
        }

        // Load scene
        yield return SceneManager.LoadSceneAsync(buildIndex);

        // Fade in
        yield return FadeIn();
    }
   
}
