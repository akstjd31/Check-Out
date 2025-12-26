using System.Collections;
using UnityEngine;

// 휴식 공간에 접어든 상태
public class HubState : IState
{
    public void Enter()
    {
        Debug.Log("휴식 상태 진입");
        Init();
    }

    public void Exit()
    {
        Debug.Log("휴식 상태 종료");
        LoadingManager.Instance.InitSceneActivation();
    }

    public void Update()
    {
    }

    private void Init()
    {
        FadeManager.Instance.StartFadeIn();
        
        // 다음 씬 정보 미리 설정
        LoadingData.NextState = GameState.Session;
        LoadingData.TargetScene = "SessionScene";

        // 휴식 공간에서 오면 해야할 일 입력
        
        // 준비가 완료되었음을 알림
        LoadingManager.Instance.AllowSceneActivation();
    }
}
