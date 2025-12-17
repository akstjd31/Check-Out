using UnityEngine;
using UnityEngine.AI;
using UnityEngine.InputSystem;
using System.Collections.Generic;
using System.Collections;

public class MonsterMovement : MonoBehaviour
{
    [Header("Station")]
    public List<Transform> moveTransformList;
    [Header("Speed")]
    [SerializeField] private float moveSpeed;
    [Header("delay")]
    public int minimumStopDelay = 2;
    public int maxStopDelay = 3;
    // 길찾기 관련 멤버
    private Queue<Transform> patrolList;
    private Transform currentDestination;
    private Transform tempTransform;
    private Transform emptyTransform;
    // NavMesh 관련 
    private NavMeshAgent navMeshAgent;
    // 테스트용 인풋
    private PlayerInput monsterInput;
    // 코루틴 조건 변수
    private WaitForSeconds endPatrol;
    // 랜덤 변수
    private System.Random patrolOrder;
    private System.Random stopDelay;
    private int tempRandom = 0;

    private void Start()
    {
        //Debug.Log(gameObject.name);
        Init();
        // 다음 목표 지점으로 이동
        PatrolNextOne();
        //TestLoop();
    }

    private void OnDestroy()
    {
        monsterInput.actions["Move"].started -= OnMove;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Monster"))
        {
            Debug.Log($"currentDestination : {currentDestination.name}");
            // stopDelay만큼 멈춰있음
            StartCoroutine(Stop());
            
        }
    }

    private void Init()
    {
        // 내비매쉬 에이전트 겟 컴포넌트 후 정지 상태로 변경
        navMeshAgent = GetComponent<NavMeshAgent>();
        navMeshAgent.isStopped = true;

        // 인풋 시스템 설정
        monsterInput = GetComponent<PlayerInput>();
        monsterInput.actions["Move"].started += OnMove;

        // 패트롤 리스트 초기화
        patrolList = new Queue<Transform>();
        // 빈 목적지 초기화
        emptyTransform = transform;
        // 현재 목적지 초기화
        currentDestination = emptyTransform;
        // 코루틴 조건 변수 초기화
        endPatrol = new WaitForSeconds(5);
        // 순찰 초기화 진행
        InitPatrol();
        // 사용할 랜덤 변수 초기화
        patrolOrder = new System.Random();
        stopDelay = new System.Random();
    }
    // 입력받은 위치로 이동하는 매서드
    public void OnMove(InputAction.CallbackContext context)
    {
        Debug.Log($"{name} {moveTransformList[0].name}로 이동 시작 ");
        navMeshAgent.SetDestination(moveTransformList[0].position);
        navMeshAgent.isStopped = false;
    }

    private void InitPatrol()
    {
        Debug.Log("InitPatrol시작");
        System.Random patrolOrder = new System.Random();
        // 배회 리스트 한 번 초기화
        patrolList.Clear();
        // 랜덤하게 순찰 노드들을 순찰 리스트에 넣음
        Debug.Log("InitPatrol : "+ moveTransformList.Count);
        for(int i = 0; i < moveTransformList.Count; i++) 
        {
            // 입력받은 순찰 위치 중에서 랜덤하게 뽑은 다음 순찰 목록에 넣음
            tempRandom = patrolOrder.Next(0,moveTransformList.Count);
            patrolList.Enqueue(moveTransformList[tempRandom]);
        }
        PrintPatrolList();
    }


    private void Move()
    {
        // 현재 목적지로 이동
        Debug.Log($"{name} {currentDestination.name}로 이동 시작 ");
        navMeshAgent.SetDestination(currentDestination.position);
        navMeshAgent.isStopped = false;
    }
   
    private IEnumerator Stop()
    {
        // StopDelay만큼 멈춤
        Debug.Log("멈춤");
        navMeshAgent.isStopped = true;
        yield return new WaitForSeconds(RandomStopDelay());
        // 다음 목표 지점으로 이동
        PatrolNextOne();
    }

    private int RandomStopDelay()
    {
        // 입력받은 딜레이 최댓값 최솟값 사이에서 랜덤하게 뽑고 반환
        tempRandom = stopDelay.Next(minimumStopDelay, maxStopDelay + 1);
        //Debug.Log("Random Delay :" + tempRandom);
        return tempRandom;
    }

    //배회 리스트 내용물 확인용
    private void PrintPatrolList()
    {
        foreach(var patrol in patrolList)
        {
            Debug.Log($"patrolList : {patrol.name}");
        }
    }

    private void PatrolNextOne()
    {
        ChooseNextDestination();
        // 목적지 설정
        currentDestination = tempTransform;
        
        // 현재 목적지로 이동
        Move();
    }

    private void TestLoop()
    {
        while (true) { PatrolNextOne(); }
    }

    private void ChooseNextDestination()
    {
        // 만약 배회 리스트가 비어있을 경우 다시 초기화에서 뽑아옴
        if (patrolList.Count == 0) { InitPatrol(); }
        // 맨 처음 들어간 Station부터 차례대로 현재 목적지로 설정
        // 만약 현재 목적지와 다음 목적지가 같다면 다시 뽑는다
        // 우선 하나를 뽑음
        tempTransform = patrolList.Dequeue();
        while (currentDestination.name == tempTransform.name)
        {
            // 다 뽑았는데 더 뽑아야 할 경우 초기화
            if (patrolList.Count == 0 && currentDestination.name == tempTransform.name)
            { InitPatrol(); }
            // 하나를 뽑는다.
            tempTransform = patrolList.Dequeue();
        }
    }
}
