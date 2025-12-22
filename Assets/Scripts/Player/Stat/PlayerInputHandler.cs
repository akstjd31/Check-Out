using System;
using UnityEngine;
using UnityEngine.InputSystem;

// 플레이어 입력 관련 클래스
public class PlayerInputHandler : MonoBehaviour
{
    [Header("Component")]
    private PlayerInput playerInput;
    private InputAction moveAction, runAction, interactAction, scrollAction, selectAction, dropAction;

    [Header("Value")]
    private string[] playerActions = new string[] { "Move", "Run", "Interact", "Scroll", "Select", "Drop"};
    public Vector3 MoveInput { get; private set; }
    public bool IsRunPressed { get; private set; }
    public event Action OnInteract;
    public event Action<int> OnScroll;              // 슬롯 변경 관련 이벤트 구독 필요
    public event Action<int> OnSelected;

    private void Awake()
    {
        playerInput = this.GetComponent<PlayerInput>();

        moveAction = playerInput.actions[playerActions[0]];
        runAction = playerInput.actions[playerActions[1]];
        interactAction = playerInput.actions[playerActions[2]];
        scrollAction = playerInput.actions[playerActions[3]];
        selectAction = playerInput.actions[playerActions[4]];
        dropAction = playerInput.actions[playerActions[5]];
    }

    private void Start()
    {
        IgnoreInput();
    }

    private void OnEnable()
    {
        moveAction.performed += OnMovePerformed;
        moveAction.canceled += OnMoveCanceled;

        runAction.performed += OnRunPerformed;
        runAction.canceled += OnRunCanceled;

        interactAction.performed += OnInteractKeyInput;

        scrollAction.performed += OnScrollWheel;

        selectAction.performed += OnSelectSlotInput;

        dropAction.performed += OnDropKeyInput;
    }

    private void Update()
    {
        if (GameManager.Instance.IsOpenedUI())
        {
            IgnoreInput();
            return;    
        }

        ReleaseIgnoreInput();
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
            interactAction.performed -= OnInteractKeyInput;
        }

        if (scrollAction != null)
        {
            scrollAction.performed -= OnScrollWheel;
        }

        if (selectAction != null)
        {
            selectAction.performed -= OnSelectSlotInput;
        }
    }

    // 입력 비활성화
    public void IgnoreInput()
    {
        moveAction.Disable();
        runAction.Disable();
        interactAction.Disable();
        scrollAction.Disable();
        selectAction.Disable();
    }

    // 입력 활성화
    public void ReleaseIgnoreInput()
    {
        moveAction.Enable();
        runAction.Enable();
        interactAction.Enable();
        scrollAction.Enable();
        selectAction.Enable();
    }

    public void OnMovePerformed(InputAction.CallbackContext ctx) => MoveInput = ctx.ReadValue<Vector3>();
    public void OnMoveCanceled(InputAction.CallbackContext ctx) => MoveInput = Vector3.zero;
    public void OnRunPerformed(InputAction.CallbackContext ctx) => IsRunPressed = true;
    public void OnRunCanceled(InputAction.CallbackContext ctx) => IsRunPressed = false;

    public void OnInteractKeyInput(InputAction.CallbackContext ctx)
    {
        OnInteract?.Invoke();
    }

    public void OnScrollWheel(InputAction.CallbackContext ctx)
    {
        float value = ctx.ReadValue<Vector2>().y;
        OnScroll?.Invoke(value > 0 ? 1 : -1);
    }

    public void OnSelectSlotInput(InputAction.CallbackContext ctx)
    {
        int slotIndex = int.Parse(ctx.control.name) - 1;
        OnSelected?.Invoke(slotIndex);
    }

    public void OnDropKeyInput(InputAction.CallbackContext ctx)
    {
        Debug.Log("G");
    }
}
