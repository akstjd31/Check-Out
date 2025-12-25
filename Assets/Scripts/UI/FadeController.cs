using System;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class FadeController : Singleton<FadeController>
{
    [SerializeField] private Image fadeImage;
    [SerializeField] private float fadeDuration;
    // public bool IsFadeEnded { get; private set; }
    public event Action OnFadeStarted;
    public event Action OnFadeEnded;

    private void OnEnable()
    {
        OnFadeStarted += () => fadeImage.gameObject.SetActive(true);
        OnFadeEnded += () => fadeImage.gameObject.SetActive(false);
    }

    private void OnDisable()
    {
        OnFadeStarted -= () => fadeImage.gameObject.SetActive(true);
        OnFadeEnded -= () => fadeImage.gameObject.SetActive(false);
    }

    public void StartFadeIn()
    {
        StartCoroutine(FadeIn());
    }

    public void LoadingComplete() => OnFadeStarted?.Invoke();

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
        OnFadeEnded?.Invoke();
    }

    private void SetAlpha(float alpha)
    {
        Color c = fadeImage.color;
        c.a = Mathf.Clamp01(alpha);
        fadeImage.color = c;
    }
}
