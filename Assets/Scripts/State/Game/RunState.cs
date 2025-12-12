using UnityEngine;

// 세션에 접어든 상태
public class RunState : IState
{
    public void Enter()
    {
        Debug.Log("세션 진입할 때 필요한 코드 작성");
    }

    public void Exit()
    {
        Debug.Log("세션에서부터 벗어날 때 필요한 코드 작성");
    }

    public void Update()
    {
        
    }
}
