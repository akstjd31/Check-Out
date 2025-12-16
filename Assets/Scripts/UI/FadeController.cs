using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class FadeController : Singleton<FadeController>
{
    [SerializeField] private Image fadeImage;
    [SerializeField] private float fadeDuration = 1f;

    private void Start()
    {
        StartFadeIn();
    }

    public void StartFadeIn() => StartCoroutine(FadeIn());

    private IEnumerator FadeIn()
    {
        float time = fadeDuration;
        while (time > 0f)
        {
            time -= Time.deltaTime;
            SetAlpha(time / fadeDuration);
            yield return null;
        }

        SetAlpha(0f);
    }

    private void SetAlpha(float alpha)
    {
        Color c = fadeImage.color;
        c.a = Mathf.Clamp01(alpha);
        fadeImage.color = c;
    }
}
