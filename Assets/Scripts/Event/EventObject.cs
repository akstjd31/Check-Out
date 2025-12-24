using UnityEngine;

public class EventObject : MonoBehaviour
{
    private const string startType = "enterCollider";
    private string startValue;

    private void Awake()
    {
        startValue = GetHierarchyPath(transform);
    }

    private void OnCollisionEnter(Collision col)
    {
        if (!col.gameObject.CompareTag("Player")) return;

        EventManager.Instance.ExecuteByStart(startType, startValue);
    }

    // 하이에라키 경로 추적 후 문자열 변환
    private string GetHierarchyPath(Transform current)
    {
        string path = current.name;

        while (current.parent != null)
        {
            current = current.parent;
            path = current.name + "/" + path;
        }

        return path;
    }
}
