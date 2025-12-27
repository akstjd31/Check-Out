using UnityEngine;

public class PlayerInteractor : MonoBehaviour
{
    private PlayerStatHolder holder;
    [SerializeField] private GameObject playerHead;     // 카메라

    [Header("Value")]
    [SerializeField] private float distance = 2f;
    // [SerializeField] LayerMask obstacleLayer;
    // [SerializeField] private LayerMask interactiveLayer;
    private int layerMask;
    private Interactable interactableObj;
    private string playerStatCanvasName = "PlayerStatCanvas";


    private void Awake()
    {
        holder = this.GetComponent<PlayerStatHolder>();

        layerMask = 7 << LayerMask.NameToLayer("Obstacle") | 8 << LayerMask.NameToLayer("Interactive");
    }

    private void Update()
    {
        // if (!GameManager.Instance.IsOpenedUI())
        // {
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

        holder.PlayerView.UpdateObjNameText(interactableObj == null ? "[null]" : $"[{interactableObj.name}]");
        holder.PlayerView.UpdateInteractionText(interactableObj?.GetCurrentText());
        // }
    }

    // 플레이어 정면 상호작용가능 물체 감지
    public T GetInteractable<T>() where T : Component
    {
        Vector3 head = playerHead.transform.position;
        Vector3 direction = playerHead.transform.forward;

        RaycastHit[] hits = Physics.RaycastAll(head, direction, distance, layerMask);
        
        foreach (RaycastHit hit in hits)
        {
            if (hit.collider.CompareTag("Interactive"))
                return hit.collider.GetComponentInParent<T>();
            else if (hit.collider.gameObject.layer.Equals("Obstacle"))
                continue;
        }

        return null;
    }

    public void Interaction()
    {
        interactableObj?.Interact();
    }
}
