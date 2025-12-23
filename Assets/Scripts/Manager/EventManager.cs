using System.Collections.Generic;
using System.Linq;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.EventSystems;

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

        TableDataParsing();
        Init();
    }

    private void Init()
    {
        // handlers = new Dictionary<string, IEventHandler>
        // {
        //     {
                    // 인터페이스를 상속받는 핸들러 관련된 객체 생성
        //     }
        // }
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
                if (!eventGroups[eventTableData.groupId].Any())
                    eventGroups[eventTableData.groupId] = new List<EventTableData>();
                
                // 하나의 그룹 ID로 실행해야하는 이벤트 테이블 데이터 묶기
                eventGroups[eventTableData.groupId].Add(eventTableData);
            }
        }
    }

    // 시작 타입, 밸류를 어떻게 구분해야할지 아직 잘 모르겠음 (왜 필요한가?)
    public void ExecuteByStart(string startType, string startValue)
    {
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
            if (!ConditionCheckingMachine.Check(evt))
                continue;

            ExecuteEvent(evt);
        }
    }

    private void ExecuteEvent(EventTableData evt)
    {
        switch (evt.eventType)
        {
            case "playAnim":
                break;
            case "playSound":
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
