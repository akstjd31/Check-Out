using Unity.Hierarchy;
using System.Collections.Generic;
using UnityEngine;

public class RandomSelector : MonoBehaviour
{
    [SerializeField] itemSpawnTable spawnTable;
    [SerializeField]int itemSpawnCount;
    [SerializeField]int monsterSpawnCount;
    [SerializeField]int elevatorSpawnCount;

    void SetItemSpawnLocation()
    {
        Vector3[] location = new Vector3[itemSpawnCount];
        //하이에라키에 존재하는 모든 스폰포인트를 받아오는 배열 또는 리스트 등 생성
        for(int i = 0; i < itemSpawnCount; i++)
        {
            //테이블에서 좌표값을 받아오기

            //중복 처리(중복일 시 다시 돌림)

            //아이템 그룹 테이블 내용을 받아와서

            //해당 그룹별로 전부, 하나하나 스위치 문 등으로 총 가중치 값을 구하고 랜덤

            //그룹별, 랜덤 뜬 값에 따른 아이템 선택 후 해당 아이디 반환

            //벡터값(x,y,z), 아이디값을 주는 것으로 메서드 1회 실행(생성)
        }
    }
    void SetSpawnItem(Vector3 position, int id)
    {
        //받아온 테이블 기준 나올 수 있는 아이템이 여러 개라면 랜덤값을 돌려 출력
        //출력된 오브젝트를 생성
    }
    void SetMonsterSpawnLocation()
    {
        //위치값에 따른 몬스터 아이디 찾기

        //아이디값과 isRandom값을 확인하여, isRandom이 false라면 즉시 배치.
        //true라면, 랜덤값 출력. 몬스터 스폰 개수만큼. ( 1보다 낮으면 배치 안함 )
        //스폰 지점은 중복되면 안 됨.
        //이후 생성
    }
    void SetSpawnMonster()
    {

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
