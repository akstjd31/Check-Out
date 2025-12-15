using UnityEngine;

public class PlayerInteractor : MonoBehaviour
{
    [SerializeField] private Camera playerHead;
    [SerializeField] private float distance = 2f;

    public T GetInteractable<T>() where T : Component
    {
        LayerMask interactiveMask = LayerMask.GetMask("Interactive");
        Vector3 head = playerHead.transform.position;
        Vector3 direction = playerHead.transform.forward;
        RaycastHit hit;

        if (Physics.Raycast(head, direction, out hit, distance, interactiveMask))
        {
            return hit.collider.GetComponentInParent<T>();
        }

        return null;
    }
}
