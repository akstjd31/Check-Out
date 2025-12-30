using System;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// 페이드 연출을 위한 매니저
/// </summary>
public class FadeManager : Singleton<FadeManager>
{
    [SerializeField] private Image fadeImage;       // 페이드 이미지
    [SerializeField] private float fadeDuration;    // 페이드 지속 시간
    public event Action OnFadeStarted;              // 페이드 시작 이벤트 액션
    public event Action OnFadeEnded;                // 페이드 끝 이벤트 액션

    private void OnEnable()
    {
        OnFadeStarted += () => fadeImage.gameObject.SetActive(true);
    }

    private void OnDisable()
    {
        OnFadeStarted -= () => fadeImage.gameObject.SetActive(true);
    }

    public void StartFadeIn()
    {
        StartCoroutine(FadeIn());
    }

    public void StartFadeOut()
    {
        StartCoroutine(FadeOut());
    }

    public void SetFadeImageDeactivate() => fadeImage.gameObject.SetActive(false);

    // 로딩 때 페이드 연출을 시작 (데이터 로드되는 부분을 가리기 위한 부분)
    public void FadeStartedInvoke() => OnFadeStarted?.Invoke();

    // 페이드 인
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

    // 페이드 아웃 (플레이어가 죽었을 떄)
    private IEnumerator FadeOut()
    {
        float time = 0f;

        while (time < fadeDuration)
        {
            time += Time.deltaTime;
            SetAlpha(time / fadeDuration);
            yield return null;
        }
        OnFadeEnded?.Invoke();

        if (GameManager.Instance.isGameOver)
            GameManager.Instance.ChangeState(GameState.Dead);
    }


    // 이미지 컬러 조정
    private void SetAlpha(float alpha)
    {
        Color c = fadeImage.color;
        c.a = Mathf.Clamp01(alpha);
        fadeImage.color = c;
    }
}
