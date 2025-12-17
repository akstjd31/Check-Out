using TMPro;
using UnityEditor.Rendering;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public enum PlayerState
{
    Idle, Walk, Run
}

public class PlayerCtrl : MonoBehaviour
{
    [Header("Component")]
    [SerializeField] private PlayerView playerView;
    private PlayerInput playerInput;
    private InputAction moveAction, runAction, interactiveAction;
    private StatController statController;
    private PlayerInteractor playerInteractor;
    [SerializeField] private PlayerState currentState;

    [Header("Value")]
    private Vector3 moveInput;
    private float staminaTimer = 1f;                        // 스태미나가 감소 or 회복되는 시점은 1초 지난 후
    private int staminaDrainRun = 15;                       // 달릴때 초당 감소
    private bool isMoving;                                  // 움직이고 있는지?
    private bool isRunning;                                 // 달리기 중인지?
    private bool isExhausted;                               // 탈진 상태인지?
    private float exhaustTimer;                             // 탈진 지속시간
    private void Awake()
    {
        playerInput = this.GetComponent<PlayerInput>();
        statController = this.GetComponent<StatController>();
        playerInteractor = this.GetComponent<PlayerInteractor>();

        moveAction = playerInput.actions["Move"];
        runAction = playerInput.actions["Run"];
        interactiveAction = playerInput.actions["Interaction"];
    }

    private void Start()
    {
        UpdateState(PlayerState.Idle);
    }

    // 플레이어 인풋 구독 (걷기, 달리기)
    private void OnEnable()
    {
        if (moveAction != null)
        {
            moveAction.performed += OnMovePerformed;
            moveAction.canceled += OnMoveCanceled;
        }

        if (runAction != null)
        {
            runAction.performed += OnRunPerformed;
            runAction.canceled += OnRunCanceled;
        }

        if (interactiveAction != null)
        {
            interactiveAction.performed += InteractionKeyPerformed;
        }
    }

    private void FixedUpdate()
    {
        // 이동
        Vector3 move = new Vector3(moveInput.x, 0, moveInput.y);
        this.transform.Translate(move * statController.CurrentMoveSpeed * Time.fixedDeltaTime);
    }

    private void Update()
    {
        // 테스트용
        //staminaText.text = "Stamina: " + statController.CurrentStamina;
        
        if (Input.GetKeyDown(KeyCode.B))
        {
            GameManager.Instance.ChangeState(GameState.Loading);
        }


        // 탈진 상태일때 타이머 계산
        if (isExhausted)
            StartExhaustTime();

        // 달릴 수 있는 상태일때 스태미나 계산
        if (CanRun())
        {
            staminaTimer -= Time.deltaTime;
            
            if (staminaTimer <= 0f)
            {
                statController.ConsumeStamina(staminaDrainRun);
                staminaTimer = 1f;
            }

            if (statController.CurrentStamina <= 0f)
                EnterExhaust();
        }
        else
        {
            // 스태미나 회복 관련
            if (!isExhausted)
            {
                staminaTimer -= Time.deltaTime;
                
                if (staminaTimer <= 0f)
                {
                    statController.RecoverStamina(statController.CurrentRecoverStamina);
                    staminaTimer = 1f;
                }
            }
        }
    }

    // 플레이어 인풋 구독 해제 (걷기, 달리기)
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

        if (interactiveAction != null)
        {
            interactiveAction.performed -= InteractionKeyPerformed;
        }
    }

    // 탈진 상태
    private void StartExhaustTime()
    {
        exhaustTimer -= Time.deltaTime;

        if (exhaustTimer <= 0f)
        {
            isExhausted = false;
        }
    }

    // 상태 갱신 및 기본 수치 적용 (이동 속도, 스태미나 회복력)
    public void UpdateState(PlayerState state)
    {
        currentState = state;
        statController.UpdateUsedValue(currentState);
    }
    
    // 현재 달릴 수 있는 상태인지?
    public bool CanRun() => !isExhausted && isRunning && isMoving && statController.IsRemainStamina();

    // 이동 키를 입력받고 있을 때
    public void OnMovePerformed(InputAction.CallbackContext context)
    {
        if (!isRunning)
            UpdateState(PlayerState.Walk);
        else
        {
            if (!isExhausted)
                UpdateState(PlayerState.Run);
        }

        isMoving = true;
        moveInput = context.ReadValue<Vector3>();
    }

    // 이동 취소되었을 때
    public void OnMoveCanceled(InputAction.CallbackContext context)
    {
        UpdateState(PlayerState.Idle);
        isMoving = false;
        moveInput = Vector3.zero;
    }

    // 달리기 키(LeftShift)를 입력받고 있을 때
    public void OnRunPerformed(InputAction.CallbackContext context)
    {
        // 탈진 상태일 경우
        if (isExhausted) return;

        UpdateState(PlayerState.Run);
        isRunning = true;
    }

    // 달리기 취소
    public void OnRunCanceled(InputAction.CallbackContext context)
    {
        // 이동 중인 상태(방향키 입력은 계속 들어오는 상태)면 이동
        if (isMoving)
            UpdateState(PlayerState.Walk);
        else
        {
            UpdateState(PlayerState.Idle);
        }
        isRunning = false;
        staminaTimer = 1f;
    }

    // 상호작용
    public void InteractionKeyPerformed(InputAction.CallbackContext context)
    {
        playerInteractor.Interaction();
    }

    // 탈진 상태의 시작
    private void EnterExhaust()
    {
        isExhausted = true;
        exhaustTimer = statController.GetDefaultExhaustTime();
        isRunning = false; // 강제 걷기
        staminaTimer = 1f;
    }
}
