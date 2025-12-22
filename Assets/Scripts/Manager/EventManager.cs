using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 이벤트 관련 제어 클래스
/// </summary>
public class EventManager : Singleton<EventManager>
{
    public Dictionary<int, EventTableData> eventTable;  // <ID, 이벤트 테이블 데이터>
    
}
