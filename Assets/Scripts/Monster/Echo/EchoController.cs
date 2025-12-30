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

    private WaitForSeconds respawnTime;

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
        echoModel.OnActiveFalse += echoSpawnSystem.StartRespawnEcho;

    }

    private void OnDisable()
    {
        // 구독 해제
        echoModel.OnEyeContact -= StartEyeContact;
        echoModel.OnActiveFalse -= echoSpawnSystem.StartRespawnEcho;
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
        echoModel.ChangeState(Monster.MonsterState.Observe);
        //player = FindAnyObjectByType<PlayerCtrl>().transform;
        // 몬스터 테스트 씬 테스트 용
        player = FindAnyObjectByType<TempPlayerController>().transform;
    }

    private void Init()
    {
        echoFieldOfView.delay = new WaitForSeconds(echoModel.Delay);
        respawnTime = new WaitForSeconds(echoModel.RespawnTime);
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

    // 에코를 비활성화하고 딜레이만큼 멈춘다음 스폰진행합니다.
    public void DisableEcho()
    {
        if(respawnTime == null) {respawnTime = new WaitForSeconds(echoModel.RespawnTime);}
        // 물리 충돌과 시야에서만 제외시킴
        gameObject.SetActive(false);
    }

    public void ActiveEcho()
    {
        if (echoSpawnSystem.GetRandomPosition(out var pos))
        {
            transform.position = pos;
            transform.LookAt(pos);
            gameObject.SetActive(true);
            echoModel.ChangeState(Monster.MonsterState.Observe);
        }

        else
        {
            Debug.Log("스폰 불가능 판정");
        }
    }

    void MonsterRotate(Transform player)
    {
        transform.forward = Vector3.Lerp(transform.forward, player.position - transform.position, rotateSpeed);
    }
}
