using System.Collections.Generic;
using Unity.AI.Navigation;
using UnityEngine;
using UnityEngine.AI;

public static class MeshCombineSystem
{
    public static void Combine(Transform root)
    {
        //생성된 오브젝트들의 머티리얼 지정할 메쉬를 구분지을 Dictionary
        Dictionary<Material, List<MeshFilter>> groups =
            new Dictionary<Material, List<MeshFilter>>();

        //합칠 대상이 될 오브젝트들의 Mesh들.
        MeshFilter[] filters = root.GetComponentsInChildren<MeshFilter>();

        //합칠 대상을 담을 리스트.
        var combine = new List<CombineInstance>();

        //모든 Mesh를 대상으로 합치기 실행
        foreach (MeshFilter filter in filters)
        {
            //필터의 부모가 root인 경우 넘김
            if (filter.transform == root)
                continue;
            
            //필터의 Mesh 확인
            if (filter.sharedMesh == null)
                continue;

            //해당 MeshFilter로부터 MeshRenderer을 받아옴.
            MeshRenderer renderer = filter.GetComponent<MeshRenderer>();
            //Renderer 또는 Renderer가 공유받은 머티리얼이 null인 경우 넘김
            if (renderer == null || renderer.sharedMaterial == null)
                continue;

            //머티리얼을 renderer의 머티리얼로 설정
            Material mat = renderer.sharedMaterial;

            //머티리얼을 TKey로 갖는 Dictionary가 존재하지 않으면 생성
            if (!groups.ContainsKey(mat))
                groups[mat] = new List<MeshFilter>();

            //생성되었거나 기존에 존재하던 Dictionary에 머티리얼에 맞게 해당 필터 투입
            groups[mat].Add(filter);
        }
        
        //생성 완료된 그룹을 대상으로 Mesh를 합침
        foreach(var pair in groups)
        {
            CombineGroup(root, pair.Key, pair.Value);
        }
    }

    /// <summary>
    /// 그룹에 맞게 Mesh를 합해주는 메서드입니다.
    /// </summary>
    /// <param name="root">부모가 될 오브젝트</param>
    /// <param name="mat">머티리얼</param>
    /// <param name="filters">메쉬 필터</param>
    static void CombineGroup(Transform root, Material mat, List<MeshFilter> filters)
    {
        //메쉬를 합치기 위한 정보를 담을 리스트 생성
        var combine = new List<CombineInstance>();

        foreach (var mf in filters)
        {
            CombineInstance ci = new CombineInstance
            {
                //프리팹 Mesh 기준으로 합침.
                mesh = mf.sharedMesh,
                //월드 기준 위치로 변경.
                transform = mf.transform.localToWorldMatrix
            };
            //합칠 대상에 추가
            combine.Add(ci);
            var renderer = mf.GetComponent<MeshRenderer>();
            if (renderer != null)
                renderer.enabled = false;
        }

        //합칠 대상이 없으면 반환
        if (combine.Count == 0)
            return;

        //합쳐진 메쉬를 생성하기 위한 변수
        Mesh combinedMesh = new Mesh();
        //리스트 내의 메쉬를 전부 합침.
        combinedMesh.CombineMeshes(combine.ToArray(), true);

        //합쳐진 메쉬를 게임오브젝트로 생성.
        GameObject combined = new GameObject("Combined_" + mat.name);
        //인자값으로 받은 부모 오브젝트의 자식으로 변경
        combined.transform.SetParent(root, true);


        combined.AddComponent<MeshFilter>().sharedMesh = combinedMesh;
        combined.AddComponent<MeshRenderer>().sharedMaterial = mat;

        //길 노드의 경우 NavMesh 대상에 추가하기 위한 콜라이더 작업 추가.
        if(combined.name.Contains("Node"))
        {
            combined.AddComponent<MeshCollider>().sharedMesh = combinedMesh;
        }

        //방과 탈출구에 관련된 타일 또는 벽은, AI가 적용된 대상이 이동할 수 없도록 NavMeshModifier을 통해 걷지 못하는 구역으로 분리.
        if (combined.name.Contains("Room") || combined.name.Contains("Exit"))
        {
            var modifier = combined.AddComponent<NavMeshModifier>();
            modifier.overrideArea = true;
            modifier.area = NavMesh.GetAreaFromName("Not Walkable");
        }
        else
        {
            Debug.Log("NavMash를 필요로 하지 않는 오브젝트의 생성 완료.");
        }
            combined.isStatic = true;
    }
}
