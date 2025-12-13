using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;

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

    private void OnEnable()
    {
        LoadingManager.Instance.OnLoadingCompleted += HandleLoadingCompleted;
    }

    private void Update()
    {
        stateMachine.Update();
    }

    private void OnDisable()
    {
        LoadingManager.Instance.OnLoadingCompleted -= HandleLoadingCompleted;
    }

    public void ChangeState(GameState newState)
    {
        Debug.Log($"{CurrentState} -> {newState} 변경");
        CurrentState = newState;
        stateMachine.ChangeState(newState);

        if (newState == GameState.Loading)
        {
            SceneManager.LoadScene("LoadingScene");
        }
    }

    public void HandleLoadingCompleted() => ChangeState(LoadingData.NextState);
}
