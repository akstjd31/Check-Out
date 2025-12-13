using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using System.Collections;
using System;

public class LoadingManager : Singletone<LoadingManager>
{
    [SerializeField] private Slider progressBar;
    public event Action OnLoadingCompleted;
    private bool readyToActivate;                   // 현재 활성화 가능 상태를 외부에서 알려주기 위함
    
    public void AllowSceneActivation() => readyToActivate = true;
    public void LoadScene(string sceneName)
    {
        readyToActivate = false;
        StartCoroutine(LoadSceneAsync(sceneName));
    }

    IEnumerator LoadSceneAsync(string sceneName)
    {
        // 백그라운드에서 로딩 & 완료되어도 바로 활성화 X
        AsyncOperation operation = SceneManager.LoadSceneAsync(sceneName);
        operation.allowSceneActivation = false;

        if (operation == null)
        {
            Debug.LogError("씬 로드 실패!");
            GameManager.Instance.ChangeState(GameState.Hub);
        }
        
        // 씬 로딩이 끝날때까지 반복
        while (operation.progress < 0.9f)
        {
            // float progress = operation.progress / 0.9f;
            // progressBar.value = progress;
            yield return null;
        }

        // 로딩 완료 처리
        // progressBar.value = 1f;

        // 로딩 완료 후에 외부 신호(readyToActivate) 대기
        Debug.Log("준비가 완료될때까지 대기 중..");
        yield return new WaitUntil(() => readyToActivate);

        // 씬 전환
        operation.allowSceneActivation = true;
        OnLoadingCompleted?.Invoke();
        Debug.Log("씬 전환됨!");
    }
}
