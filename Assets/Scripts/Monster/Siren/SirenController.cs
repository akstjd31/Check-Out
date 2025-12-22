using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SirenController : MonoBehaviour
{
    [SerializeField] private SirenView sirenView;
    private SirenModel sirenModel;
    private MonsterMovement sirenMovement;
    private float alertTimer;
    private SphereCollider sceramCollider;
    [SerializeField] private List<Monster> screamInMonster;
    private WaitForSeconds stopToMissing;
    private bool onScream = false;
    private WaitForSeconds delay;

    private void Awake()
    {
        // 컴포넌트 추가
        sirenView = GetComponent<SirenView>();
        sirenModel = GetComponent<SirenModel>();
        sirenMovement = GetComponent<MonsterMovement>();
        sceramCollider = transform.GetChild(0).GetComponentInChildren<SphereCollider>();
    }

    private void Start()
    {
        Init();
        Debug.Log("사이렌 컨트롤러 배회 시작");
        sirenModel.monsterState = Monster.MonsterState.WanderingAround;
        sirenMovement.ChangeSpeed(sirenModel.PatrolSpeed);
        Debug.Log(sirenModel.PatrolSpeed);
        sirenMovement.PatrolNextOne();
    }

    private void Update()
    {
        //Debug.Log($"visibleTargets : {sirenFieldOfView.visibleTargets.Count}");
        //Debug.Log($"sirenState : {sirenModel.monsterState}");
        //Debug.Log(sirenFieldOfView.visibleTargets.Count > 0);
        //Debug.Log(sirenModel.monsterState != Monster.MonsterState.Alert);
        // 만약 플레이어가 시야에 들어온다면 발견 상태 실행 후 추격 진행
        //if (sirenModel.monsterState != Monster.MonsterState.Alert)
        //{
        //    //Debug.Log("조건 만족");
        //    // 발견 상태 수행
        //    sirenModel.ChangeState(Monster.MonsterState.FindPlayer);
        //}
    }

    private void Init()
    {
        // 모델 값을 해당 컴포넌트에 전달
        // Monster Field Of View
        //Debug.Log($"Rdaius : {sirenModel.ViewRadius}");
        //Debug.Log($"Angle : {sirenModel.ViewAngle}");
        //Debug.Log($"PlayerMask : {sirenModel.PlayerMask.value}");
        //Debug.Log($"Obstacle : {sirenModel.ObstacleMask.value}");

        //sirenFieldOfView.viewRadius = sirenModel.ViewRadius;
        //sirenFieldOfView.viewAngle = sirenModel.ViewAngle;
        delay = new WaitForSeconds(sirenModel.Delay);

        // MonsterMovement
        foreach(var node in sirenModel.moveTransformList) { sirenMovement.moveTransformList.Add(node);}
        sirenMovement.MinimumStopDelay = sirenModel.MinimumStopDelay;
        sirenMovement.MaxStopDelay = sirenModel.MaxStopDelay;

        // 어그로 해제 딜레이 변수 초기화
        stopToMissing = new WaitForSeconds(sirenModel.StopToMissingDelay);

        // 플레이어한테 보이는 지에 대한 변수 초기화
        sirenModel.isObservedFromPlayer = false;

        // 비명내 몬스터리스트 초기화
        screamInMonster = new List<Monster>();

        // 비명질렀을 시 몬스터 호출 범위
        sceramCollider.radius = 0.5f * sirenModel.Distance;
    }

    private void OnEnable()
    {
        // 이벤트 구독 설정
        sirenModel.OnWanderingAround += StartPatrol;
        sirenModel.OnFindPlayer += Find;
        sirenModel.OnAlert += StartAlert;
    }

    private void OnDisable()
    {
        // 이벤트 구독 해제
        sirenModel.OnWanderingAround -= StartPatrol;
        sirenModel.OnFindPlayer -= Find;
        sirenModel.OnAlert -= StartAlert;
    }

    private void StartPatrol()
    {
        Debug.Log("배회 실행");
        sirenMovement.ChangeSpeed(sirenModel.PatrolSpeed);
        sirenMovement.PatrolNextOne();
    }

    private void Find()
    {
        Debug.Log("발견 상태 수행 완료");
        // 추격 상태 수행
        sirenModel.ChangeState(Monster.MonsterState.Alert);
    }

    private void OnTriggerStay(Collider other)
    {
        if (other.TryGetComponent<Monster>(out var monster))
        {
            if (screamInMonster.Contains(monster) == false)
                screamInMonster.Add(monster);
        }
    }

    private void OnTriggerExit(Collider other)
    {
         if (other.TryGetComponent<Monster>(out var monster))
         {
             screamInMonster.Remove(monster);
         }
    }

    // 플레이어 발견했을때 비명 지르는 상태로 진입
    private IEnumerator Alert()
    {
        
        while (true)
        {
            // 3초 동안 비명
            if (alertTimer <= 3f)
            {
                sirenMovement.Move(transform, 0f);
                Debug.Log("비명 지르는 중");

                Debug.Log($"현재 {screamInMonster.Count}마리 호출 중");

                foreach (var target in screamInMonster)
                {
                    if (target == null)
                        continue;

                    // 배회상태에서만 몬스터를 자기 위치로 불러옴
                    if (target.monsterState == Monster.MonsterState.WanderingAround)
                    {
                        target.GetComponent<WalkerController>().GetTransform(transform);
                        target.ChangeState(Monster.MonsterState.Alerted);
                    }
                }
                alertTimer += sirenModel.Delay;
                Debug.Log(alertTimer);
                yield return delay;
            }

            else
            {
                foreach (var target in screamInMonster)
                {
                    if (target == null)
                        continue;

                    // 배회상태에서 불려온 몬스터는 다시 배회상태로
                    if (target.monsterState == Monster.MonsterState.WanderingAround)
                    {
                        target.ChangeState(Monster.MonsterState.WanderingAround);
                    }
                }
                screamInMonster.Clear();
                yield return new WaitForSeconds(3f);
                sirenModel.ChangeState(Monster.MonsterState.WanderingAround);
                alertTimer = 0f;
                break;
            }
        }
    }

    private void StartAlert()
    {
        Debug.Log("비명 시작");
        StartCoroutine(Alert());
    }

    public void StateChange(Monster.MonsterState state)
    {
        if (sirenModel == null)
            sirenModel = GetComponent<SirenModel>();

        sirenModel.ChangeState(state);
    }
}
