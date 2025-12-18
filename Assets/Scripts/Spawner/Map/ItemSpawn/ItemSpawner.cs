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
        switch(tableType)
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
        return new Vector3(spawnTable[3000+idValue].locationX, spawnTable[3000 + idValue].locationY, spawnTable[3000 + idValue].locationZ);
    }

    /// <summary>
    /// 같은 그룹 내에서 생성할 오브젝트를 결정하기 위한 메서드입니다.
    /// </summary>
    /// <param name="groupValue">테이블 내에서의 그룹ID값</param>
    /// <returns></returns>
    public int DeclareObjectId(int groupValue)
    {
        //일치하는 그룹 아이디를 가진 테이블 내의 데이터 아이디를 담아줄 리스트
        List<int> idWithCorrectGroup = new List<int>();

        //랜덤값으로 가중치 내에서 아이템을 결정하기 위한, 랜덤값 최대치. ( 가중치 총합 )
        int probabilityAdded = 0;

        int correctId = 0;
        //그룹 테이블 내의 모든 데이터들을 확인하면서 그룹아이디가 동일한 데이터의 아이디값을 저장
        for(int i = 1; i <= groupCount; i++)
        {
            if(groupTable[4000+i].itemGroup == groupTable[4000+groupValue].itemGroup)
                idWithCorrectGroup.Add(4000+i);
        }

        //저장된 데이터값이 없을 경우 반환
        if (idWithCorrectGroup.Count <= 0)
        {
            Debug.Log($"{4000 + groupValue} 그룹 아이디를 가진 아이템 테이블 내의 데이터가 존재하지 않습니다.");
            return 0;
        }

        //카운트가 하나 이상일 경우 랜덤값을 돌려야 하기 때문에 따로 분류
        else if(idWithCorrectGroup.Count > 1)
        {

            //이 코드가 실행되는 건 데이터값이 있을 경우이므로, 가중치의 총합을 더해준다.
            for (int i = 0; i < idWithCorrectGroup.Count; i++)
            {
                probabilityAdded += groupTable[idWithCorrectGroup[i]].probability;
            }

            //해당 범위까지의 랜덤값을 굴리고...
            int randomNumber = Random.Range(1, probabilityAdded + 1);

            //값을 비교해 본다.
            int counter = 0;
            for(int i = 0; i < idWithCorrectGroup.Count; i++)
            {
                if(randomNumber <= groupTable[idWithCorrectGroup[i]].probability + counter)
                {
                    counter = groupTable[idWithCorrectGroup[i]].itemId;
                    break;
                }
                else if (randomNumber > groupTable[idWithCorrectGroup[i]].probability + counter)
                {
                    counter += groupTable[idWithCorrectGroup[i]].probability;
                }
            }
            //비교에 성공했을 때의 값을 저장
            correctId = counter;
        }
        //저장된 값을 반환
        return correctId;
    }
}
