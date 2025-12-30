using System.Collections.Generic;
using System.Linq;
using UnityEngine;

/// <summary>
/// 이벤트 관련 제어 클래스
/// </summary>
public class EventManager : Singleton<EventManager>
{
    public Dictionary<int, List<EventTableData>> eventGroups;  // <ID, 이벤트 테이블 데이터>
    public Dictionary<int, float> cooldownData;                // <ID, cooldown>

    public float Delay { get; set; } = 0f;
    public float Cooldown { get; set; } = 0f;

    protected override void Awake()
    {
        base.Awake();

        eventGroups = new Dictionary<int, List<EventTableData>>();
        cooldownData = new Dictionary<int, float>();
    }

    private void Start()
    {
        TableDataParsing();
    }

    private void Update()
    {
        if (cooldownData == null) return;

        float delta = Time.deltaTime;
        var keys = new List<int>(cooldownData.Keys);

        foreach (int key in keys)
        {
            if (cooldownData[key] > 0f)
            {
                cooldownData[key] = Mathf.Max(0f, cooldownData[key] - delta);
            }
        }
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

                if (!cooldownData.ContainsKey(eventTableData.groupId))
                    cooldownData[eventTableData.groupId] = 0f;

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
            if (cooldownData[group[0].groupId] > 0f || !IsGroupTriggered(group, startType, startValue))
                continue;

            ExecuteGroup(group);
        }
    }

    private bool IsGroupTriggered(List<EventTableData> group, string startType, string startValue)
    {
        foreach (var evt in group)
        {
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
        action.Execute(evt.eventValue, evt.targetObject, evt.startValue);

        // 만약 쿨 다운 액션이었다면 쿨 다운 값 추가
        if (action is CoolDownAction)
        {
            Debug.Log($"쿨다운 설정 완료! 현재 값: {Cooldown}");
            cooldownData[evt.groupId] = Cooldown;
        }
    }
}
