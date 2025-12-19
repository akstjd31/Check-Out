using System.Collections;
using UnityEngine;
using UnityEngine.AI;

public class WalkerController : MonoBehaviour
{
    [SerializeField] private WalkerView walkerView;
    private WalkerModel walkerModel;
    private MonsterFieldOfView walkerFieldOfView;
    private MonsterMovement walkerMovement;
    private WaitForSeconds stopToMissing;
    private float chaseTimer;
    private float checkTimer;
    private bool onRun = true;

    

    private void Start()
    {
        Init();
        Debug.Log("워커 컨트롤러 배회 시작");
        walkerModel.walkerState = WalkerModel.WalkerState.WanderingAround;
        walkerMovement.ChangeSpeed(walkerModel.PatrolSpeed);
        walkerMovement.PatrolNextOne();
        StartCoroutine(walkerFieldOfView.FindTargetsWithDelay());
    }

    private void Update()
    {
        Debug.Log($"visibleTargets : {walkerFieldOfView.visibleTargets.Count}");
        Debug.Log($"walkerState : {walkerModel.walkerState}");
        Debug.Log(walkerFieldOfView.visibleTargets.Count > 0);
        Debug.Log(walkerModel.walkerState != WalkerModel.WalkerState.Chase);
        // 만약 플레이어가 시야에 들어온다면 발견 상태 실행 후 추격 진행
        if (walkerFieldOfView.visibleTargets.Count > 0 && walkerModel.walkerState != WalkerModel.WalkerState.Chase)
        {
            Debug.Log("조건 만족");
            // 발견 상태 수행
            walkerModel.ChangeState(WalkerModel.WalkerState.FindPlayer);
            //FindAndChase();
        }
    }

    private void Init()
    {
        // 컴포넌트 추가
        walkerView = GetComponent<WalkerView>();
        walkerModel = GetComponent<WalkerModel>();
        walkerFieldOfView = GetComponent<MonsterFieldOfView>();
        walkerMovement = GetComponent<MonsterMovement>();



        // 모델 값을 해당 컴포넌트에 전달
        // Monster Field Of View
        //Debug.Log($"Rdaius : {walkerModel.ViewRadius}");
        //Debug.Log($"Angle : {walkerModel.ViewAngle}");
        //Debug.Log($"PlayerMask : {walkerModel.PlayerMask.value}");
        //Debug.Log($"Obstacle : {walkerModel.ObstacleMask.value}");

        walkerFieldOfView.viewRadius = walkerModel.ViewRadius;
        walkerFieldOfView.viewAngle = walkerModel.ViewAngle;
        walkerFieldOfView.delay = new WaitForSeconds(walkerModel.Delay);

        // MonsterMovement
        foreach(var node in walkerModel.moveTransformList) {walkerMovement.moveTransformList.Add(node);}
        walkerMovement.MinimumStopDelay = walkerModel.MinimumStopDelay;
        walkerMovement.MaxStopDelay = walkerModel.MaxStopDelay;

        // 이벤트 구독 설정
        walkerModel.OnFindPlayer += Find;
        walkerModel.OnChase += StartChase;

        // 어그로 해제 딜레이 변수 초기화
        stopToMissing = new WaitForSeconds(walkerModel.StopToMissingDelay);

        // 플레이어한테 보이는 지에 대한 변수 초기화
        walkerModel.isObseredFromPlayer = false;
    }

