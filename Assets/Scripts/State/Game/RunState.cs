using UnityEngine;
using UnityEngine.SceneManagement;

// 세션에 접어든 상태
public class RunState : IState
{
    public void Enter()
    {
        Debug.Log("세션 상태 진입");
        Init();
    }

    public void Exit()
    {
        Debug.Log("세션 상태 종료");
        SaveData();

        ItemManager.Instance.ReturnAllItem();
        LoadingManager.Instance.InitSceneActivation();
    }

    public void Update()
    {
        if (GameManager.Instance.isGameOver)
        {
            GameManager.Instance.isGameOver = false;
            InventoryManager.Instance.ResetInventory();
            GameManager.Instance.ChangeState(GameState.Loading);
        }
    }

    private void SaveData()
    {
        GameManager.Instance.SaveMoney();
        StorageManager.Instance.SaveStorage();
        InventoryManager.Instance.SaveInventory();
    }

    private void Init()
    {
        // 다음 씬 정보 미리 설정
        LoadingData.NextState = GameState.Hub;
        LoadingData.TargetScene = "HubScene";

        // 아이템 스폰
        // 적 스폰
        // 랜덤 엘리베이터 위치 지정
        
        // 준비가 완료되었음을 알림
        LoadingManager.Instance.AllowSceneActivation();
    }
}
