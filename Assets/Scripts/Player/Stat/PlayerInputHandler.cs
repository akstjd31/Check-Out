using System;
using UnityEngine;
using UnityEngine.InputSystem;

// 플레이어 입력 관련 클래스
public class PlayerInputHandler : MonoBehaviour
{
    [Header("InputSystem")]
    private PlayerInput playerInput;
    private InputAction moveAction, runAction, interactAction, scrollAction;
    private string[] playerActions = new string[] { "Move", "Run", "Interact", "Scroll" };

    [Header("Value")]
    public Vector3 MoveInput { get; private set; }
    public bool IsRunPressed { get; private set; }
    public event Action OnInteract;
    public event Action<int> OnScroll;

    private void Awake()
    {
        playerInput = this.GetComponent<PlayerInput>();

        moveAction = playerInput.actions[playerActions[0]];
        runAction = playerInput.actions[playerActions[1]];
        interactAction = playerInput.actions[playerActions[2]];
        scrollAction = playerInput.actions[playerActions[3]];
    }

    private void OnEnable()
    {
        moveAction.performed += OnMovePerformed;
        moveAction.canceled += OnMoveCanceled;

        runAction.performed += OnRunPerformed;
        runAction.canceled += OnRunCanceled;

        interactAction.performed += OnInteractKey;
        scrollAction.performed += OnScrollWheel;
    }

    private void OnDisable()
    {
        if (moveAction != null)
        {
            moveAction.performed -= OnMovePerformed;
            moveAction.canceled -= OnMoveCanceled;
        }

        if (runAction != null)
        {
            runAction.performed -= OnRunPerformed;
            runAction.canceled -= OnRunCanceled;
        }

        if (interactAction != null)
        {
            interactAction.performed -= OnInteractKey;
        }

        if (scrollAction != null)
        {
            scrollAction.performed -= OnScrollWheel;
        }
    }

    public void OnMovePerformed(InputAction.CallbackContext ctx) => MoveInput = ctx.ReadValue<Vector3>();
    public void OnMoveCanceled(InputAction.CallbackContext ctx) => MoveInput = Vector3.zero;
    public void OnRunPerformed(InputAction.CallbackContext ctx) => IsRunPressed = true;
    public void OnRunCanceled(InputAction.CallbackContext ctx) => IsRunPressed = false;

    public void OnInteractKey(InputAction.CallbackContext ctx)
    {
        if (ctx.performed)
            OnInteract?.Invoke();
    }

    public void OnScrollWheel(InputAction.CallbackContext ctx)
    {
        float value = ctx.ReadValue<Vector2>().y;
        OnScroll?.Invoke(value > 0 ? -1 : 1);
    }
}
