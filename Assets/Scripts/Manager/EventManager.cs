using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 이벤트 관련 제어 클래스
/// </summary>
public class EventManager : Singleton<EventManager>
{
    public Dictionary<int, EventTableData> eventTable;  // <ID, 이벤트 테이블 데이터>
    private Dictionary<string, IEventHandler> handlers; // 이벤트 핸들러

    public void ExecuteEvent(int eventId)
    {
        // 해당 ID에 맞는 데이터가 존재하지 않을 떄
        if (!eventTable.TryGetValue(eventId, out var eventData))
            return;
        

    }
}
