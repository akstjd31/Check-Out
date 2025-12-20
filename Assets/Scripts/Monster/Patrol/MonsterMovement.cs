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
    [SerializeField] private int minimumStopDelay = 2;
    [SerializeField] private int maxStopDelay = 3;
    // 길찾기 관련 멤버
    private Transform currentDestination;
    private Transform tempTransform;
    private Transform emptyTransform;
    private Transform nextDestination;
    // NavMesh 관련 
    private NavMeshAgent navMeshAgent;
    // 테스트용 인풋
    private PlayerInput monsterInput;
    // 랜덤 변수
    private System.Random patrolOrder;
    private System.Random stopDelay;
    private int tempRandom = 0;
    // 몬스터가 멈췄을 경우 진행할 타이머
    private float exceptionTimer = 0;
    [Header("Exception")]
    // 몬스터 움직이지 않는지 판단할 시간
    [SerializeField]private float isExceptionTimer = 7;

    // 프로퍼티 
    public int MinimumStopDelay { get; set; }
    public int MaxStopDelay { get; set; }
    private void Awake()
    {
        //Debug.Log(gameObject.name);
        Init();
        // 다음 목표 지점으로 이동
        //PatrolNextOne();
        //TestLoop();
        Debug.Log("움직임 컴포넌트 스타트 실행 완료");
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("WayPoint"))
        {
            Debug.Log($"currentDestination : {currentDestination.name}");
            // stopDelay만큼 멈춰있음
            StartCoroutine(Stop());
        }
    }

    private void Update()
    {
        // 만약 몬스터가 멈춰있을 경우 타이머를 진행
        if (navMeshAgent.velocity == Vector3.zero && exceptionTimer < 7f) 
        {
            //Debug.Log($"몬스터 움직임 감지 못 함 : {exceptionTimer}");
            exceptionTimer += Time.deltaTime;
        }
        // 몬스터가 일정 시간 동안 멈춰있을 경우 자체적으로 배회 진행
        else if (navMeshAgent.velocity == Vector3.zero && exceptionTimer >= 7f)
        {
            // 타이머 초기화
            PatrolNextOne();
        }
        else { return; }
    }

    public void Init()
    {
        // 내비매쉬 에이전트 겟 컴포넌트 후 정지 상태로 변경
        navMeshAgent = GetComponent<NavMeshAgent>();
        navMeshAgent.isStopped = true;
        // 속도 설정
        navMeshAgent.speed = moveSpeed;
        // 빈 목적지 초기화
        emptyTransform = transform;
        // 현재 목적지 초기화
        currentDestination = emptyTransform;
        // 사용할 랜덤 변수 초기화
        patrolOrder = new System.Random();
        stopDelay = new System.Random();
    }


    private void Move(Vector3 target)
    {
        navMeshAgent.SetDestination(target);
        navMeshAgent.isStopped = false;
        currentDestination = nextDestination;
    }

    public void Move(Transform inputTransform, float inputSpeed)
    {
        Debug.Log("플레이어를 발견. 해당 좌표로 이동합니다.");
        navMeshAgent.speed = inputSpeed;
        navMeshAgent.SetDestination(inputTransform.position);
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
    public void StopToMissing()
    {
        // StopDelay만큼 멈춤
        Debug.Log("멈춤");
        navMeshAgent.isStopped = true;
    }

    private int RandomStopDelay()
    {
        // 입력받은 딜레이 최댓값 최솟값 사이에서 랜덤하게 뽑고 반환
        tempRandom = stopDelay.Next(minimumStopDelay, maxStopDelay + 1);
        //Debug.Log("Random Delay :" + tempRandom);
        return tempRandom;
    }

    //public void PatrolNextOne(float inputSpeed)
    //{
    //    ChooseNextDestination();
    //    // 목적지 설정
    //    currentDestination = tempTransform;
    //    // 속도 설정
    //    navMeshAgent.speed = inputSpeed;
    //    Debug.Log(inputSpeed);

    //    // 현재 목적지로 이동
    //    Move();
    //}

    // 패트롤 시작
    public void PatrolNextOne()
    {
        nextDestination = ChooseNextDestination();

        // 현재 목적지로 이동
        Move(nextDestination.position);
    }

    // private void TestLoop()
    // {
    //     while (true) { PatrolNextOne(); }
    // }

    // 다음 목적지 뽑기
    private Transform ChooseNextDestination()
    {
        System.Random random = new System.Random();

        while (true)
        {
            int index = random.Next(0, moveTransformList.Count);

            if (currentDestination != moveTransformList[index])
            {
                return moveTransformList[index];
            }
        }
    }

    public void ChangeSpeed(float speed)
    {
        if (navMeshAgent == null)
            navMeshAgent = GetComponent<NavMeshAgent>();
        navMeshAgent.speed = speed;
    }
}
