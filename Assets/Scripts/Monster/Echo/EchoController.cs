using System.Collections;
using UnityEngine;

public class EchoController : MonsterController
{
    [SerializeField] private EchoView echoView;
    [SerializeField] private float rotateSpeed;
    private EchoModel echoModel;
    private EchoSpawnSystem echoSpawnSystem;

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
        echoSpawnSystem = FindFirstObjectByType<EchoSpawnSystem>();
        // 플레이어 태그를 통해서 현재 씬에 있는 플레이어를 불러와서 컴포넌트 추가
        GameObject[] targets = GameObject.FindGameObjectsWithTag("Player");
        foreach (GameObject players in targets)
        {
            playerState = players.GetComponent<PlayerStateMachine>();
            playerSanity = players.GetComponent<PlayerSanity>();
        }
        Init();
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
            playerState = other.GetComponentInParent<PlayerStateMachine>();
            playerSanity = other.GetComponentInParent<PlayerSanity>();
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
        echoModel.ChangeState(Monster.MonsterState.Observe);
        player = GameManager.Instance.Player.transform;
        // 몬스터 테스트 씬 테스트 용
        //player = FindAnyObjectByType<TempPlayerController>().transform;
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
        //Debug.Log($"playerSanity : {playerSanity}");
        //Debug.Log($"playerState : {playerState}");
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
