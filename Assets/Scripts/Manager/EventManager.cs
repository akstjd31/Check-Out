using System.Collections.Generic;
using System.Linq;
using UnityEngine;

/// <summary>
/// 이벤트 관련 제어 클래스
/// </summary>
public class EventManager : Singleton<EventManager>
{
    public Dictionary<int, List<EventTableData>> eventGroups;  // <ID, 이벤트 테이블 데이터>

    protected override void Awake()
    {
        base.Awake();

        eventGroups = new Dictionary<int, List<EventTableData>>();
    }

    private void Start()
    {
        TableDataParsing();
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

    // 이벤트가 실제로 동작할 떄
    public void OnEventTriggered(string startType, string startValue)
    {
        
        foreach (var group in eventGroups.Values)
        {
            if (!IsGroupTriggered(group, startType, startValue))
                continue;

            ExecuteGroup(group);
        }
    }

    private bool IsGroupTriggered(List<EventTableData> group, string startType, string startValue)
    {
        foreach (var evt in group)
        {
            Debug.Log("hi");
            if (evt.startType == startType &&
                evt.startValue == startValue)
                return true;
        }
        return false;
    }

    // 조건 확인
    private bool CheckConditions(EventTableData evt)
    {
        if (evt.conditionType1 != "none")
        {
            var cond = ConditionFactory.Create(evt.conditionType1);
            if (!cond.Check(evt.conditionValue1))
                return false;
        }

        if (evt.conditionType2 != "none")
        {
            var cond = ConditionFactory.Create(evt.conditionType2);
            if (!cond.Check(evt.conditionValue2))
                return false;
        }
        return true;
    }

    // 그룹 이벤트 수행
    private void ExecuteGroup(List<EventTableData> group)
    {
        foreach (var evt in group)
        {
            if (!CheckConditions(evt))
                continue;

            ExecuteEvent(evt);
        }
    }

    // 단일 이벤트 실행
    private void ExecuteEvent(EventTableData evt)
    {
        if (evt == null)
            return;

        IEventAction action = EventFactory.Create(evt.eventType);

        if (action == null)
        {
            Debug.LogError($"알 수 없는 EventType: {evt.eventType}");
            return;
        }

        Debug.Log("이벤트 실행!");
        action.Execute(evt.eventValue, evt.targetObject);
    }
}
