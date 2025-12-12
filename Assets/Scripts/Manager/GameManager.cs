using UnityEngine;

public class GameManager : Singletone<GameManager>
{
    public GameState CurrentState { get; private set; }
    private StateMachine<GameState> stateMachine;

    private void Awake()
    {
        stateMachine = new StateMachine<GameState>();

        // stateMachine.AddState(GameState.Hub,)
    }
}
