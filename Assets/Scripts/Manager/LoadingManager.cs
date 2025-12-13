using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using System.Collections;
using System;

public class LoadingManager : Singletone<LoadingManager>
{
    [SerializeField] private Slider progressBar;
    public void LoadScene(string sceneName)
    {
        StartCoroutine(LoadSceneAsync(sceneName));
    }

    IEnumerator LoadSceneAsync(string sceneName)
    {
        // 백그라운드에서 로딩 & 완료되어도 바로 활성화 X
        AsyncOperation operation = SceneManager.LoadSceneAsync(sceneName);
        operation.allowSceneActivation = false;
        
        // 씬 로딩이 끝날때까지 반복
        while (operation.progress < 0.9f)
        {
            float progress = operation.progress / 0.9f;
            progressBar.value = progress;
            yield return null;
        }

        // 로딩 완료 처리
        progressBar.value = 1f;

        // 연출이 필요하다면 연출 시간 (ex. 페이드 효과, 어떤 시네머신)
        yield return new WaitForSeconds(1.0f);

        // 씬 전환
        operation.allowSceneActivation = true;
    }
}
