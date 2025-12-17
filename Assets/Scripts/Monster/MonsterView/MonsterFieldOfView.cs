using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class MonsterFieldOfView : MonoBehaviour
{
    public float viewRadius;
    // 각도 360으로 제한
    [Range(0, 360)]
    public float viewAngle;
    // 해당 레이어에 있는 오브젝트만 탐지하도록 입력받음
    public LayerMask playerMask;
    public LayerMask obstacleMask;

    private WaitForSeconds delay;

    public List<Transform> visibleTargets = new List<Transform>();

    private void Awake()
    {
        // 딜레이 0.2초로 설정
        delay = new WaitForSeconds(0.2f);
    }

    private void Start()
    {
        // 0.2초 간격으로 플레이어를 탐지합니다.
        StartCoroutine("FindTargetsWithDelay");
    }
    public Vector3 DirectionFromAngle(float angleInDegrees, bool angleIsGlobal)
    {
        // 각도가 글로벌이 아닐 경우 입력받은 각도를 글로벌로 변환
        if(!angleIsGlobal) { angleInDegrees += transform.eulerAngles.y; }
        return new Vector3(Mathf.Sin(angleInDegrees * Mathf.Deg2Rad), 0, Mathf.Cos(angleInDegrees * Mathf.Deg2Rad));
    }

    private void FindVisibleTarget()
    {
        // 매서드 시작 시 리스트를 초기화
        visibleTargets.Clear();
        // 타겟에 해당하는 레이어에 존재하는 객체만을 콜라이더에 저장
        Collider[] targetsInViewRadius = Physics.OverlapSphere(transform.position, viewRadius, playerMask);
        // 콜라이더에 들어간 객체를 차례로 수행 과정 진행
        for (int i = 0; i < targetsInViewRadius.Length; i++)
        {
            // 타겟의 트랜스폼 정보를 받아옴
            Transform target = targetsInViewRadius[i].transform;
            Vector3 directionToTarget = (target.position - transform.position).normalized;
            // 만약 타겟이 시야각 안에 있다면 다음 내용을 수행
            if(Vector3.Angle(transform.forward, directionToTarget) < viewAngle / 2)
            {
                // 타겟광의 거리를 확인
                float dstToTarget = Vector3.Distance (transform.position, target.position);
                // 몬스터와 플레이어 사이에 장애물이 없을 경우 다음 내용을 수행
                if( !Physics.Raycast (transform.position, directionToTarget, dstToTarget, obstacleMask))
                {
                    Debug.Log($"{targetsInViewRadius[i]} 발견");
                    visibleTargets.Add(target);
                }
            }
        }
    }

    private IEnumerator FindTargetsWithDelay()
    {
        while(true)
        {
            yield return delay;
            FindVisibleTarget();
        }
    }
}
