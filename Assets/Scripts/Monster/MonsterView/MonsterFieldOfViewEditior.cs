using UnityEngine;
using UnityEditor;


// 몬스터 필드 오브 뷰에 관련한 에디터입니다.
[CustomEditor(typeof(MonsterFieldOfView))]
public class MonsterFieldOfViewEditior : Editor
{

    private void OnSceneGUI()
    {
        // 관측된 오브젝트를 fov에 대입합니다.
        MonsterFieldOfView fov = (MonsterFieldOfView)target;
        Handles.color = Color.white;
        // 3D 공간에 시야각을 표현합니다.
        Handles.DrawWireArc(fov.transform.position, Vector3.up, Vector3.forward, 360, fov.viewRadius);
        // 몬스터가 바라보는 각도 설정
        Vector3 viewAngleA = fov.DirectionFromAngle (-fov.viewAngle / 2, false);
        Vector3 viewAngleB = fov.DirectionFromAngle (fov.viewAngle / 2, false);
        // 시야 각도를 에디터에서 볼 수 있도록 표현합니다.
        Handles.DrawLine(fov.transform.position, fov.transform.position + viewAngleA * fov.viewRadius);
        Handles.DrawLine(fov.transform.position, fov.transform.position + viewAngleB * fov.viewRadius);
        // 보이는 타겟을 대상으로 레이 캐스트를 볼 수 있도록 합니다.
        Handles.color = Color.red;
        foreach (Transform visibleTarget in fov.visibleTargets) { Handles.DrawLine(fov.transform.position, visibleTarget.position); }
    }
}
