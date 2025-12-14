using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class PlayerCtrl : MonoBehaviour
{
    private PlayerInput playerInput;
    private InputAction moveAction, interactiveAction;
    private Vector3 moveInput;
    [SerializeField] private float moveSpeed;
    [SerializeField] private Button sessionBtn;
    
    private void Awake()
    {
        playerInput = this.GetComponent<PlayerInput>();

        moveAction = playerInput.actions["Move"];
        interactiveAction = playerInput.actions["Interactive"];

        sessionBtn.onClick.AddListener(OnClickLoadingStateButton);
    }

    private void OnEnable() 
    {
        moveAction.performed += OnMovePerformed;
        moveAction.canceled += OnMoveCanceled;
        interactiveAction.started += OnInteractiveStarted;
    }

    private void FixedUpdate()
    {
        Vector3 move = new Vector3(moveInput.x, 0, moveInput.y);
        this.transform.Translate(move * moveSpeed * Time.fixedDeltaTime);
    }

    private void Update()
    {
        
    }

    private void OnDisable()
    {
        moveAction.performed -= OnMovePerformed;
        moveAction.canceled -= OnMoveCanceled;
        interactiveAction.started -= OnInteractiveStarted;
    }

    private void OnMovePerformed(InputAction.CallbackContext ctx) => moveInput = ctx.ReadValue<Vector3>();
    private void OnMoveCanceled(InputAction.CallbackContext ctx) => moveInput = Vector3.zero;

    private void OnInteractiveStarted(InputAction.CallbackContext ctx)
    {
        GameManager.Instance.ChangeState(GameState.Loading);
    }

    public void OnClickLoadingStateButton() => GameManager.Instance.ChangeState(GameState.Loading);
}
