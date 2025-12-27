using Unity.AI.Navigation;
using UnityEngine;
using UnityEngine.AI;

public class ExitPoint : MonoBehaviour
{
    [SerializeField] private NavMeshLink navMeshLink;

    public void SetNavMeshLink(NavMeshLink navMeshLink) => this.navMeshLink = navMeshLink;
    public NavMeshLink GetNavMeshLink() => navMeshLink;
}
