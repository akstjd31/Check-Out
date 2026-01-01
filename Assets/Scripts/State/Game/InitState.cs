public class InitState : IState
{
    public void Enter()
    {
        //debug.Log("메인 상태 진입");
        Init();
    }

    public void Exit()
    {
        //debug.Log("메인 상태 종료");
    }

    public void Update()
    {
    }

    private void Init()
    {
        GameManager.Instance.ChangeState(GameState.Main);
    }
}
