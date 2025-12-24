using System.Collections;
using Unity.VisualScripting;
using UnityEngine;

public class EchoController : MonsterController
{
    [SerializeField] private EchoView echoView;
    [SerializeField] private float rotateSpeed;
    private EchoModel echoModel;

    private FieldOfView echoFieldOfView;
    private Vector3 directionToTarget;

    private PlayerStateMachine playerState;
    private PlayerSanity playerSanity;
    private Transform player;

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
        echoModel.OnEyeContact += StartEyeContact;
    }

    private void OnDisable()
    {
        // 구독 해제
        echoModel.OnEyeContact -= StartEyeContact;
    }

    private void Update()
    {
        transform.LookAt(new Vector3(player.transform.position.x, transform.position.y, player.transform.position.z));
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
        player = FindAnyObjectByType<PlayerCtrl>().transform;
    }

    private void Init()
    {
        echoFieldOfView.delay = new WaitForSeconds(echoModel.Delay);

        // 플레이어한테 보이는 지에 대한 변수 초기화
        echoModel.isObservedFromPlayer = false;
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

    void MonsterRotate(Transform player)
    {
        transform.forward = Vector3.Lerp(transform.forward, player.position - transform.position, rotateSpeed);
    }
}
