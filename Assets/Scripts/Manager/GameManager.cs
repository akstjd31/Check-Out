using UnityEngine;

public class GameManager : Singletone<GameManager>
{
    public GameState CurrentState { get; private set; }
    private StateMachine<GameState> stateMachine;

    private void Awake()
    {
        stateMachine = new StateMachine<GameState>();

        stateMachine.AddState(GameState.Hub, new HubState());
        stateMachine.AddState(GameState.Loading, new LoadingState());
        stateMachine.AddState(GameState.Session, new RunState());
    }

    private void Start()
    {
        ChangeState(GameState.Hub);
    }

    private void Update()
    {
        stateMachine.Update();
    }

    public void ChangeState(GameState newState)
    {
        CurrentState = newState;
        stateMachine.ChangeState(newState);
        Debug.Log($"상태 {newState}로 변경됨!");
    }
}
