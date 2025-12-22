using UnityEngine;

public abstract class MonsterController : MonoBehaviour
{
    public Transform targetTransform;

    protected virtual void StartPatrol()
    {
    }

    protected virtual void Find()
    {
    }
    protected virtual void StartAlerted()
    {
    }

    public void GetTransform(Transform transform)
    {
        targetTransform = transform;
    }
}
