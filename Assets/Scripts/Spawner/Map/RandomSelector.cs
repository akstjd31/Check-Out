using Unity.Hierarchy;
using System.Collections.Generic;
using UnityEngine;

public class RandomSelector : MonoBehaviour
{

    [SerializeField] ItemSpawner spawner;
    [SerializeField] MonsterSpawnLocation spawnTableChecker;
    [SerializeField]int itemSpawnCount;
    [SerializeField]int monsterSpawnCount;
    [SerializeField]int elevatorSpawnCount;

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

    public void SetMonsterSpawnLocation()
    {
        int count = monsterSpawnCount;
        //위치값에 따른 몬스터 아이디 찾기
        int tableCount = spawnTableChecker.CheckTableCount();
        for(int i = 5001; i<=5000 + tableCount; i++)
        {
            if(spawnTableChecker.CheckRandom(i) != true)
            {
                GameObject monster = Resources.Load<GameObject>($"{spawnTableChecker.CheckPrefabLocation(i)}");
                Instantiate(monster, spawnTableChecker.CheckPos(i), Quaternion.identity);
                count--;
            }
        }
        if(count > 0)
        {
            int[] indexChecker = new int[count];
            for(int i = 0; count > 0; i++)
            {
                int index = Random.Range(5001, tableCount + 5001);
                for(int j = 0; j<i; j++)
                {
                    if (indexChecker[j] == index)
                    {
                        index = Random.Range(5001, tableCount + 5001);
                        j = 0;
                    }
                }
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
                    
                GameObject monster = Resources.Load<GameObject>($"{spawnTableChecker.CheckPrefabLocation(index)}");
                Instantiate(monster, spawnTableChecker.CheckPos(index), Quaternion.identity);
                indexChecker[i] = index;
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
