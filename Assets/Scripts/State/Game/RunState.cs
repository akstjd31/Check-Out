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
        LoadingManager.Instance.InitSceneActivation();
    }

    public void Update()
    {
    }

    private void Init()
    {
        GameManager.Instance.SaveMoney();
        StorageManager.Instance.SaveStorage();
        InventoryManager.Instance.SaveInventory();
        
        FadeController.Instance.Init();
        
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
