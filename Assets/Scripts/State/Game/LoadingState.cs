using UnityEngine;

// 로딩 중일때
public class LoadingState : IState
{
    private float timer = 5f;
    public void Enter()
    {
        Debug.Log("로딩 상태에 진입할 때 필요한 코드 작성");
        LoadingManager.Instance.LoadScene("LoadingScene");
    }

    public void Exit()
    {
        Debug.Log("로딩 상태에서 벗어날 때 필요한 코드 작성");
        timer = 5f;
    }

    public void Update()
    {
        timer -= Time.deltaTime;

        if (timer < 0f)
        {
            GameManager.Instance.ChangeState(GameState.Session);
        }
    }
}
