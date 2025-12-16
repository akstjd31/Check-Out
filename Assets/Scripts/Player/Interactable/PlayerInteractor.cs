using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerInteractor : MonoBehaviour
{
    [SerializeField] private GameObject playerHead;
    [SerializeField] private float distance = 2f;

    private LayerMask interactiveMask;
    private Interactable interactableObj;

    private void Awake()
    {
        interactiveMask = LayerMask.GetMask("Interactive");
    }

    private void Update()
    {
        // 현재 보고 있는 오브젝트
        var currentObj = GetInteractable<Interactable>();

        // 전에 보던 오브젝트랑 현재 오브젝트가 다를경우
        if (currentObj != interactableObj)
        {
            if (interactableObj != null)
                interactableObj.OnFocusExit();
            
            interactableObj = currentObj;

            if (interactableObj != null)
                interactableObj.OnFocusEnter();
        }
    }

    // 플레이어 정면 상호작용가능 물체 감지
    public T GetInteractable<T>() where T : Component
    {
        Vector3 head = playerHead.transform.position;
        Vector3 direction = playerHead.transform.forward;
        RaycastHit hit;

        Debug.DrawRay(head, direction * distance, Color.red);

        if (Physics.Raycast(head, direction, out hit, distance, interactiveMask))
        {
            return hit.collider.GetComponentInParent<T>();
        }

        return null;
    }

    // 특정 키를 눌러 상호작용 호출
    public void OnInteract(InputAction.CallbackContext ctx)
    {
        if (!ctx.performed) return;

        interactableObj?.Interact();
    }
}
