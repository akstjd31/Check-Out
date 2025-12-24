using System.Collections.Generic;
using System.Linq;
using UnityEngine;

/// <summary>
/// 이벤트 관련 제어 클래스
/// </summary>
public class EventManager : Singleton<EventManager>
{
    public Dictionary<int, List<EventTableData>> eventGroups;  // <ID, 이벤트 테이블 데이터>
    public Dictionary<string, IEventHandler> handlers;

    protected override void Awake()
    {
        base.Awake();

        eventGroups = new Dictionary<int, List<EventTableData>>();
        TableDataParsing();
    }

    private void Start()
    {
        
    }

    // 테이블 데이터 파싱
    private void TableDataParsing()
    {
        var eventTable = TableManager.Instance.GetTable<int, EventTableData>();

        if (eventTable == null)
        {
            Debug.LogError("이벤트 테이블 데이터가 없음!");
            return;
        }

        foreach (var targetId in TableManager.Instance.GetAllIds(eventTable))
        {
            EventTableData eventTableData = eventTable[targetId];

            if (eventTableData != null)
            {
                // 없으면 리스트 생성 후 넣어주기
                if (!eventGroups.ContainsKey(eventTableData.groupId))
                    eventGroups[eventTableData.groupId] = new List<EventTableData>();
                
                // 하나의 그룹 ID로 실행해야하는 이벤트 테이블 데이터 묶기
                eventGroups[eventTableData.groupId].Add(eventTableData);
            }
        }
    }

    public void ExecuteByStart(string startType, string startValue)
    {
        // // enterCollider : 콜라이더 진입 시 1회
        // if (startType.Equals("enterCollider"))
        // {
        //     // 특정 오브젝트 경로 (진입을 감지할 영역의 식별자)
        // }
        // else
        // {
        //     // 상호작용이 일어날 대상 ID or 이름
        // }

        foreach (var group in eventGroups.Values)
        {
            var first = group[0];

            // 그룹에서 첫 시작이 되는 부분만 확인해서 실행
            if (!IsStartMatched(first, startType, startValue))
                continue;

            ExecuteGroup(group);
        }
    }

    // 한 그룹을 체킹 후 실행
    private void ExecuteGroup(List<EventTableData> group)
    {
        foreach (var evt in group)
        {
            Debug.Log($"[{evt.description}] 실행!");
            if (!ConditionCheckingMachine.Check(evt))
                continue;
            
            ExecuteEvent(evt);
        }
    }

    private void ExecuteEvent(EventTableData evt)
    {
        switch (evt.eventType)
        {
            case "soundOnce":
                SoundManager.Instance.PlaySound(evt.eventValue);
                break;
            case "setSwitchOn":
                break;
            case "showText":
                break;
            case "spawnObject":
                break;
            default:
                Debug.Log("해당 이벤트 타입에 맞는 데이터가 없습니다! (아니면 데이터가 추가가 안됨)");
                break;
        }
    }

    private bool IsStartMatched(EventTableData data, string startType, string startValue)
    {
        if (data.startType != startType)
            return false;

        // startValue는 문자열 비교 or 숫자 비교 둘 다 가능
        return data.startValue == startValue;
    }
}
