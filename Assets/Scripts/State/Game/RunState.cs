using UnityEngine;
using UnityEngine.SceneManagement;

// 세션에 접어든 상태
public class RunState : IState
{
    public void Enter()
    {
        Debug.Log("세션 진입할 때 필요한 코드 작성");
        LoadingManager.Instance.LoadScene("SessionScene");

        Init();
    }

    public void Exit()
    {
        Debug.Log("세션에서부터 벗어날 때 필요한 코드 작성");
    }

    public void Update()
    {
    }

    private void Init()
    {
        // 아이템 스폰
        // 적 스폰
        // 랜덤 엘리베이터 위치 지정
        
        // 준비가 완료되었음을 알림
        LoadingManager.Instance.AllowSceneActivation();
    }
}
