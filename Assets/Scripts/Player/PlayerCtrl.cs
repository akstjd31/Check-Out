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
    private PlayerInput playerInput;
    private InputAction moveAction, runAction;
    private StatController statController;
    [SerializeField] private PlayerState currentState;
    private Vector3 moveInput;
    private float runStaminaTimer = 1f;
    private int staminaDrainRun = 20;           // 달릴때 초당 감소
    private bool isMoving;                      // 움직이고 있는지?
    private bool isRunning;                     // 달리기 중인지?
    private bool isExhausted;                   // 탈진 상태인지?
    private float exhaustTimer;                 // 탈진 지속시간
    [SerializeField] private Button sessionBtn; // 테스트용 버튼
    [SerializeField] private TextMeshProUGUI staminaText;
    
    private void Awake()
    {
        playerInput = this.GetComponent<PlayerInput>();
        statController = this.GetComponent<StatController>();

        moveAction = playerInput.actions["Move"];
        runAction = playerInput.actions["Run"];

        sessionBtn.onClick.AddListener(OnClickLoadingStateButton);
    }

    private void Start()
    {
        UpdateState(PlayerState.Idle);
    }

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
    }

    private void FixedUpdate()
    {
        Vector3 move = new Vector3(moveInput.x, 0, moveInput.y);
        this.transform.Translate(move * statController.CurrentMoveSpeed * Time.fixedDeltaTime);
    }

    private void Update()
    {
        staminaText.text = "Stamina: " + statController.CurrentStamina;
        
        if (isExhausted)
            StartExhaustTime();

        if (CanRun())
        {
            runStaminaTimer -= Time.deltaTime;
            
            if (runStaminaTimer <= 0f)
            {
                statController.ConsumeStamina(staminaDrainRun);
                runStaminaTimer = 1f;
            }

            if (statController.CurrentStamina <= 0f)
                EnterExhaust();
        }
        else
        {
            // 달릴 수 있는 상태가 아니면서 탈진하지 않았다면
            if (!isExhausted)
            {
                runStaminaTimer -= Time.deltaTime;
                
                if (runStaminaTimer <= 0f)
                {
                    statController.RecoverStamina(statController.CurrentRecoverStamina);
                    runStaminaTimer = 1f;
                }
            }
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
    public void UpdateState(PlayerState state)
    {
        currentState = state;
        statController.UpdateUsedValue(currentState);
    }
    
    // 현재 달릴 수 있는 상태인지?
    public bool CanRun() => !isExhausted && isRunning && isMoving && statController.IsRemainStamina();

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

    public void OnMoveCanceled(InputAction.CallbackContext context)
    {
        UpdateState(PlayerState.Idle);
        isMoving = false;
        moveInput = Vector3.zero;
    }

    public void OnRunPerformed(InputAction.CallbackContext context)
    {
        // 탈진 상태일 경우
        if (isExhausted) return;

        UpdateState(PlayerState.Run);
        isRunning = true;
    }

    public void OnRunCanceled(InputAction.CallbackContext context)
    {
        if (isMoving)
            UpdateState(PlayerState.Walk);
        else
        {
            UpdateState(PlayerState.Idle);
        }
        isRunning = false;
        runStaminaTimer = 1f;
    }

    // 탈진 상태의 시작
    private void EnterExhaust()
    {
        isExhausted = true;
        exhaustTimer = statController.GetDefaultExhaustTime();
        isRunning = false; // 강제 걷기
        Debug.Log("탈진");
        runStaminaTimer = 1f;
    }

    public void OnClickLoadingStateButton() => GameManager.Instance.ChangeState(GameState.Loading);
}
