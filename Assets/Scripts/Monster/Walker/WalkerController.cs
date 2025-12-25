using System.Collections;
using UnityEngine;
using UnityEngine.AI;

public class WalkerController : MonsterController
{
    [SerializeField] private WalkerView walkerView;
    [SerializeField] private float rotateSpeed;
    private WalkerModel walkerModel;
    private FieldOfView walkerFieldOfView;
    private MonsterMovement walkerMovement;
    private WaitForSeconds stopToMissing;
    private float chaseTimer;
    private float checkTimer;
    private bool onRun = true;
   

    private void Awake()
    {
        // 컴포넌트 추가
        walkerView = GetComponent<WalkerView>();
        walkerModel = GetComponent<WalkerModel>();
        walkerFieldOfView = GetComponent<FieldOfView>();
        walkerMovement = GetComponent<MonsterMovement>();
    }

    private void Start()
    {
        Init();
        Debug.Log("워커 컨트롤러 배회 시작");
        walkerModel.monsterState = Monster.MonsterState.WanderingAround;
        walkerMovement.ChangeSpeed(walkerModel.PatrolSpeed);
        walkerMovement.PatrolNextOne();
        StartCoroutine(walkerFieldOfView.FindTargetsWithDelay());
    }

    private void Update()
    {
        //Debug.Log($"walkerState : {walkerModel.walkerState}");
        //Debug.Log(walkerFieldOfView.visibleTargets.Count > 0);
        //Debug.Log($"visibleTargets : {walkerFieldOfView.visibleTargets.Count}");
        //Debug.Log(walkerModel.walkerState != WalkerModel.WalkerState.Chase);
        // 만약 플레이어가 시야에 들어온다면 발견 상태 실행 후 추격 진행
        if (walkerFieldOfView.visibleTargets.Count > 0 && walkerModel.monsterState != Monster.MonsterState.Chase)
        {
            //Debug.Log("조건 만족");
            // 발견 상태 수행
            walkerModel.ChangeState(Monster.MonsterState.FindPlayer);
        }
        if (walkerFieldOfView.visibleTargets.Count == 0 && walkerModel.monsterState == Monster.MonsterState.Chase)
        {
            // 만약 플레이어를 시야에 놓치면 MissingPlayer 전환
            walkerModel.ChangeState(Monster.MonsterState.MissingPlayer);
        }
    }
    private void OnEnable()
    {
        // 이벤트 구독 설정
        walkerModel.OnWanderingAround += StartPatrol;
        walkerModel.OnFindPlayer += Find;
        walkerModel.OnChase += StartChase;
        walkerModel.OnMissingPlayer += StartMissingPlayer;
        walkerModel.OnAlerted += StartAlerted;
    }

    private void OnDisable()
    {
        // 이벤트 구독 해제
        walkerModel.OnWanderingAround -= StartPatrol;
        walkerModel.OnFindPlayer -= Find;
        walkerModel.OnChase -= StartChase;
        walkerModel.OnMissingPlayer -= StartMissingPlayer;
        walkerModel.OnAlerted -= StartAlerted;
    }

    private void Init()
    {
        // 모델 값을 해당 컴포넌트에 전달
        // Monster Field Of View
        //Debug.Log($"Rdaius : {walkerModel.ViewRadius}");
        //Debug.Log($"Angle : {walkerModel.ViewAngle}");
        //Debug.Log($"PlayerMask : {walkerModel.PlayerMask.value}");
        //Debug.Log($"Obstacle : {walkerModel.ObstacleMask.value}");

        walkerFieldOfView.delay = new WaitForSeconds(walkerModel.Delay);

        // MonsterMovement
        foreach(var node in walkerModel.moveTransformList) {walkerMovement.moveTransformList.Add(node);}
        walkerMovement.MinimumStopDelay = walkerModel.MinimumStopDelay;
        walkerMovement.MaxStopDelay = walkerModel.MaxStopDelay;

        // 이벤트 구독 설정
        

        // 어그로 해제 딜레이 변수 초기화
        stopToMissing = new WaitForSeconds(walkerModel.StopToMissingDelay);

        // 플레이어한테 보이는 지에 대한 변수 초기화
        walkerModel.isObservedFromPlayer = false;
    }

   

    protected override void Find()
    {
        Debug.Log("발견 상태 수행 완료");
        // 추격으로 전환
        walkerModel.ChangeState(Monster.MonsterState.Chase);
    }

    private IEnumerator Chase()
    {
       
        while (walkerFieldOfView.visibleTargets.Count > 0)
        {
            // 빠른 추격 상태 동안 수행
            if (chaseTimer <= 5f && onRun)
            {
                Debug.Log($"추격 진행 시간 : {chaseTimer}, 빠른 추적 {onRun}");
                walkerMovement.Move(walkerFieldOfView.visibleTargets[0], walkerModel.ChaseFast);
                chaseTimer += walkerModel.Delay;
                yield return walkerFieldOfView.delay;
            }
            // 느린 추격 상태 동안 수행
            else if (chaseTimer <= 5f && !onRun)
            {
                Debug.Log($"추격 진행 시간 : {chaseTimer}, 빠른 추적 {onRun}");
                walkerMovement.Move(walkerFieldOfView.visibleTargets[0], walkerModel.ChaseSlow);
                chaseTimer += walkerModel.Delay;
                yield return walkerFieldOfView.delay;
            }
            // 5초가 지나면 onRun을 전환하고 chase Timer 초기화
            else if (chaseTimer > 5f && onRun)
            {
                Debug.Log("타이머 초기화 느린 추적 실행");
                onRun = false;
                chaseTimer = 0;
            }
            else if (chaseTimer > 5f && !onRun)
            {
                Debug.Log("타이머 초기화 빠른 추적 실행");
                onRun = false;
                onRun = true;
                chaseTimer = 0;
            }
        }
    }

    private void StartChase()
    {
        Debug.Log("추격 시작");
        StartCoroutine(Chase());
    }

    private void StartMissingPlayer()
    {
        // 어그로가 해제되면 상태를 바꿈 + 추격 멈춤
        Debug.Log("플레이어가 시야에서 사라졌습니다.");
        StartCoroutine(MissingPlayer());
    }
    private IEnumerator MissingPlayer()
    {
        Debug.Log("MissingPlayer 수행");
        walkerMovement.StopToMissing();
        checkTimer = 0;
        while(walkerFieldOfView.visibleTargets.Count == 0 && walkerModel.monsterState == Monster.MonsterState.MissingPlayer)
        {
            // 플레이어를 다시 찾으면 추격 상태 전환
            if(walkerFieldOfView.visibleTargets.Count == 0 && checkTimer < 2)
            {
                Debug.Log($"놓친 시간 : {checkTimer}");
                checkTimer += 0.2f;
                yield return new WaitForSeconds(0.2f);
            }
            //플레이어를 다시 못찾으면 타이머를 진행
            else
            {
                Debug.Log("배회로 다시 전환합니다.");
                checkTimer = 0;
                walkerModel.ChangeState(Monster.MonsterState.WanderingAround);
            }
            
        }
        // 초기화 및 추격 진행
        checkTimer = 0;
        walkerModel.StartCoroutine(Chase());
    }

    protected override void StartPatrol()
    {
        Debug.Log("배회 실행");
        targetTransform = null;
        walkerMovement.ChangeSpeed(walkerModel.PatrolSpeed);
        walkerMovement.PatrolNextOne();
    }

    protected override void StartAlerted()
    {
        walkerMovement.Move(targetTransform, walkerModel.PatrolSpeed);
    }
}
