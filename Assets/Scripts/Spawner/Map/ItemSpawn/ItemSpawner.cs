using UnityEngine;
using System.Collections.Generic;

public class ItemSpawner : MonoBehaviour
{
    public int id { get; private set; }
    public string itemType { get; private set; }
    public string itemName { get; private set; }
    public string itemDescription { get; private set; }
    public bool is_canSell { get; private set; }
    public int sellPrice { get; private set; }
    public int imgPath { get; private set; }


    int itemCount = 0;
    int groupCount = 0;
    int spawnCount = 0;

    Table<int, ItemTableData> itemTable;
    Table<int, ItemGroupTableData> groupTable;
    Table<int, ItemSpawnTableData> spawnTable;
    public int GetSpawnGroup(int index)
    {
        return spawnTable[3000 + index].itemGroup;
    }

    private void Start()
    {
        itemTable = TableManager.Instance.GetTable<int, ItemTableData>();
        if (itemTable == null)
        {
            Debug.Log("아이템 생성 테이블이 null입니다.");
            return;
        }

        foreach (int targetId in TableManager.Instance.GetAllIds(itemTable))
        {
            ItemTableData monsterSpawnTableData = itemTable[targetId];
            itemCount++;
        }

        groupTable = TableManager.Instance.GetTable<int, ItemGroupTableData>();
        if (groupTable == null)
        {
            Debug.Log("아이템 그룹 테이블이 null입니다.");
            return;
        }

        foreach (int targetId in TableManager.Instance.GetAllIds(groupTable))
        {
            ItemGroupTableData monsterSpawnTableData = groupTable[targetId];
            groupCount++;
        }

        spawnTable = TableManager.Instance.GetTable<int, ItemSpawnTableData>();
        if (spawnTable == null)
        {
            Debug.Log("아이템 스폰 위치 테이블이 null입니다.");
            return;
        }

        foreach (int targetId in TableManager.Instance.GetAllIds(spawnTable))
        {
            ItemSpawnTableData monsterSpawnTableData = spawnTable[targetId];
            spawnCount++;
        }
    }

    /// <summary>
    /// 어떤 테이블의 총 수량을 받아올지 정하는 메서드입니다. 오타 없이 정확히 기입해 주십시오.
    /// </summary>
    /// <param name="tableType">"ItemTable" : 아이템, "GroupTable" : 아이템 그룹, "SpawnTable" : 아이템 스폰 테이블</param>
    /// <returns></returns>
    public int GetItemCount(string tableType)
    {
        switch (tableType)
        {
            case "ItemTable":
                return itemCount;
            case "GroupTable":
                return groupCount;
            case "SpawnTable":
                return spawnCount;
            default:
                Debug.LogWarning("해당하는 테이블이 컴포넌트에 존재하지 않습니다.");
                return 0;
        }
    }

    /// <summary>
    /// 생성될 위치를 확인하기 위한 메서드입니다.
    /// </summary>
    /// <param name="idValue">테이블 내에서의 id값</param>
    /// <returns></returns>
    public Vector3 CheckPosition(int idValue)
    {
        return new Vector3(spawnTable[3000 + idValue].locationX, spawnTable[3000 + idValue].locationY, spawnTable[3000 + idValue].locationZ);
    }

    /// <summary>
    /// 같은 그룹 내에서 생성할 오브젝트를 결정하기 위한 메서드입니다.
    /// </summary>
    /// <param name="groupValue">테이블 내에서의 그룹ID값</param>
    /// <returns></returns>
    public int DeclareObjectId(int groupValue)
    {
        List<int> idWithCorrectGroup = new List<int>();     // 일치하는 아이템 그룹 ID 리스트
        int probabilityAdded = 0;                           // 가중치 총합

        // 전체 그룹 ID 중에 파라미터로 들어온 그룹 ID가 동일하면 추가, 총 가중치 계산을 위한 합까지
        foreach (int id in TableManager.Instance.GetAllIds(groupTable))
        {
            if (groupTable[id].itemGroup == groupValue)
            {
                idWithCorrectGroup.Add(id);
                probabilityAdded += groupTable[id].probability;
            }
        }

        // 예외처리
        if (idWithCorrectGroup.Count == 0)
        {
            Debug.Log($"{groupValue} 그룹 아이템이 존재하지 않습니다.");
            return 0;
        }

        // 랜덤 가중치
        int randomNumber = Random.Range(1, probabilityAdded + 1);
        int acc = 0;                                                

        // 누적해서 randomNumber가 해당 acc범위내에 들어가면 당첨
        foreach (int id in idWithCorrectGroup)
        {
            acc += groupTable[id].probability;
            if (randomNumber <= acc)
            {
                return groupTable[id].itemId;
            }
        }

        return 0;
    }
}
