using UnityEngine;

public class MainState : IState
{
    public void Enter()
    {
        //debug.Log("메인 상태 진입");
        Init();
    }

    public void Exit()
    {
        //debug.Log("메인 상태 종료");
        //LoadingManager.Instance.InitSceneActivation();
    }

    public void Update()
    {
    }

    private void Init()
    {
        Cursor.lockState = CursorLockMode.Confined;
        Cursor.visible = true;

        LoadingData.NextState = GameState.Hub;
        LoadingData.TargetScene = "RestSpace";

        SoundManager.Instance.PlayMainSound();
        //LoadingManager.Instance.AllowSceneActivation();
    }
}
