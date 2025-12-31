using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameManager : Singleton<GameManager>
{
    [Header("State")]
    public GameState CurrentState { get; private set; }

    private StateMachine<GameState> stateMachine;
    public GameObject Player { get; private set; }
    public PlayerStat stat { get; private set; }
    private PlayerView playerView;                  // 임시 돈 텍스트 확인용
    public bool isGameOver = false;

    [SerializeField] private int testitemid = 1;

    [Header("Value")]
    public int Money { get; private set; } = 0;
    private string fileName = "MoneyData.json";

    private EchoSpawnSystem echoSpawnSystem;

    [SerializeField] private Slider slider;

    protected override void Awake()
    {
        base.Awake();

        stateMachine = new StateMachine<GameState>();

        stateMachine.AddState(GameState.Main, new MainState());
        stateMachine.AddState(GameState.Hub, new HubState());
        stateMachine.AddState(GameState.Loading, new LoadingState());
        stateMachine.AddState(GameState.Session, new RunState());
        stateMachine.AddState(GameState.Dead, new DeadState());

        stat = new PlayerStat();
        PlayerStatTableDataParsing();

        if (slider != null)
        {
            slider.SetValueWithoutNotify(1);
            AudioListener.volume = 1;

            slider.onValueChanged.AddListener(SetVolume);
        }

    }

    private void Start()
    {
        ChangeState(GameState.Main);
    }

    public void PlayerInit(GameObject player, PlayerStatHolder holder, PlayerView playerView)
    {
        Player = player;
        holder.Init(stat);
        this.playerView = playerView;
        ChangeMoney(0);

        FieldOfView playerFieldOfView = player.GetComponent<FieldOfView>();

        if (GameManager.Instance.CurrentState == GameState.Session)
        {
            echoSpawnSystem = FindAnyObjectByType<EchoSpawnSystem>();
            echoSpawnSystem.Init(player, playerFieldOfView);
        }
    }

    private void PlayerStatTableDataParsing()
    {
        var playerStatTable = TableManager.Instance.GetTable<int, PlayerConfigTableData>();

        if (playerStatTable == null)
        {
            //debug.Log("플레이어 스탯 테이블이 null 입니다");
            return;
        }

        // 한 줄 마다 value값을 PlayerStat에 넣어주는 작업
        foreach (int targetId in TableManager.Instance.GetAllIds(playerStatTable))
        {
            PlayerConfigTableData playerConfigTableData = playerStatTable[targetId];

            if (playerConfigTableData != null)
            {
                int id = playerConfigTableData.id;
                string value = playerConfigTableData.configValue;
                stat.Apply((PlayerStatId)id, value);
            }
        }

        //debug.Log("플레이어 스탯 테이블 데이터 불러오기 완료!");
    }

    public void OnGameDataLoadButton()
    {
        LoadMoney();
        StorageManager.Instance.LoadStorage();
        InventoryManager.Instance.LoadInventory();
    }
    public void OnGameStartButton() => ChangeState(GameState.Loading);

    public void OnGameExitButton()
    {
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#else
        Application.Quit();
#endif
    }

    private void OnEnable()
    {
        LoadingManager.Instance.OnLoadingEnded += HandleLoadingEnded;
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Return))
            ItemManager.Instance.Test(testitemid);
        // else if (Input.GetKeyDown(KeyCode.C))
        //     EventManager.Instance.ExecuteByStart("interaction", "none");    // 테스트용

        stateMachine?.Update();
    }

    private void OnDisable()
    {
        if (LoadingManager.Instance != null)
        {
            LoadingManager.Instance.OnLoadingEnded -= HandleLoadingEnded;
        }

    }

    // 돈 저장 기능
    public void SaveMoney()
    {
        MoneyData data = new MoneyData();
        data.money = Money;

        SaveLoadManager.Instance.Save<MoneyData>(fileName, data);
        //debug.Log("돈 데이터 저장 완료!");
    }

    // 돈 불러오기 기능
    public void LoadMoney()
    {
        var data = SaveLoadManager.Instance.Load<MoneyData>(fileName);

        if (data == null) return;

        Money = data.money;
        //debug.Log("돈 데이터 불러오기 완료!");
    }

    public void ChangeMoney(int amount)
    {
        Money += amount;

        if (playerView != null)
            playerView.UpdateMoneyText(Money);
    }

    // 상태 변경
    public void ChangeState(GameState newState)
    {
        //debug.Log($"{CurrentState} -> {newState} 변경");
        CurrentState = newState;
        stateMachine.ChangeState(newState);

        // 다음 상태가 로딩 상태면? 해당 씬 호출
        if (newState.Equals(GameState.Loading))
        {
            SceneManager.LoadScene("LoadingScene");
        }
        else if (newState.Equals(GameState.Dead))
        {
            SceneManager.LoadScene("DeadScene");
        }
    }

    // 로딩 완료 시 이벤트
    public void HandleLoadingEnded() => ChangeState(LoadingData.NextState);

    // UI 열려있는지?
    public bool IsOpenedUI() => StorageManager.Instance.IsOpen || StoreManager.Instance.IsOpen;

    private void SetVolume(float value)
    {
        AudioListener.volume = value;
    }
}
