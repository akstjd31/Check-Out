using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : Singleton<GameManager>
{
    [Header("State")]
    // public GameState CurrentState { get; private set; }
    public GameState CurrentState;
    private StateMachine<GameState> stateMachine;
    private GameObject player;

    [Header("Value")]
    public int Money { get; private set; } = 0;
    private string fileName = "MoneyData.json";

    protected override void Awake()
    {
        base.Awake();
        
        stateMachine = new StateMachine<GameState>();

        stateMachine.AddState(GameState.Main, new MainState());
        stateMachine.AddState(GameState.Hub, new HubState());
        stateMachine.AddState(GameState.Loading, new LoadingState());
        stateMachine.AddState(GameState.Session, new RunState());
    }

    private void Start()
    {
        ChangeState(GameState.Main);
    }

    public void OnGameStartButton()
    {
        ChangeState(GameState.Loading);
    }

    private void OnEnable()
    {
        LoadingManager.Instance.OnLoadingEnded += HandleLoadingEnded;
    }

    private void Update()
    {
        if (CurrentState.Equals(GameState.Hub) && player == null)
        {
            player = GameObject.FindGameObjectWithTag("Player");
        }

        
        
        if (Input.GetKeyDown(KeyCode.Return))
            ItemManager.Instance.Test(1);
        else if (Input.GetKeyDown(KeyCode.C))
            EventManager.Instance.ExecuteByStart("interaction", "none");    // 테스트용
            
        stateMachine?.Update();

        // Debug.Log(Money);
    }

    public GameObject GetPlayer() => player;

    private void OnDisable()
    {
        if (LoadingManager.Instance != null)
            LoadingManager.Instance.OnLoadingEnded -= HandleLoadingEnded;
    }

    // 돈 저장 기능

    public void SaveMoney()
    {
        MoneyData data = new MoneyData();
        data.money = Money;

        SaveLoadManager.Instance.Save<MoneyData>(fileName, data);
        Debug.Log("돈 데이터 저장 완료!");
    }

    // 돈 불러오기 기능
    public void LoadMoney()
    {
        var data = SaveLoadManager.Instance.Load<MoneyData>(fileName);

        if (data == null) return;

        Money = data.money;

        Debug.Log("돈 데이터 불러오기 완료!");
    }

    public void ChangeMoney(int amount) => Money += amount;

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
    public void HandleLoadingEnded() => ChangeState(LoadingData.NextState);

    // UI 열려있는지?
    public bool IsOpenedUI() => !FadeController.Instance.IsFadeEnded || StorageManager.Instance.IsOpen || StoreManager.Instance.IsOpen;
}
