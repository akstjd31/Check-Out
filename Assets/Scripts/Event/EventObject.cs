using UnityEngine;

enum StartEventType
{
    EnterCollider,
    Interaction
}

public class EventObject : MonoBehaviour
{
    [SerializeField] private StartEventType startType;
    private string startValue;

    private void Start()
    {
        string cloneName = "(Clone)";
        // 뒤에 클론 붙어있다면 제거 후 저장
        if (this.name.Contains(cloneName))
            startValue = this.name.Substring(0, this.name.Length - cloneName.Length);
        else
            startValue = this.name;
    }

    private void OnTriggerEnter(Collider col)
    {
        if (!col.CompareTag("Player")) return;

        EventManager.Instance.OnEventTriggered(StartEventTypeToString(startType), startValue);
    }

    // 시작 이벤트 타입을 스트링으로 변환
    private string StartEventTypeToString(StartEventType type)
    {
        switch (type)
        {
            case StartEventType.EnterCollider:
                return "enterCollider";
            case StartEventType.Interaction:
                return "interaction";
        }

        return null;
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
