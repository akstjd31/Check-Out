using Unity.Hierarchy;
using System.Collections.Generic;
using UnityEngine;

public class RandomSelector : MonoBehaviour
{

    [Header("아이템 생성 담당 코드")]
    [SerializeField] ItemSpawner spawner;

    [Header("적 생성 담당 코드")]
    [SerializeField] MonsterSpawnLocation spawnTableChecker;

    [Header("생성할 최대 개수")]
    [SerializeField]int itemSpawnCount;
    [SerializeField]int monsterSpawnCount;
    [SerializeField]int elevatorSpawnCount;

    /// <summary>
    /// 아이템의 생성을 담당하는 메서드입니다.
    /// </summary>
    public void SetItemSpawnLocation()
    {
        //생성할 위치를 가져올 테이블 내 데이터의 인덱스 저장용 리스트.
        List<int> spawnIndex = new List<int>();
        
        //테이블에서 좌표값을 받아오기
        int maxPositionCount = spawner.GetItemCount("SpawnTable");

        //0이 나온 경우 실패이므로 반환. 테이블 추출에 실패했든 잘못된 기입으로 오류가 생겼든 0이 나올 것이다.
        if (maxPositionCount == 0) return;

        //하이에라키에 존재하는 모든 스폰포인트를 받아오는 배열 또는 리스트 등 생성
        for(int i = 0; i < itemSpawnCount; i++)
        {
            //랜덤값 생성
            int index = Random.Range(1, maxPositionCount + 1);
            //중복 처리(중복일 시 다시 돌림)
            for (int counts = 0; counts < spawnIndex.Count; counts++)
            {
                if(index == spawnIndex[counts])
                {
                    index = Random.Range(1, maxPositionCount + 1);
                    counts = -1;
                }
            }
            //중복 검사를 통과했으면 인덱스 리스트에 추가.
            spawnIndex.Add(index);
        }
        foreach(int index in spawnIndex)
        {
            //아이템 그룹 테이블 내용을 받아와서
            int spawnItemId = spawner.DeclareObjectId(index);
            Debug.Log(spawnItemId);
            //수정 필요. 아이템 소환을 위한 코드.
            ItemManager.Instance.SpawnItem(spawnItemId, spawner.CheckPosition(index));
        }
    }

    /// <summary>
    /// 몬스터의 소환을 담당하는 메서드입니다.
    /// </summary>
    public void SetMonsterSpawnLocation()
    {
        int count = monsterSpawnCount;
        //위치값에 따른 몬스터 아이디 찾기
        int tableCount = spawnTableChecker.CheckTableCount();
        //몬스터 관련 ID의 경우 5001부터 시작이므로 해당 부분에 맞춰서 시작점 변경. (변동 일어날 시 시작 숫자 변경)
        for(int i = 5001; i<=5000 + tableCount; i++)
        {
            //랜덤 스폰 여부가 거짓인 경우 확정 소환.
            if(spawnTableChecker.CheckRandom(i) != true)
            {
                GameObject monster = Resources.Load<GameObject>($"{spawnTableChecker.CheckPrefabLocation(i)}");
                Instantiate(monster, spawnTableChecker.CheckPos(i), Quaternion.identity);
                count--;
            }
        }
        //랜덤 스폰 여부가 거짓인 데이터의 소환이 종료된 이후, 소환할 개수가 0보다 큰 경우 아래 코드를 시행
        if(count > 0)
        {
            //소환할 인덱스를 체크할 int형 배열
            int[] indexChecker = new int[count];
            //랜덤으로 ID를 체크하기 위한 반복문.
            for(int i = 0; count > 0; i++)
            {
                //5001에서 시작하여 테이블 카운트 개수만큼을 랜덤값을 돌림.
                int index = Random.Range(5001, tableCount + 5001);
                //중복된 ID값이 나왔을 경우, 반복용 변수 j를 0으로 되돌리고 다시 랜덤값 생성.
                for(int j = 0; j<i; j++)
                {
                    if (indexChecker[j] == index)
                    {
                        index = Random.Range(5001, tableCount + 5001);
                        j = -1;
                    }
                }
                //이렇게 해서 나온 ID값의 랜덤 스폰 여부가 참이 아닌 경우에는 다시 동일한 과정을 반복.
                while(!spawnTableChecker.CheckRandom(index))
                {
                    index = Random.Range(5001, tableCount + 5001);
                    for (int j = 0; j < i; j++)
                    {
                        if (indexChecker[j] == index)
                        {
                            index = Random.Range(5001, tableCount + 5001);
                            j = 0;
                        }
                    }
                }
                //해당하는 몬스터를 생성.
                GameObject monster = Resources.Load<GameObject>($"{spawnTableChecker.CheckPrefabLocation(index)}");
                //방금 생성한 몬스터를, 해당 ID에 지정된 위치로, 방향 회전 없이 생성.
                Instantiate(monster, spawnTableChecker.CheckPos(index), Quaternion.identity);
                //만들어 둔 몬스터의 ID값을 배열에 저장.
                indexChecker[i] = index;
                //생성 가능 수 1 감소.
                count--;

            }
        }
    }
    void SetElevatorActivate()
    {
        GameObject[] elevators = GameObject.FindGameObjectsWithTag("Elevator");

        foreach(GameObject go in elevators)
        {
            go.gameObject.SetActive(false);
        }
    }
}
