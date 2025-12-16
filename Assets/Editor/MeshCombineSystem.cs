using System.Collections.Generic;
using UnityEngine;

public static class MeshCombineSystem
{
    public static void Combine(Transform root)
    {
        //생성된 오브젝트들의 머티리얼 지정할 메쉬를 구분지을 Dictionary
        Dictionary<Material, List<MeshFilter>> groups =
            new Dictionary<Material, List<MeshFilter>>();

        MeshFilter[] filters = root.GetComponentsInChildren<MeshFilter>();

        var combine = new List<CombineInstance>();

        foreach (MeshFilter filter in filters)
        {
            if (filter.transform == root)
                continue;

            if (filter.sharedMesh == null)
                continue;

            MeshRenderer renderer = filter.GetComponent<MeshRenderer>();
            if (renderer == null || renderer.sharedMaterial == null)
                continue;

            Material mat = renderer.sharedMaterial;

            if (!groups.ContainsKey(mat))
                groups[mat] = new List<MeshFilter>();

            groups[mat].Add(filter);
        }

        foreach(var pair in groups)
        {
            CombineGroup(root, pair.Key, pair.Value);
        }
    }

    static void CombineGroup(Transform root, Material mat, List<MeshFilter> filters)
    {
        var combine = new List<CombineInstance>();

        foreach (var mf in filters)
        {
            CombineInstance ci = new CombineInstance
            {
                mesh = mf.sharedMesh,
                transform = mf.transform.localToWorldMatrix
            };

            combine.Add(ci);
            mf.gameObject.SetActive(false);
        }

        if (combine.Count == 0)
            return;

        Mesh combinedMesh = new Mesh();
        combinedMesh.CombineMeshes(combine.ToArray(), true);

        GameObject combined = new GameObject("Combined_" + mat.name);
        combined.transform.SetParent(root, true);

        combined.AddComponent<MeshFilter>().sharedMesh = combinedMesh;
        combined.AddComponent<MeshRenderer>().sharedMaterial = mat;

        combined.isStatic = true;
    }
}
