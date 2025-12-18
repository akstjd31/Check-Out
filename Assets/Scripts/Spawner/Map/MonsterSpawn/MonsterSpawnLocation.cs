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

    //몬스터 스폰 장소 관련 테이블을 받아오기 위한 스폰테이블.
    Table<int, MonsterSpawnTableData> spawnTable;

    //몬스터 테이블의 인자값? 수.
    int count = 0;
    private void Start()
    {
        //테이블매니저로부터 몬스터 스폰 테이블의 데이터를 받아온다.
        spawnTable = TableManager.Instance.GetTable<int, MonsterSpawnTableData>();
        //없을 경우 안내 후 반환.
        if (spawnTable == null)
        {
            Debug.Log("적 생성 테이블이 null입니다.");
            return;
        }

        //테이블매니저로부터 테이블 내 각각의 ID를 불러와서
        foreach (int targetId in TableManager.Instance.GetAllIds(spawnTable))
        {
            //몬스터스폰데이터의 대상 아이디 데이터를 대입.
            MonsterSpawnTableData monsterSpawnTableData = spawnTable[targetId];
            //대입한 수 증가.
            count++;
        }
    }

    /// <summary>
    /// 테이블 내의 총 데이터 개수를 받아오기 위한 메서드입니다.
    /// </summary>
    /// <returns></returns>
    public int CheckTableCount()
    {
        Debug.Log($"몬스터 소환 가능한 장소의 수는 {count}입니다.");
        return count;
    }

    /// <summary>
    /// id값을 받아 해당 아이디 기준 스폰 위치를 반환받는 메서드입니다.
    /// </summary>
    /// <param name="id">테이블 내의 데이터 ID</param>
    /// <returns></returns>
    public Vector3 CheckPos(int id)
    {
        return new Vector3(spawnTable[id].locationX, spawnTable[id].locationY, spawnTable[id].locationZ);
    }

    /// <summary>
    /// id값을 받아 프리팹의 랜덤 소환 여부를 확인하는 메서드입니다.
    /// </summary>
    /// <param name="id">테이블 내의 데이터 ID</param>
    /// <returns></returns>
    public bool CheckRandom(int id)
    {
        Debug.Log($"이 id의 프리팹 랜덤 소환 여부는 {spawnTable[id].isRandom}입니다.");
        return spawnTable[id].isRandom;
    }

    /// <summary>
    /// id값을 받아 프리팹의 경로를 받아오는 메서드입니다.
    /// </summary>
    /// <param name="id">테이블 내의 데이터 ID</param>
    /// <returns></returns>
    public string CheckPrefabLocation(int id)
    {
        Debug.Log($"소환할 프리팹의 위치는 {spawnTable[id].monsterId}입니다.");
        return spawnTable[id].monsterId;
    }
}
