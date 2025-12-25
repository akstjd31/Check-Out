using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using System.Collections;
using System;
/// <summary>
/// 로딩 매니저 (씬 로드를 비동기로 처리)
/// </summary>
public class LoadingManager : Singleton<LoadingManager>
{
    [SerializeField] private Slider loadingBar;
    public event Action OnLoadingEnded;
    private bool readyToActivate;                   // 현재 활성화 가능 상태를 외부에서 알려주기 위함
    
    //public void SetLoadingBarActive(bool active) => loadingBar.gameObject.SetActive(active);
    public void AllowSceneActivation() => readyToActivate = true;
    public void InitSceneActivation() => readyToActivate = false;

    public void LoadScene(string sceneName)
    {
        StartCoroutine(LoadSceneAsync(sceneName));

        if (GameManager.Instance.CurrentState == GameState.Loading)
        {
            OnLoadingEnded?.Invoke();
        }
    }

    private void LoadData()
    {
        if (GameManager.Instance.PreviousState != GameState.Main)
            SaveData();

        GameManager.Instance.LoadMoney();
        StorageManager.Instance.LoadStorage();
        InventoryManager.Instance.LoadInventory();
    }

    private void SaveData()
    {
        GameManager.Instance.SaveMoney();
        StorageManager.Instance.SaveStorage();
        InventoryManager.Instance.SaveInventory();
    }

    // 씬 로드 비동기 작업
    IEnumerator LoadSceneAsync(string sceneName)
    {
        // 백그라운드에서 로딩 & 완료되어도 바로 활성화 X
        AsyncOperation operation = SceneManager.LoadSceneAsync(sceneName);
        operation.allowSceneActivation = false;

        if (operation == null)
        {
            Debug.LogError("씬 로드 실패!");
            yield return null;
        }
        
        // 씬 로딩이 끝날때까지 반복
        // while (operation.progress < 0.9f)
        // {
        //     float progress = operation.progress / 0.9f;
        //     loadingBar.value = progress;
        //     yield return null;
        // }

        // 로딩 완료 처리
        // progressBar.value = 1f;

        // 로딩 완료 후에 외부 신호(readyToActivate) 대기
        Debug.Log("준비가 완료될때까지 대기 중..");

        LoadData();

        // 외부에서 신호를 기다림
        yield return new WaitUntil(() => readyToActivate); 

        // 씬 전환
        operation.allowSceneActivation = true;

        FadeController.Instance.LoadingComplete();

        Debug.Log("씬 전환됨!");
    }
}
