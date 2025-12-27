using System.Collections.Generic;
using UnityEngine;


[RequireComponent(typeof(MonsterSpawnLocation))]
[RequireComponent(typeof(ItemSpawner))]
public class RandomSelector : MonoBehaviour
{

    [Header("아이템 생성 담당 코드")]
    [SerializeField] ItemSpawner spawner;

    [Header("적 생성 담당 코드")]
    [SerializeField] MonsterSpawnLocation spawnTableChecker;

    [Header("생성할 최대 개수")]
    [SerializeField] int itemSpawnCount;
    [SerializeField] int monsterSpawnCount;
    [SerializeField] int elevatorSpawnCount;
    bool isPlaying = false;

    private void Update()
    {
        if (!isPlaying)
        {
            SetItemSpawnLocation();
            //SetMonsterSpawnLocation();
            SetElevatorActivate();
            isPlaying = true;
        }
    }

    /// <summary>
    /// 아이템의 생성을 담당하는 메서드입니다.
    /// </summary>
    // public void SetItemSpawnLocation()
    // {
    //     //생성할 위치를 가져올 테이블 내 데이터의 인덱스 저장용 리스트.
    //     List<int> spawnIndex = new List<int>();

    //     //테이블에서 좌표값을 받아오기
    //     int maxPositionCount = spawner.GetItemCount("SpawnTable");

    //     //0이 나온 경우 실패이므로 반환. 테이블 추출에 실패했든 잘못된 기입으로 오류가 생겼든 0이 나올 것이다.
    //     if (maxPositionCount == 0) return;

    //     //하이에라키에 존재하는 모든 스폰포인트를 받아오는 배열 또는 리스트 등 생성
    //     for(int i = 0; i < itemSpawnCount; i++)
    //     {
    //         //랜덤값 생성
    //         int index = Random.Range(1, maxPositionCount + 1);
    //         //중복 처리(중복일 시 다시 돌림)
    //         for (int counts = 0; counts < spawnIndex.Count; counts++)
    //         {
    //             if(index == spawnIndex[counts])
    //             {
    //                 index = Random.Range(1, maxPositionCount + 1);
    //                 counts = -1;
    //             }
    //         }
    //         //중복 검사를 통과했으면 인덱스 리스트에 추가.
    //         spawnIndex.Add(index);
    //     }
    //     foreach(int index in spawnIndex)
    //     {
    //         //아이템 그룹 테이블 내용을 받아와서
    //         int spawnItemId = spawner.DeclareObjectId(index);
    //         Debug.Log(spawnItemId);
    //         //수정 필요. 아이템 소환을 위한 코드.
    //         ItemManager.Instance.SpawnItem(spawnItemId, spawner.CheckPosition(index));
    //     }
    // }

    public void SetItemSpawnLocation()
    {
        List<int> spawnIndex = new List<int>();

        int maxPositionCount = spawner.GetItemCount("SpawnTable");
        if (maxPositionCount == 0) return;

        // 스폰 위치 랜덤 인덱스 뽑기
        for (int i = 0; i < itemSpawnCount; i++)
        {
            int index = Random.Range(1, maxPositionCount + 1);

            for (int c = 0; c < spawnIndex.Count; c++)
            {
                if (index == spawnIndex[c])
                {
                    index = Random.Range(1, maxPositionCount + 1);
                    c = -1;
                }
            }

            spawnIndex.Add(index);
        }

        // 선택된 위치 중에
        foreach (int index in spawnIndex)
        {
            // 스폰 위치의 그룹 ID 값을 받아
            int groupId = spawner.GetSpawnGroup(index);

            // 해당 그룹 ID로 가중치 랜덤 돌리고 아이템 ID 선정
            int spawnItemId = spawner.DeclareObjectId(groupId);

            // 스폰 좌표
            Vector3 spawnPos = spawner.CheckPosition(index);

            // 생성
            ItemManager.Instance.SpawnItem(spawnItemId, spawnPos);
        }
    }

