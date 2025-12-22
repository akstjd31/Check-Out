using UnityEngine;

public abstract class MonsterController : MonoBehaviour
{
    public virtual void StartPatrol()
    {
    }

    public virtual void Find()
    {
    }
    public virtual void StartAlert()
    {
    }

    public virtual void StateChange(Monster.MonsterState state)
    {
    }
}