    private IEnumerator FindAndChase()
    {
        Debug.Log("Find And Chase 실행");
        // 발견 상태 수행
        walkerModel.ChangeState(WalkerModel.WalkerState.FindPlayer);
        // 추격 상태 수행
        walkerModel.ChangeState(WalkerModel.WalkerState.Chase);
        while(walkerFieldOfView.visibleTargets.Count > 0)
        {
            // 빠른 추격 상태 동안 수행
            if(chaseTimer <= 5f && onRun)
            {
                walkerMovement.Move(walkerFieldOfView.visibleTargets[0], walkerModel.ChaseFast);
                chaseTimer += walkerModel.Delay;
                yield return walkerFieldOfView.delay;
            }
            // 느린 추격 상태 동안 수행
            else if(chaseTimer <= 5f && !onRun)
            {
                walkerMovement.Move(walkerFieldOfView.visibleTargets[0], walkerModel.ChaseSlow);
                chaseTimer += walkerModel.Delay;
                yield return walkerFieldOfView.delay;
            }
            // 5초가 지나면 onRun을 전환하고 chase Timer 초기화
            else if(chaseTimer > 5f && onRun)
            {
                onRun = false;
                chaseTimer = 0;
            }
            else if (chaseTimer > 5f && !onRun)
            {
                onRun = true;
                chaseTimer = 0;
            }

        }
        // 어그로가 해제되면 2초 동안 멈춤
        Debug.Log("플레이어가 시야에서 사라졌습니다.");
        walkerModel.ChangeState(WalkerModel.WalkerState.MissingPlayer);
        walkerMovement.StopToMissing();
        yield return stopToMissing;
        // stopToMissing만큼 대기 후 배회 상태로 변환
        walkerModel.ChangeState(WalkerModel.WalkerState.WanderingAround);
        walkerMovement.PatrolNextOne();
    }

    private void Find()
    {
        Debug.Log("발견 상태 수행");
        // 발견 상태 수행
        walkerModel.ChangeState(WalkerModel.WalkerState.FindPlayer);
        // 추격 상태 수행
        walkerModel.ChangeState(WalkerModel.WalkerState.Chase);
    }

    private IEnumerator Chase()
    {
        while (walkerFieldOfView.visibleTargets.Count > 0)
        {
            // 빠른 추격 상태 동안 수행
            if (chaseTimer <= 5f && onRun)
            {
                walkerMovement.Move(walkerFieldOfView.visibleTargets[0], walkerModel.ChaseFast);
                chaseTimer += walkerModel.Delay;
                yield return walkerFieldOfView.delay;
            }
            // 느린 추격 상태 동안 수행
            else if (chaseTimer <= 5f && !onRun)
            {
                walkerMovement.Move(walkerFieldOfView.visibleTargets[0], walkerModel.ChaseSlow);
                chaseTimer += walkerModel.Delay;
                yield return walkerFieldOfView.delay;
            }
            // 5초가 지나면 onRun을 전환하고 chase Timer 초기화
            else if (chaseTimer > 5f && onRun)
            {
                onRun = false;
                chaseTimer = 0;
            }
            else if (chaseTimer > 5f && !onRun)
            {
                onRun = true;
                chaseTimer = 0;
            }
            // 플레이어를 놓치면 MissingPlayer 수행
            else { walkerModel.ChangeState(WalkerModel.WalkerState.MissingPlayer); }
        }
    }

    private void StartChase()
    {
        Debug.Log("추격 시작");
        StartCoroutine(Chase());
    }

    private IEnumerator MissingPlayer()
    {
        // 어그로가 해제되면 상태를 바꿈 + 추격 멈춤
        Debug.Log("플레이어가 시야에서 사라졌습니다.");
        walkerModel.ChangeState(WalkerModel.WalkerState.MissingPlayer);
        walkerMovement.StopToMissing();
        StopCoroutine(Chase());
        checkTimer = 0;
        while(true)
        {
            // 플레이어를 다시 찾으면 추격 상태 전환
            if(walkerFieldOfView.visibleTargets.Count > 0)
            {
                checkTimer = 0;
                walkerModel.StartCoroutine(Chase());
            }
            //플레이어를 다시 못찾으면 타이머를 진행
            else if(walkerFieldOfView.visibleTargets.Count == 0 && checkTimer < 2)
            {
                checkTimer += 0.2f;
                yield return new WaitForSeconds(0.2f);
            }
            // 플레이어를 못 찾은 상태로 2초가 지나면 배회로 전환
            else if(walkerFieldOfView.visibleTargets.Count == 0 && checkTimer >= 2)
            {
                Debug.Log("");
                checkTimer = 0;
                walkerModel.ChangeState(WalkerModel.WalkerState.WanderingAround);
            }
        }
    }
}
