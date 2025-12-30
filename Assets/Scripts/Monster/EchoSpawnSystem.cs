using System.Collections;
using UnityEditor;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.UIElements;

public class EchoSpawnSystem : MonoBehaviour
{
    [SerializeField] GameObject echoPrefab;

    [SerializeField] float minDistance; // 반지름 기준
    [SerializeField] float maxDistance;

    [SerializeField] int maxTry;

    [SerializeField] Transform player;

    [SerializeField] FieldOfView playerView;

    [SerializeField ]private LayerMask obstacleMask;

    private EchoModel echoModel;
    private EchoController echoController;
    private WaitForSeconds respawnDelay;
    bool obstacleCheck = true;

    public GameObject EchoPrefab { get { return echoPrefab; } }


    private void Awake()
    {
        //Init();
    }

    public void Init()
    {
        if(player == null)
        {
            player = GameManager.Instance.Player.transform;
            playerView = player.GetComponent<FieldOfView>();
        }

        echoController = FindFirstObjectByType<EchoController>();
        echoModel = FindFirstObjectByType<EchoModel>();
        respawnDelay = new WaitForSeconds(echoModel.RespawnTime);
    }


    public void Init(GameObject inputPlayer, FieldOfView inputPlayerView)
    {
        if (player == null)
        {
            player = inputPlayer.transform;
            playerView = inputPlayerView.GetComponent<FieldOfView>();
        }

        echoController = FindFirstObjectByType<EchoController>();
        echoModel = FindFirstObjectByType<EchoModel>();
        respawnDelay = new WaitForSeconds(echoModel.RespawnTime);
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
        }

        else
        {
            Debug.Log("스폰 불가능 판정");
        }
    }

    public void StartRespawnEcho()
    {
        StartCoroutine(RespawnEcho());
    }

    // 에코 리스폰 딜레이 후 다시 활성화
    private IEnumerator RespawnEcho()
    {
        Debug.Log("에코 리스폰 딜레이 시작");
        yield return respawnDelay;
        Debug.Log("에코 리스폰 딜레이 종료");
        ActiveEcho();
    }

    public void ActiveEcho()
    {
        if (GetRandomPosition(out var pos))
        {
            echoController.transform.position = pos;
            echoController.transform.LookAt(pos);
            echoController.gameObject.SetActive(true);
            echoModel.ChangeState(Monster.MonsterState.Observe);
        }

        else
        {
            Debug.Log("스폰 불가능 판정");
        }
    }

    public void CheckEcho()
    {
        Debug.Log(" CheckEcho를 수행합니다. ");
        // 태그로 몬스터 반환
        GameObject[] monsters = GameObject.FindGameObjectsWithTag("Monster");

        if (monsters.Length == 0)
        {
            Debug.Log("씬에 몬스터가 없습니다.");
        }

        foreach ( var monster in monsters)
        {
            EchoController echo = monster.GetComponent<EchoController>();

            if ( echo != null)
            {
                Debug.Log(" 에코를 발견했습니다. 에코를 리스폰합니다. ");
                StartRespawnEcho();
                return;
            }
        }

        // 현재 씬에 에코가 없다면 에코를 소환
        SpawnEcho();
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
