using UnityEngine;

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