    /// <summary>
    /// 몬스터의 소환을 담당하는 메서드입니다.
    /// </summary>
    public void SetMonsterSpawnLocation()
    {
        if (spawnTableChecker == null)
            return;

        int count = monsterSpawnCount;
        //위치값에 따른 몬스터 아이디 찾기
        int tableCount = spawnTableChecker.CheckTableCount();
        //몬스터 관련 ID의 경우 5001부터 시작이므로 해당 부분에 맞춰서 시작점 변경. (변동 일어날 시 시작 숫자 변경)
        for (int i = 5001; i <= 5000 + tableCount; i++)
        {
            //랜덤 스폰 여부가 거짓인 경우 확정 소환.
            if (spawnTableChecker.CheckRandom(i) != true)
            {
                GameObject monster = Resources.Load<GameObject>($"{spawnTableChecker.CheckPrefabLocation(i)}");
                Instantiate(monster, spawnTableChecker.CheckPos(i), Quaternion.identity);
                count--;
            }
        }
        //랜덤 스폰 여부가 거짓인 데이터의 소환이 종료된 이후, 소환할 개수가 0보다 큰 경우 아래 코드를 시행
        if (count > 0)
        {
            //소환할 인덱스를 체크할 int형 배열
            int[] indexChecker = new int[count];
            //랜덤으로 ID를 체크하기 위한 반복문.
            for (int i = 0; count > 0; i++)
            {
                //5001에서 시작하여 테이블 카운트 개수만큼을 랜덤값을 돌림.
                int index = Random.Range(5001, tableCount + 5001);
                //중복된 ID값이 나왔을 경우, 반복용 변수 j를 0으로 되돌리고 다시 랜덤값 생성.
                for (int j = 0; j < i; j++)
                {
                    if (indexChecker[j] == index)
                    {
                        index = Random.Range(5001, tableCount + 5001);
                        j = -1;
                    }
                }
                //이렇게 해서 나온 ID값의 랜덤 스폰 여부가 참이 아닌 경우에는 다시 동일한 과정을 반복.
                while (!spawnTableChecker.CheckRandom(index))
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
    public void SetElevatorActivate()
    {
        //소환을 반복한 횟수를 저장할 지역변수.
        int repeatCount = elevatorSpawnCount;
        //비활성화 되어있는 엘레베이터크리에이션 오브젝트를 포함해 싹 다 불러옴
        ElevatorCreation[] elevators = FindObjectsByType<ElevatorCreation>(
            FindObjectsInactive.Include,
            FindObjectsSortMode.None);

        //스폰할 위치를 찾기 위한 리스트.
        List<int> spawnedPoint = new List<int>();

        //일단 모든 엘레베이터 크리에이션 오브젝트에 적용할 코드
        for (int i = 0; i < elevators.Length; i++)
        {
            //랜덤 소환 대상이 아닌 경우 고정형이므로 바로 소환.
            if (elevators[i].isRandom == false)
            {
                Instantiate(elevators[i].elevatorPrefab, elevators[i].Position, elevators[i].Rotation);
                spawnedPoint.Add(i);
            }

        }

        for (int i = 0; i < repeatCount; i++)
        {
            //랜덤값을 저장할 랜덤 지역변수.
            int random = Random.Range(0, elevators.Length);

            //배열의 (랜덤값) 번째의 엘레베이터크리에이션이 가진 isRandom속성이 false(고정 소환형)이거나
            //이미 소환한 종류에 포함이 되는 경우 다시 한번 랜덤값을 뽑는다.
            while (spawnedPoint.Contains(random))
            {
                random = Random.Range(0, elevators.Length);
            }
            //해당 값 위치로 소환.
            Instantiate(elevators[random].elevatorPrefab, elevators[random].Position, elevators[random].Rotation);
            //배열 내에 있는 엘레베이터크리에이션 코드 중 소환을 완료한 칸의 숫자를 저장.
            spawnedPoint.Add(random);
        }

        //소환되지 않은 장소에는 벽을 소환(4방향 다 틀어막힌 벽을 생성하였음.)
        for (int i = 0; i < elevators.Length; i++)
        {
            if (!spawnedPoint.Contains(i))
                Instantiate(elevators[i].elevatorWall, elevators[i].Position, elevators[i].Rotation);
        }
    }
}
