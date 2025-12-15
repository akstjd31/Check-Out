using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;

public class GameManager : Singleton<GameManager>
{
    public GameState CurrentState { get; private set; }
    private StateMachine<GameState> stateMachine;


    protected override void Awake()
    {
        base.Awake();
        
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

    // 상태 변경
    public void ChangeState(GameState newState)
    {
        Debug.Log($"{CurrentState} -> {newState} 변경");
        CurrentState = newState;
        stateMachine.ChangeState(newState);

        // 다음 상태가 로딩 상태면? 해당 씬 호출
        if (newState == GameState.Loading)
        {
            SceneManager.LoadScene("LoadingScene");
        }
    }

    // 로딩 완료 시 이벤트
    public void HandleLoadingCompleted() => ChangeState(LoadingData.NextState);
}
