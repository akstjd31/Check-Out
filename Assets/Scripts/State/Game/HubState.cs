using UnityEngine;

// 휴식 공간에 접어든 상태
public class HubState : IState
{
    public void Enter()
    {
        Debug.Log("휴식 상태에 진입할 때 필요한 코드 작성");
    }

    public void Exit()
    {
        Debug.Log("휴식 상태에서 벗어날 때 필요한 코드 작성");
    }

    public void Update()
    {
        
    }
}
