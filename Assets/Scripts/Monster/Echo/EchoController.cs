using System.Collections;
using UnityEngine;
using UnityEngine.Playables;
using static UnityEngine.GraphicsBuffer;

public class EchoController : MonsterController
{
    [SerializeField] private EchoView echoView;
    private EchoModel echoModel;

    private FieldOfView echoFieldOfView;
    private Vector3 directionToTarget;

    private PlayerStateMachine playerState;
    private PlayerSanity playerSanity;

    public bool inRange;

    [SerializeField] private float darknessTime;

    private float secondTime = 0;

    private void Awake()
    {
        // 컴포넌트 추가
        echoModel = GetComponent<EchoModel>();
        echoView = GetComponent<EchoView>();
        echoFieldOfView = GetComponent<FieldOfView>();
    }

    private void OnEnable()
    {
        //구독 설정
        echoModel.OnObserve += StartObservePlayer;
        echoModel.OnEyeContact += StartEyeContact;
    }

    private void OnDisable()
    {
        // 구독 해제
        echoModel.OnObserve -= StartObservePlayer;
        echoModel.OnEyeContact -= StartEyeContact;
    }

    private void Update()
    {
        if (inRange && echoModel.isObservedFromPlayer && echoModel.monsterState != Monster.MonsterState.EyeContact)
        {
            echoModel.ChangeState(Monster.MonsterState.EyeContact);
        }    
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            playerState = other.GetComponent<PlayerStateMachine>();
            playerSanity = other.GetComponent<PlayerSanity>();
            inRange = true;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            inRange = false;
        }
    }

    private void Start()
    {
        Init();
        echoModel.ChangeState(Monster.MonsterState.Observe);
    }

    private void Init()
    {
        // FieldOfView
        echoFieldOfView.viewRadius = echoModel.ViewRadius;
        echoFieldOfView.viewAngle = echoModel.ViewAngle;
        echoFieldOfView.delay = new WaitForSeconds(echoModel.Delay);

        // 플레이어한테 보이는 지에 대한 변수 초기화
        echoModel.isObservedFromPlayer = false;
    }

    private void StartObservePlayer()
    {
        StartCoroutine(ObservePlayer());
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

    public void StartEyeContact()
    {
        StartCoroutine (EyeContact());
        
    }

    private IEnumerator EyeContact()
    {
        playerSanity.SetDarkness(true);
        while (secondTime < darknessTime)
        {
            //Debug.Log(secondTime);
            if (playerState.CurrentSituation != PlayerSituation.Dark)
                playerState.ChangeSituation(PlayerSituation.Dark);
            secondTime += echoModel.Delay;
            yield return new WaitForSeconds(echoModel.Delay);
        }
        playerSanity.SetDarkness(false);
        secondTime = 0;
        echoModel.ChangeState(Monster.MonsterState.Observe);
    }
}
