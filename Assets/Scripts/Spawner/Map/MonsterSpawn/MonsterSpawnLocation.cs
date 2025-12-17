using UnityEngine;

public class MonsterSpawnLocation : MonoBehaviour
{
    public int id {  get; private set; }
    public float locationX { get; private set; }
    public float locationY { get; private set; }
    public float locationZ { get; private set; }
    public bool isRandom {  get; private set; }
    public string monsterId { get; private set; }

    //프리팹을 가져오는 건 대략 이런 방식이다. 프리팹 파일의 이름까지 작성.
    //GameObject Prefab = Resources.Load<GameObject>("Prefabs/Monster/MonsterExample");

    Table<int, MonsterSpawnTableData> spawnTable;

    int count = 0;
    private void Start()
    {
        spawnTable = TableManager.Instance.GetTable<int, MonsterSpawnTableData>();
        if (spawnTable == null)
        {
            Debug.Log("적 생성 테이블이 null입니다.");
            return;
        }

        foreach (int targetId in TableManager.Instance.GetAllIds(spawnTable))
        {
            MonsterSpawnTableData monsterSpawnTableData = spawnTable[targetId];
            count++;
        }
    }


    public int CheckTableCount()
    {
        Debug.Log($"몬스터 소환 가능한 장소의 수는 {count}입니다.");
        return count;
    }

    public Vector3 CheckPos(int id)
    {
        return new Vector3(spawnTable[id].locationX, spawnTable[id].locationY, spawnTable[id].locationZ);
    }

    public bool CheckRandom(int id)
    {
        Debug.Log($"이 id의 프리팹 랜덤 소환 여부는 {spawnTable[id].isRandom}입니다.");
        return spawnTable[id].isRandom;
    }

    public string CheckPrefabLocation(int id)
    {
        Debug.Log($"소환할 프리팹의 위치는 {spawnTable[id].monsterId}입니다.");
        //return spawnTable[id].monsterId;
        return "Prefabs/Monster/Walker";
    }
}
