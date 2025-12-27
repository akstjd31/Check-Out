using System;
using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(StatController))]
// 플레이어 입력 관련 클래스
public class PlayerInputHandler : MonoBehaviour
{
    [Header("Component")]
    private Rigidbody rigid;
    private PlayerInput playerInput;
    private InputAction moveAction, runAction, interactAction, scrollAction, selectAction, dropAction , useAction;
    private StatController stat;

    [Header("Value")]
    private string[] playerActions = new string[] { "Move", "Run", "Interact", "Scroll", "Select", "Drop" , "Use" };
    public Vector3 MoveInput { get; private set; }
    public bool IsRunPressed { get; private set; }
    public event Action OnInteract;
    public event Action<int> OnScroll;              // 슬롯 변경 관련 이벤트 구독 필요
    public event Action<int> OnSelected;
    public event Action OnDrop;
    public event Action <string> OnUsedItem;
    

    private void Awake()
    {
        rigid = this.GetComponent<Rigidbody>();
        playerInput = this.GetComponent<PlayerInput>();
        stat = this.GetComponent<StatController>();

        moveAction = playerInput.actions[playerActions[0]];
        runAction = playerInput.actions[playerActions[1]];
        interactAction = playerInput.actions[playerActions[2]];
        scrollAction = playerInput.actions[playerActions[3]];
        selectAction = playerInput.actions[playerActions[4]];
        dropAction = playerInput.actions[playerActions[5]];
        useAction = playerInput.actions[playerActions[6]];
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

        useAction.performed += OnUseKeyInput;

        stat.OnDeath += IgnoreInput;

        FadeManager.Instance.OnFadeStarted += IgnoreInput;
        FadeManager.Instance.OnFadeEnded += ReleaseIgnoreInput;

    }

    private void Update()
    {
        if (GameManager.Instance.IsOpenedUI() && moveAction.enabled)
        {
            IgnoreInput();
            return;
        }
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

        if (useAction != null)
        {
            useAction.performed -= OnUseKeyInput;
        }

        stat.OnDeath -= IgnoreInput;

        if (FadeManager.Instance != null)
        {
            FadeManager.Instance.OnFadeStarted -= IgnoreInput;
            FadeManager.Instance.OnFadeEnded -= ReleaseIgnoreInput;
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
        useAction.Disable();
    }

    // 입력 활성화
    public void ReleaseIgnoreInput()
    {
        moveAction.Enable();
        runAction.Enable();
        interactAction.Enable();
        scrollAction.Enable();
        selectAction.Enable();
        useAction.Enable();
    }

    public void OnMovePerformed(InputAction.CallbackContext ctx) => MoveInput = ctx.ReadValue<Vector3>();
    public void OnMoveCanceled(InputAction.CallbackContext ctx)
    {
        MoveInput = Vector3.zero;
        rigid.linearVelocity = Vector3.zero;
    }

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
        OnDrop?.Invoke();
    }

    public void OnUseKeyInput(InputAction.CallbackContext ctx)
    {
        if (ctx.control.name == "leftButton")
        {
            OnUsedItem?.Invoke("LeftClick");
        }

        if (ctx.control.name == "r")
        {
            OnUsedItem?.Invoke("R");
        }
    }

    // 아이템 사용 키 메서드 필요
}
