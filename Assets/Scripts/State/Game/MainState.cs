using UnityEngine;

public class MainState : IState
{
    public void Enter()
    {
        Debug.Log("메인 상태 진입");
        Init();
    }

    public void Exit()
    {
        Debug.Log("메인 상태 종료");
        LoadingManager.Instance.InitSceneActivation();
    }

    public void Update()
    {
    }

    private void Init()
    {
        LoadingData.NextState = GameState.Hub;
        LoadingData.TargetScene = "HubScene";

        LoadingManager.Instance.AllowSceneActivation();
    }
}
