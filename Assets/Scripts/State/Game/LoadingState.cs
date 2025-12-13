using UnityEngine;

// 로딩 중일때
public class LoadingState : IState
{
    public void Enter()
    {
        Debug.Log("로딩 상태 진입");
        LoadingManager.Instance.LoadScene(LoadingData.TargetScene);
    }

    public void Exit()
    {
        Debug.Log("로딩 상태 종료");
    }

    public void Update()
    {
    }
}
