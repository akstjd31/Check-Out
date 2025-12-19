using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class FadeController : Singleton<FadeController>
{
    [SerializeField] private Image fadeImage;
    [SerializeField] private float fadeDuration = 2f;
    public bool IsFadeEnded { get; private set; }

    private void Start()
    {
        StartFadeIn();
    }

    public void Init() => IsFadeEnded = false;

    public void StartFadeIn()
    {
        if (!IsFadeEnded)
            StartCoroutine(FadeIn());
    }

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
        IsFadeEnded = true;
    }

    private void SetAlpha(float alpha)
    {
        Color c = fadeImage.color;
        c.a = Mathf.Clamp01(alpha);
        fadeImage.color = c;
    }
}
