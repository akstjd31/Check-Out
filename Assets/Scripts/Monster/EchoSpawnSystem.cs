using System.Collections;
using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;
using UnityEngine.AI;

public class EchoSpawnSystem : MonoBehaviour
{
    [SerializeField] GameObject echoPrefab;

    [SerializeField] float maxSanity = 29f;
    [SerializeField] float minSanity = 1f;

    [SerializeField] float minDistance; // 반지름 기준
    [SerializeField] float maxDistance;

    [SerializeField] int maxTry;

    [SerializeField] Transform player;

    [SerializeField] FieldOfView playerView;

    [SerializeField ]private LayerMask obstacleMask;

    private EchoModel echoModel;

    private Monster monster;

    private StatController statController;

    WaitForSeconds respawnTime;

    public bool spawnActivate = true;

    public bool isSpawn = false;

    private float currentSanity;

    public GameObject EchoPrefab { get { return echoPrefab; } }


    private void Awake()
    {
        statController = GetComponent<StatController>();
    }

    public void Init(GameObject inputPlayer, FieldOfView inputPlayerView)
    {
        if (player == null)
        {
            player = inputPlayer.transform;
            playerView = inputPlayerView.GetComponent<FieldOfView>();
        }
    }

    private void Update()
    {
        currentSanity = statController.CurrentSanityPercent;
        if (currentSanity <= maxSanity && currentSanity >= minSanity)
        {
            //debug.Log("에코 소환중");
            CheckEcho();
        }

        else if (currentSanity > maxSanity && echoModel != null)
        {
            //debug.Log("에코 비활성화");
            DisableEcho();
        }

    }

    public bool GetRandomPosition(out Vector3 position)
    {
        position = default;

        Vector3 playerPosition = player.position;

        for (int i = 0; i < maxTry; i++)
        {
            // 플레이어 기준 도넛 모양으로 랜덤 위치 뽑기
            Vector2 circle = Random.insideUnitCircle.normalized;
            float t = Random.value;
            float r = Mathf.Sqrt(Mathf.Lerp(minDistance * minDistance, maxDistance * maxDistance, t));

            Vector3 circlePosition = playerPosition + new Vector3(circle.x, 0f, circle.y) * r;

            // NavMesh 위에 있는지 확인
            if (!NavMesh.SamplePosition(circlePosition, out var hit, 1.0f, NavMesh.AllAreas))
                continue;

            Vector3 pos = hit.position;

            // 보이는지 체크
            bool invisible = IsOutFOV(pos) || HasObstacle(pos);

            if (!invisible)
                continue;

            position = pos;
            return true;
        }
        // 위치를 생성할 수 없으면 false 반환
        return false;
    }

    // 플레이어 시야 안에 있는지 확인
    private bool IsOutFOV(Vector3 pos)
    {
        Vector3 playerPosition = player.position;
        Vector3 randomPosition = pos - playerPosition;
        randomPosition.y = 0f;

        float angle = Vector3.Angle(player.forward, randomPosition.normalized);

        return angle > (playerView.viewAngle * 0.5f);
    }

    // 장애물 넘어에 있는지 확인
    private bool HasObstacle(Vector3 pos)
    {
        Vector3 eye = player.position + Vector3.up ; // 플레이어 눈
        Vector3 target = pos + Vector3.up; // 몬스터 높이

        Vector3 dir = (target - eye);
        float distance = dir.magnitude;
        dir /= distance;

        // 레이를 발사해서 장애물만 탐지
        return Physics.Raycast(eye, dir, distance, obstacleMask);
    }

    public void SpawnEcho()
    {
        if (GetRandomPosition(out var pos))
        {
            Instantiate(echoPrefab, pos, Quaternion.identity);
            echoModel = FindFirstObjectByType<EchoModel>(FindObjectsInactive.Include);
            respawnTime = new WaitForSeconds(echoModel.RespawnTime);
        }

        else
        {
            //debug.Log("스폰 불가능 판정");
        }
    }

    public void ActiveEcho()
    {
        if (GetRandomPosition(out var pos))
        {
            echoModel.transform.position = pos;
            echoModel.gameObject.SetActive(true);
            echoModel.ChangeState(Monster.MonsterState.Observe);
        }

        else
        {
            //debug.Log("스폰 불가능 판정");
        }
    }

    public void CheckEcho()
    {
        if (isSpawn == true) return;
        if (spawnActivate == false) return;

        isSpawn = true;
        spawnActivate = false;
        // 태그로 몬스터 반환
        var echo = FindAnyObjectByType<EchoController>(FindObjectsInactive.Include);

        if (echo != null)
        {
            ActiveEcho();
            return;
        }

        // 현재 씬에 에코가 없다면 에코를 소환
        SpawnEcho();
    }
    public IEnumerator StartSpawnCount()
    {
        yield return new WaitForSeconds(30f);
        spawnActivate = true;
    }

    // 에코를 비활성화하고 딜레이만큼 멈춘다음 스폰진행합니다.
    public void DisableEcho()
    {
        // 물리 충돌과 시야에서만 제외시킴
        isSpawn = false;
        echoModel.gameObject.SetActive(false);
        StartCoroutine(StartSpawnCount());
    }

    public void GetTarget(Monster monster)
    {
        this.monster = monster;
    }

#if UNITY_EDITOR
    private void OnDrawGizmosSelected()
    {
        if (player == null) return;

        Vector3 center = player.position;
        center.y = player.position.y + 0.05f;

        // 바깥 원 (max)
        Handles.color = new Color(1f, 0f, 0f, 0.8f);
        Handles.DrawWireDisc(center, Vector3.up, maxDistance);

        // 안쪽 원 (min)
        Handles.color = new Color(0f, 1f, 0f, 0.8f);
        Handles.DrawWireDisc(center, Vector3.up, minDistance);
    }
#endif

}
