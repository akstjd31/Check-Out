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

    bool obstacleCheck = true;

    private bool GetRandomPosition(out Vector3 position)
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
        Vector3 eye = player.position + Vector3.up; // 플레이어 눈
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
