using UnityEngine;

public class SessionManager : Singleton<SessionManager>
{
    void OnSessionStart()
    {
        bool resetTrigger = ResetMap();
        if (resetTrigger)
            return;
    }

    GameObject[] items;
    GameObject[] enemies;
    GameObject[] elevators;

    /// <summary>
    /// 맵 초기화 과정을 진행하고, 3가지가 전부 충족된 경우 true를 반환하고 하나라도 실패하면 false를 반환하는 메서드입니다.
    /// </summary>
    /// <returns></returns>
    public bool ResetMap()
    {
        bool isItemsResetSuccess = ResetItems();
        bool isEnemiesResetSuccess = ResetEnemies();
        bool isElevatorsResetSuccess = ResetElevator();
        if (isItemsResetSuccess && isEnemiesResetSuccess && isElevatorsResetSuccess)
            return true;
        else
            return false;
    }


    //현재는 모든 테이블이 존재하는 것이 아니므로 특정 컴포넌트를 가진 것이 아닌 그냥 태그 등으로 판단.
    /// <summary>
    /// 세션에 존재하는 아이템을 찾아 비활성화하는 메서드입니다.
    /// </summary>
    /// <returns></returns>
    public bool ResetItems()
    {
        //상호 작용 가능한 오브젝트가 아이템일 것이므로... (문을 포함한다면 문도 새로 만들면 그만.)
        items = GameObject.FindGameObjectsWithTag("Interactable");

        //아이템 오브젝트 수를 출력
        Debug.Log($"아이템 오브젝트 {items.Length}개 발견");

        //모든 아이템 오브젝트를 비활성화
        foreach (GameObject item in items)
        {
            item.gameObject.SetActive(false);
        }
        return true;
    }

    /// <summary>
    /// 세션에 존재하는 적을 찾아 비활성화하는 메서드입니다.
    /// </summary>
    /// <returns></returns>
    public bool ResetEnemies()
    {
        //괴물 태그를 달고 있는 게 적일 테니까...
        enemies = GameObject.FindGameObjectsWithTag("Monster");

        //적 오브젝트 수를 출력
        Debug.Log($"적 오브젝트 {enemies.Length}개 발견");

        //여기 왔다는 건 오브젝트가 존재한다는 것이므로 그 오브젝트들을 비활성화
        foreach (GameObject enemy in enemies)
        {
            enemy.gameObject.SetActive(false);
        }
        return true;
    }

    /// <summary>
    /// 세션에 현재 활성화 되어있는 모든 엘레베이터 오브젝트를 비활성화시킵니다.
    /// </summary>
    /// <returns></returns>
    public bool ResetElevator()
    {
        elevators = GameObject.FindGameObjectsWithTag("Elevator");

        Debug.Log($"엘레베이터 오브젝트 {elevators.Length}개 발견");

        foreach (GameObject elevator in elevators)
        {
            elevator.gameObject.SetActive(false);
        }
        return true;
    }
}
