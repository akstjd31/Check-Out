using UnityEngine;

public class FindNavMeshLink : MonoBehaviour
{
    [SerializeField] private Door door;
    [SerializeField] ExitPoint exitPoint;

    private void OnTriggerEnter(Collider other)
    {
        if (other.TryGetComponent<ExitPoint>(out exitPoint))
        {
            if (!door.HasNavMeshLink())
                door.SetNavMeshLink(exitPoint.GetNavMeshLink());
        }
    }
}
