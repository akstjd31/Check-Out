using System;
using UnityEngine;
using UnityEngine.InputSystem;

// 플레이어 입력 관련 클래스
public class PlayerInputHandler : MonoBehaviour
{
    [Header("InputSystem")]
    private PlayerInput playerInput;
    private InputAction moveAction, runAction, interactiveAction, scrollAction;

    [Header("Value")]
    public Vector2 MoveInput { get; private set; }
    public bool IsRunPressed { get; private set; }
    public event Action OnInteract;
    public event Action<int> OnScroll;

    private void Awake()
    {
        playerInput = this.GetComponent<PlayerInput>();
    }

    private void OnEnable()
    {
        moveAction.performed += OnMovePerformed;
        moveAction.canceled += OnMoveCanceled;
        runAction.performed += OnRun;
        interactiveAction.performed += OnInteractKey;
        scrollAction.performed += OnInteractKey;
    }

    private void OnDisable()
    {
        moveAction.performed -= OnMovePerformed;
        moveAction.canceled -= OnMoveCanceled;
        runAction.performed -= OnRun;
        interactiveAction.performed -= OnInteractKey;
        scrollAction.performed -= OnInteractKey;
    }

    public void OnMovePerformed(InputAction.CallbackContext ctx) => MoveInput = ctx.ReadValue<Vector2>();
    public void OnMoveCanceled(InputAction.CallbackContext ctx) => MoveInput = Vector2.zero;
    public void OnRun(InputAction.CallbackContext ctx) => IsRunPressed = ctx.performed;
    
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
