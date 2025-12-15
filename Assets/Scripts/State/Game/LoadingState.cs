using UnityEngine;

// 로딩 중일때
public class LoadingState : IState
{
    private float timer;
    private bool isLoading;

    public void Enter()
    {
        timer = 3f;
        isLoading = false;
        Debug.Log("로딩 상태 진입");
    }

    public void Exit()
    {
        Debug.Log("로딩 상태 종료");
    }

    public void Update()
    {
        if (isLoading)
            return;

        timer -= Time.deltaTime;

        if (timer <= 0f)
        {
            isLoading = true;
            LoadingManager.Instance.LoadScene(LoadingData.TargetScene);
        }
    }
}
