using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 게임의 진행 상태를 bool 값으로 관리하는 스위치 클래스
/// </summary>
public class SwitchManager : Singleton<SwitchManager>
{
    public Dictionary<int, SwitchTableData> switchData; // <ID, 데이터>
    private Dictionary<int, bool> switchStates;         // <ID, defaultValue를 bool로 변환한 값>

    protected override void Awake()
    {
        base.Awake();

        switchData = new Dictionary<int, SwitchTableData>();
        switchStates = new Dictionary<int, bool>();
    }

    private void Start()
    {
        TableDataParsing();
        Init();
    }

    private void TableDataParsing()
    {
        var switchTable = TableManager.Instance.GetTable<int, SwitchTableData>();

        if (switchTable == null)
        {
            Debug.LogError("스위치 테이블 데이터가 없음!");
            return;
        }

        foreach (var targetId in TableManager.Instance.GetAllIds(switchTable))
        {
            SwitchTableData switchTableData = switchTable[targetId];

            if (!switchData.ContainsKey(switchTableData.id))
                switchData[switchTableData.id] = switchTableData;
        }
    }

    // 초기화
    private void Init()
    {
        foreach (var pair in switchData)
        {
            int id = pair.Key;
            SwitchTableData data = pair.Value;

            bool defaultState = ParseBool(data.defaultValue);
            switchStates[id] = defaultState;
        }
    }

    // string -> bool
    private bool ParseBool(string value) => value.Equals("TRUE");

    // 스위치 ID 찾기
    public SwitchTableData GetSwitchData(int id)
    {
        if (switchData.TryGetValue(id, out var data) == false) return null;

        return data;
    }

    // 스위치의 bool 값 가져오기
    public bool GetSwitch(int id)
    {
        if (!switchStates.TryGetValue(id, out var value))
        {
            Debug.LogError($"스위치 ID {id}가 없습니다!");
            return false;
        }

        if (value)
        {
            Debug.Log("이미 스위치 값이 true 입니다!");
            return false;
        }

        switchStates[id] = true;
        return switchStates[id];
    }

    public void SetSwitch(int id, bool value)
    {
        if (!switchStates.ContainsKey(id))
        {
            Debug.LogError($"스위치 ID {id} 없음");
            return;
        }

        switchStates[id] = value;
    }

    public void ResetSwitchesOnSessionEnter()
    {
        foreach (var pair in switchData)
        {
            int id = pair.Key;
            SwitchTableData data = pair.Value;

            if (data.isReset == "TRUE")
            {
                switchStates[id] = ParseBool(data.defaultValue);
            }
        }
    }
}
