using UnityEngine;
using UnityEngine.SceneManagement;

// 세션에 접어든 상태
public class RunState : IState
{
    private bool flag;
    public void Enter()
    {
        Debug.Log("세션 상태 진입");
        Init();
    }

    public void Exit()
    {
        Debug.Log("세션 상태 종료");

        ItemManager.Instance.ReturnAllItem();
        LoadingManager.Instance.InitSceneActivation();
    }

    public void Update()
    {
        SoundManager.Instance.IncreaseVolume();
        
        if (GameManager.Instance.isGameOver && !flag)
        {
            flag = true;

            FadeManager.Instance.FadeStartedInvoke();
            FadeManager.Instance.StartFadeOut();
        }
    }

    private void Init()
    {
        flag = false;
        FadeManager.Instance.StartFadeIn();

        SoundManager.Instance.PlayBackgroundSound();

        // 다음 씬 정보 미리 설정
        LoadingData.NextState = GameState.Hub;
        LoadingData.TargetScene = "HubScene";

        // 아이템 스폰
        // 적 스폰
        // 랜덤 엘리베이터 위치 지정
        
        // 준비가 완료되었음을 알림
        LoadingManager.Instance.AllowSceneActivation();
    }
}
