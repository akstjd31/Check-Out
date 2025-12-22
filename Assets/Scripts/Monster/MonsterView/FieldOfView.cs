using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.Rendering;

public class FieldOfView : MonoBehaviour
{
    public float viewRadius;
    // 각도 360으로 제한
    [Range(0, 360)]
    public float viewAngle;
    // 해당 레이어에 있는 오브젝트만 탐지하도록 입력받음
    public LayerMask targetMask;
    public LayerMask obstacleMask;

    public WaitForSeconds delay;
    // 현재 시야에 타겟이 있다면 위치 정보를 담아둘 장소
    public List<Transform> visibleTargets = new List<Transform>();

    // 시각뷰 시각화(테스트용)
    public float meshResolution;

    Mesh viewMesh;
    public MeshFilter viewMeshFilter;

    private void Awake()
    {
        viewMesh = new Mesh();
        viewMesh.name = "View Mesh";
        viewMeshFilter.mesh = viewMesh;
        // 딜레이 0.2초로 설정
        delay = new WaitForSeconds(0.2f);
        //viewRadius = 1;
    }

    private void Start()
    { 
        // 테스트를 위해 아래 매서드 호출은 남겨둡니다.
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
        //Debug.Log("시야 갱신");
        // 매서드 시작 시 리스트를 초기화
        visibleTargets.Clear();
        // 타겟에 해당하는 레이어에 존재하는 객체만을 콜라이더에 저장
        Collider[] targetsInViewRadius = Physics.OverlapSphere(transform.position, viewRadius, targetMask);
        // 콜라이더에 들어간 객체를 차례로 수행 과정 진행
        for (int i = 0; i < targetsInViewRadius.Length; i++)
        {
            // 타겟의 트랜스폼 정보를 받아옴
            Transform target = targetsInViewRadius[i].transform;
            Vector3 directionToTarget = (target.position - transform.position).normalized;
            // 만약 타겟이 시야각 안에 있다면 다음 내용을 수행
            if(Vector3.Angle(transform.forward, directionToTarget) < viewAngle / 2)
            {
                // 타겟과의 거리를 확인
                float dstToTarget = Vector3.Distance (transform.position, target.transform.position);
                // 자기자신과 타겟 사이에 장애물이 없을 경우 다음 내용을 수행
                if( !Physics.Raycast (transform.position, directionToTarget, dstToTarget, obstacleMask))
                {
                    Debug.Log($"{targetsInViewRadius[i]} 발견");
                    visibleTargets.Add(target);
                }
            }
        }
    }

    public IEnumerator FindTargetsWithDelay()
    {
        while(true)
        {
            yield return delay;
            FindVisibleTarget();
        }
    }

    //시각화
    void DrawFieldOfView()
    {
        int stepCount = Mathf.RoundToInt(viewAngle * meshResolution);
        float stepAngleSize = viewAngle / stepCount;
        List<Vector3> viewPoints = new List<Vector3>();
        ViewCastInfo prevViewCast = new ViewCastInfo();

        for (int i = 0; i <= stepCount; i++)
        {
            float angle = transform.eulerAngles.y - viewAngle / 2 + stepAngleSize * i;
            ViewCastInfo newViewCast = ViewCast(angle);

            // i가 0이면 prevViewCast에 아무 값이 없어 정점 보간을 할 수 없으므로 건너뛴다.
            if (i != 0)
            {
                bool edgeDstThresholdExceed = Mathf.Abs(prevViewCast.dst - newViewCast.dst) > edgeDstThreshold;

                // 둘 중 한 raycast가 장애물을 만나지 않았거나 두 raycast가 서로 다른 장애물에 hit 된 것이라면(edgeDstThresholdExceed 여부로 계산)
                if (prevViewCast.hit != newViewCast.hit || (prevViewCast.hit && newViewCast.hit && edgeDstThresholdExceed))
                {
                    Edge e = FindEdge(prevViewCast, newViewCast);

                    // zero가 아닌 정점을 추가함
                    if (e.PointA != Vector3.zero)
                    {
                        viewPoints.Add(e.PointA);
                    }

                    if (e.PointB != Vector3.zero)
                    {
                        viewPoints.Add(e.PointB);
                    }
                }
            }

            viewPoints.Add(newViewCast.point);
            prevViewCast = newViewCast;
        }

        int vertexCount = viewPoints.Count + 1;
        Vector3[] vertices = new Vector3[vertexCount];
        int[] triangles = new int[(vertexCount - 2) * 3];
        vertices[0] = Vector3.zero;
        for (int i = 0; i < vertexCount - 1; i++)
        {
            vertices[i + 1] = transform.InverseTransformPoint(viewPoints[i]);
            if (i < vertexCount - 2)
            {
                triangles[i * 3] = 0;
                triangles[i * 3 + 1] = i + 1;
                triangles[i * 3 + 2] = i + 2;
            }
        }
        viewMesh.Clear();
        viewMesh.vertices = vertices;
        viewMesh.triangles = triangles;
        viewMesh.RecalculateNormals();
    }

    private void LateUpdate()
    {
        if (viewMeshFilter == null)
            return;
        DrawFieldOfView();
    }

    public struct ViewCastInfo
    {
        public bool hit;
        public Vector3 point;
        public float dst;
        public float angle;

        public ViewCastInfo(bool _hit, Vector3 _point, float _dst, float _angle)
        {
            hit = _hit;
            point = _point;
            dst = _dst;
            angle = _angle;
        }
    }

    ViewCastInfo ViewCast(float globalAngle)
    {
        Vector3 dir = DirectionFromAngle(globalAngle, true);
        RaycastHit hit;
        if (Physics.Raycast(transform.position, dir, out hit, viewRadius, obstacleMask))
        {
            return new ViewCastInfo(true, hit.point, hit.distance, globalAngle);
        }
        else
        {
            return new ViewCastInfo(false, transform.position + dir * viewRadius, viewRadius, globalAngle);
        }
    }

    public struct Edge
    {
        public Vector3 PointA, PointB;
        public Edge(Vector3 _PointA, Vector3 _PointB)
        {
            PointA = _PointA;
            PointB = _PointB;
        }
    }

    public int edgeResolveIterations;
    public float edgeDstThreshold;

    Edge FindEdge(ViewCastInfo minViewCast, ViewCastInfo maxViewCast)
    {
        float minAngle = minViewCast.angle;
        float maxAngle = maxViewCast.angle;
        Vector3 minPoint = Vector3.zero;
        Vector3 maxPoint = Vector3.zero;

        for (int i = 0; i < edgeResolveIterations; i++)
        {
            float angle = minAngle + (maxAngle - minAngle) / 2;
            ViewCastInfo newViewCast = ViewCast(angle);
            bool edgeDstThresholdExceed = Mathf.Abs(minViewCast.dst - newViewCast.dst) > edgeDstThreshold;
            if (newViewCast.hit == minViewCast.hit && !edgeDstThresholdExceed)
            {
                minAngle = angle;
                minPoint = newViewCast.point;
            }
            else
            {
                maxAngle = angle;
                maxPoint = newViewCast.point;
            }
        }

        return new Edge(minPoint, maxPoint);
    }

}
