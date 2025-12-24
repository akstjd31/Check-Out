using System.Collections;
using UnityEngine;
using UnityEngine.AI;

public class MannequinController : MonsterController
{
    [SerializeField] private MannequinView mannequinView;
    private MannequinModel mannequinModel;
    private FieldOfView mannequinFieldOfView;
    private MonsterMovement mannequinMovement;
    private WaitForSeconds stopToMissing;
    private float chaseTimer;
    private float checkTimer;
    private bool onRun = true;

    private void Awake()
    {
        // 컴포넌트 추가
        mannequinView = GetComponent<MannequinView>();
        mannequinModel = GetComponent<MannequinModel>();
        mannequinFieldOfView = GetComponent<FieldOfView>();
        mannequinMovement = GetComponent<MonsterMovement>();
    }

    private void Start()
    {
        Init();
        Debug.Log("마네킹 컨트롤러 배회 시작");
        mannequinModel.monsterState = Monster.MonsterState.WanderingAround;
        mannequinMovement.ChangeSpeed(mannequinModel.PatrolSpeed);
        mannequinMovement.PatrolNextOne();
        StartCoroutine(mannequinFieldOfView.FindTargetsWithDelay());
        
    }

    private void Update()
    {
        //Debug.Log($"walkerState : {walkerModel.walkerState}");
        //Debug.Log(walkerFieldOfView.visibleTargets.Count > 0);
        //Debug.Log($"visibleTargets : {walkerFieldOfView.visibleTargets.Count}");
        //Debug.Log(walkerModel.walkerState != WalkerModel.WalkerState.Chase);
        // 만약 접근 / 정지 상태가 아닐 때 플레이어가 시야에 들어온다면 발견 상태 실행 후 추격 진행
        if (mannequinFieldOfView.visibleTargets.Count > 0 && mannequinModel.monsterState != Monster.MonsterState.Approach && mannequinModel.monsterState != Monster.MonsterState.Stop)
        {
            //Debug.Log("조건 만족");
            // 발견 상태 수행
            mannequinModel.ChangeState(Monster.MonsterState.FindPlayer);
        }
        if (mannequinFieldOfView.visibleTargets.Count > 0 && mannequinModel.monsterState == Monster.MonsterState.Approach && mannequinModel.isObservedFromPlayer)
        {
            //
            mannequinModel.ChangeState(Monster.MonsterState.Stop);
        }
        if (mannequinFieldOfView.visibleTargets.Count > 0 && mannequinModel.monsterState == Monster.MonsterState.Stop && !mannequinModel.isObservedFromPlayer)
        {
            //
            mannequinModel.ChangeState(Monster.MonsterState.Approach);
        }
        if (mannequinFieldOfView.visibleTargets.Count == 0 && mannequinModel.monsterState == Monster.MonsterState.Approach)
        {
            // 만약 플레이어를 시야에 놓치면 MissingPlayer 전환
            mannequinModel.ChangeState(Monster.MonsterState.MissingPlayer);
        }
    }
    private void OnEnable()
    {
        // 이벤트 구독 설정
        mannequinModel.OnWanderingAround += StartPatrol;
        mannequinModel.OnFindPlayer += Find;
        mannequinModel.OnApproach += StartApproach;
        mannequinModel.OnStop += StartStop;
        mannequinModel.OnMissingPlayer += StartMissingPlayer;
        mannequinModel.OnAlerted += StartAlerted;
    }

    private void OnDisable()
    {
        // 이벤트 구독 해제
        mannequinModel.OnWanderingAround -= StartPatrol;
        mannequinModel.OnFindPlayer -= Find;
        mannequinModel.OnApproach -= StartApproach;
        mannequinModel.OnStop -= StartStop;
        mannequinModel.OnMissingPlayer -= StartMissingPlayer;
        mannequinModel.OnAlerted -= StartAlerted;
    }

