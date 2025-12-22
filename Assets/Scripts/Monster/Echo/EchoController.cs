using System.Collections;
using UnityEngine;
using static UnityEngine.GraphicsBuffer;

public class EchoController : MonsterController
{
    [SerializeField] private EchoView echoView;
    private EchoModel echoModel;
<<<<<<< Updated upstream
    private EchoController echoController;
    private FieldOfView echoFieldOfView;

    // 플레이어를 저장할 변수
    private GameObject playerTransform;
    private PlayerStateMachine playerStateMachine;
    // 플레이어와의 거리를 저장할 변수
    private float distanceToTarget;
    // 플레이어를 강제로 어둠 상태로 전환하는 시간을 담을 변수
    private WaitForSeconds holdingTime;
    private WaitForSeconds spawnTime;
    // PlayerSituation를 임시로 저장할 변수
    private PlayerSituation tempPlayerSituation;
    private void Awake()
    {
        //컴포넌트 추가
        echoModel = GetComponent<EchoModel>();
        echoView = GetComponent<EchoView>();
        echoController = GetComponent<EchoController>();
        echoFieldOfView = GetComponent<FieldOfView>();
        playerStateMachine = GetComponent<PlayerStateMachine>();
=======
    private FieldOfView echoFieldOfView;
    private Vector3 directionToTarget;

    private void Awake()
    {
        // 컴포넌트 추가
        echoModel = GetComponent<EchoModel>();
        echoView = GetComponent<EchoView>();
        echoFieldOfView = GetComponent<FieldOfView>();
>>>>>>> Stashed changes
    }

    private void OnEnable()
    {
<<<<<<< Updated upstream
        echoModel.OnObserve += StartObservePlayer;
        echoModel.OnEyeContact += StartEyeContactPlayer;
        echoModel.OnContactPlayer += StartContactPlayer;
=======
        //구독 설정
        echoModel.OnObserve += StartObservePlayer;
>>>>>>> Stashed changes
    }

    private void OnDisable()
    {
<<<<<<< Updated upstream
        echoModel.OnObserve -= StartObservePlayer;
        echoModel.OnEyeContact -= StartEyeContactPlayer;
        echoModel.OnContactPlayer -= StartContactPlayer;
=======
        // 구독 해제
        echoModel.OnObserve -= StartObservePlayer;
>>>>>>> Stashed changes
    }

    private void Start()
    {
        Init();
<<<<<<< Updated upstream
=======
        echoModel.ChangeState(Monster.MonsterState.Observe);
>>>>>>> Stashed changes
    }

    private void Init()
    {
<<<<<<< Updated upstream
        // 모델 값을 해당 컴포넌트에 전달
        holdingTime = new WaitForSeconds(echoModel.DarkSituationHoldingTime);
        spawnTime = new WaitForSeconds(echoModel.RespawnTime);
        // Field Of View
        echoFieldOfView.viewRadius = echoModel.ViewRadius;
        echoFieldOfView.viewAngle = echoModel.ViewAngle;
        echoFieldOfView.delay = new WaitForSeconds(echoModel.Delay);
        echoFieldOfView.targetMask = echoModel.PlayerMask;
        echoFieldOfView.obstacleMask = echoModel.ObstacleMask;

        // 플레이어한테 보이는 지에 대한 변수 초기화
        echoModel.isObservedFromPlayer = false;

        // 오브젝트 비활성화
        gameObject.SetActive(false);
=======
        // FieldOfView
        echoFieldOfView.viewRadius = echoModel.ViewRadius;
        echoFieldOfView.viewAngle = echoModel.ViewAngle;
        echoFieldOfView.delay = new WaitForSeconds(echoModel.Delay);

        // 플레이어한테 보이는 지에 대한 변수 초기화
        echoModel.isObservedFromPlayer = false;
>>>>>>> Stashed changes
    }

    private void StartObservePlayer()
    {
<<<<<<< Updated upstream
        StartCoroutine(ObservePlayer());
    }
    private IEnumerator ObservePlayer()
    {
        while (true)
        {
            // 플레이어가 시야 안에 있다면 아래 내용 수행
            if(echoFieldOfView.visibleTargets.Count > 0)
            {
                distanceToTarget = Vector3.Distance(echoFieldOfView.visibleTargets[0].position,transform.position);
                // 플레이어와 에코가 일정 거리 만큼 가까우면 눈 마주침 상태로 전환
                if (distanceToTarget < echoModel.RangeOfRecognition)
                {
                    // 에코 상태 관찰로 전환 ( 스폰 구현되면 스폰 후 상태 전환할 예정 )
                    echoModel.ChangeState(Monster.MonsterState.Observe);
                }
            }

            yield return echoFieldOfView.delay;
        }
    }

    private void StartEyeContactPlayer()
    {
        StartCoroutine(EyeContactPlayer());
    }

    private IEnumerator EyeContactPlayer()
    {
        // 플레이어를 어둠 상황으로 변환 10초 후 관찰 상태로 변환
        tempPlayerSituation = playerStateMachine.CurrentSituation;
        playerStateMachine.ChangeSituation(PlayerSituation.Dark);
        yield return holdingTime;
        // 이후 다시 원래 상태로 돌아감
        playerStateMachine.ChangeSituation(tempPlayerSituation);
        
    }

    private void StartContactPlayer()
    {
        StartCoroutine(ContactPlayer());
    }

    private IEnumerator ContactPlayer()
    {
        // 에코 비활성화 후 다시 활성화
        gameObject.SetActive(true);
        yield return spawnTime;

        // 다시 관찰 상태로 전환 ( 스폰 구현되면 스폰 후 상태 전환할 예정 )
        echoModel.ChangeState(Monster.MonsterState.Observe);
    }
=======
        ObservePlayer();
    }

    private IEnumerator ObservePlayer()
    {
        while(true)
        {
            if(echoFieldOfView.visibleTargets.Count > 0)
            {
                // 플레이어가 시야에 있으면 플레이어를 바라봄
                transform.LookAt(echoFieldOfView.visibleTargets[0]);

                // 플레이어가 지정 범위 내에 있으면 상태 전환
                directionToTarget = (echoFieldOfView.visibleTargets[0].position - transform.position).normalized;

            }
            yield return echoFieldOfView.delay;
        }
    }
>>>>>>> Stashed changes
}
