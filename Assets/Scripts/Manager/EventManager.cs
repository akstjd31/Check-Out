using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;

/// <summary>
/// 이벤트 관련 제어 클래스
/// </summary>
public class EventManager : Singleton<EventManager>
{
    public Dictionary<int, List<EventTableData>> eventGroups;  // <ID, 이벤트 테이블 데이터>

    public void ExecuteByStart(string startType, string startValue)
    {
        foreach (var group in eventGroups.Values)
        {
            
        }
    }

    private void ExecuteGroup(List<EventTableData> group)
    {
        foreach (var evt in group)
        {
            
        }
    }

    
}