    private void Init()
    {
        // 모델 값을 해당 컴포넌트에 전달
        // Monster Field Of View
        //Debug.Log($"Rdaius : {walkerModel.ViewRadius}");
        //Debug.Log($"Angle : {walkerModel.ViewAngle}");
        //Debug.Log($"PlayerMask : {walkerModel.PlayerMask.value}");
        //Debug.Log($"Obstacle : {walkerModel.ObstacleMask.value}");

        mannequinFieldOfView.delay = new WaitForSeconds(mannequinModel.Delay);

        // MonsterMovement
        foreach(var node in mannequinModel.moveTransformList) {mannequinMovement.moveTransformList.Add(node);}
        mannequinMovement.MinimumStopDelay = mannequinModel.MinimumStopDelay;
        mannequinMovement.MaxStopDelay = mannequinModel.MaxStopDelay;

        // 이벤트 구독 설정
        

        // 어그로 해제 딜레이 변수 초기화

        // 플레이어한테 보이는 지에 대한 변수 초기화
        mannequinModel.isObservedFromPlayer = false;
    }

   

    protected override void Find()
    {
        Debug.Log("발견 상태 수행 완료");
        // 추격으로 전환
        if (mannequinModel.isObservedFromPlayer)
            mannequinModel.ChangeState(Monster.MonsterState.Stop);
        else
            mannequinModel.ChangeState(Monster.MonsterState.Approach);
    }

    private IEnumerator Approach()
    {
        while (mannequinFieldOfView.visibleTargets.Count > 0 && !mannequinModel.isObservedFromPlayer)
        {
            // 접근 상태 동안 수행
            Debug.Log("접근 시작");
            mannequinMovement.Move(mannequinFieldOfView.visibleTargets[0], mannequinModel.ApproachSpeed);
            yield return mannequinFieldOfView.delay;
        }
    }

    private void StartApproach()
    {
        Debug.Log("추격 시작");
        StartCoroutine(Approach());
    }
    private IEnumerator Stop()
    {
        while (mannequinModel.isObservedFromPlayer)
        {
            mannequinMovement.VelocityZero();
            // 시야에 확보된 상태일 경우 이동 불가
            Debug.Log("플레이어가 관찰 중입니다.");
            mannequinMovement.Move(targetTransform, 0f);
            yield return mannequinFieldOfView.delay;
        }
        mannequinModel.ChangeState(Monster.MonsterState.Approach);
    }

    private void StartStop()
    {
        Debug.Log("정지 상태 전환");
        StartCoroutine(Stop());
    }

    private void StartMissingPlayer()
    {
        // 어그로가 해제되면 상태를 바꿈 + 추격 멈춤
        Debug.Log("플레이어가 시야에서 사라졌습니다.");
        StartCoroutine(MissingPlayer());
    }

    /// <summary>
    /// 플레이어를 놓치게 되었을 경우, 다음 기능을 수행합니다.
    /// </summary>
    /// <returns></returns>
    private IEnumerator MissingPlayer()
    {
        Debug.Log("MissingPlayer 수행");
        mannequinMovement.StopToMissing();
        checkTimer = 0;
        while(mannequinFieldOfView.visibleTargets.Count == 0 && mannequinModel.monsterState == Monster.MonsterState.MissingPlayer)
        {
            // 플레이어를 다시 찾으면 추격 상태 전환
            if(mannequinFieldOfView.visibleTargets.Count == 0)
            {
                yield return new WaitForSeconds(0.2f);
                Debug.Log("배회로 다시 전환합니다.");
                checkTimer = 0;
                mannequinModel.ChangeState(Monster.MonsterState.WanderingAround);
            }
            
        }
        // 초기화 및 추격 진행
        checkTimer = 0;
        mannequinModel.StartCoroutine(Approach());
    }

    /// <summary>
    /// 배회 상태일 때 실행할 메서드입니다.
    /// </summary>
    protected override void StartPatrol()
    {
        Debug.Log("배회 실행");
        targetTransform = null;
        mannequinMovement.ChangeSpeed(mannequinModel.PatrolSpeed);
        mannequinMovement.PatrolNextOne();
    }

    /// <summary>
    /// 사이렌에 의한 경보 발동 시 해당 위치로 이동합니다.
    /// </summary>
    protected override void StartAlerted()
    {
        mannequinMovement.Move(targetTransform, mannequinModel.PatrolSpeed);
    }
}
