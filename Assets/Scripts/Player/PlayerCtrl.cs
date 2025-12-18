using TMPro;
using UnityEditor.Rendering;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public enum PlayerState
{
    Idle, Walk, Run
}
public enum PlayerSituation
{
    Safe, Normal, Dark, Chase, Invincible
}

public class PlayerCtrl : MonoBehaviour
{
    [Header("Component")]
    private PlayerInput playerInput;
    private InputAction moveAction, runAction, interactiveAction, scrollAction;
    private StatController statController;
    private PlayerInteractor playerInteractor;
    private PlayerAreaDetector currentAreaDetector;
    [SerializeField] private PlayerState currentState;
    [SerializeField] private PlayerSituation currentSituation;
    [SerializeField] private PlayerView playerView;          // 플레이어 뷰          

    [Header("Value")]
    private int slotIndex = 0;                              // 테스트용 인덱스
    private Vector3 moveInput;
    private float staminaTimer = 1f;                        // 스태미나가 감소 or 회복되는 시점은 1초 지난 후
    private bool isMoving;                                  // 움직이고 있는지?
    private bool isRunning;                                 // 달리기 중인지?
    private bool isExhausted;                               // 탈진 상태인지?
    private float exhaustTimer;                             // 탈진 지속시간
    private bool isInvincible;                             // 무적(쿨타임)
    private float invincibleTimer;
    private void Awake()
    {
        playerInput = this.GetComponent<PlayerInput>();
        statController = this.GetComponent<StatController>();
        playerInteractor = this.GetComponent<PlayerInteractor>();
        currentAreaDetector = this.GetComponent<PlayerAreaDetector>();
    
        moveAction = playerInput.actions["Move"];
        runAction = playerInput.actions["Run"];
        interactiveAction = playerInput.actions["Interaction"];
        scrollAction = playerInput.actions["Scroll"];
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

        if (scrollAction != null)
        {
            scrollAction.performed += OnMouseScrollChangeSlotIndex;
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
        //정신력이 0이 아닐시 작동
        if (statController.IsRemainSanity())
        {
            playerView.UpdateSanityText(statController.CurrentSanity);
            OnSanity();
            statController.ConsumeSanity(statController.CurrentSanityDps);
        }
        else
        {
            Debug.Log("GAME OVER");
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
                statController.ConsumeStamina(statController.CurrentRunStaminaCost);
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

        if (scrollAction != null)
        {
            scrollAction.performed -= OnMouseScrollChangeSlotIndex;
        }
    }

    private void OnMouseScrollChangeSlotIndex(InputAction.CallbackContext context)
    {
        float scrollValue = context.ReadValue<Vector2>().y;

        // 마우스 휠(업/다운)에 따른 슬롯 인덱스 변환 코드 작성 => 수정 필요 (임시 데이터)
        if (scrollValue > 0)
            slotIndex--;
        else if (scrollValue < 0)
            slotIndex++;
        
        if (slotIndex < 0)
            slotIndex = 3;
        else if (4 <= slotIndex)
            slotIndex = 0;

        playerView?.UpdateSlotIndexText(slotIndex);
    }

    // 플레이어 인풋 액션들 비활성화
    public void IgnoreInput()
    {
        moveAction.Disable();
        runAction.Disable();
        interactiveAction.Disable();
        scrollAction.Disable();

        // 새로운 액션이 추가되면 작성
    }

    // 플레이어 인풋 액션들 활성화
    public void ReleaseIgnoreInput()
    {
        moveAction.Enable();
        runAction.Enable();
        interactiveAction.Enable();
        scrollAction.Enable();

        // 새로운 액션이 추가되면 작성
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

    public void UpdateSituation(PlayerSituation situation)
    {
        currentSituation = situation;
        statController.UpdateSituationUsedValue(currentSituation);
    }

    // 현재 달릴 수 있는 상태인지?
    public bool CanRun() => !isExhausted && isRunning && isMoving && statController.IsRemainStamina();

    // 이동 키를 입력받고 있을 때
    public void OnMovePerformed(InputAction.CallbackContext context)
    {
        if (!isRunning)
        {
            SendMyPosition();
            UpdateState(PlayerState.Walk);
        }
        else
        {
            if (!isExhausted)
            {
                SendMyPosition();
                UpdateState(PlayerState.Run);
            }
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

        SendMyPosition();
        UpdateState(PlayerState.Run);
        isRunning = true;
    }

    // 달리기 취소
    public void OnRunCanceled(InputAction.CallbackContext context)
    {
        // 이동 중인 상태(방향키 입력은 계속 들어오는 상태)면 이동
        if (isMoving)
        {
            SendMyPosition();
            UpdateState(PlayerState.Walk);
        }
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

    //무적시간 :  피격시로 설정할려함
    private void UpdateInvincibleTimer()
    {
        if (!isInvincible)
            return;

        invincibleTimer -= Time.deltaTime;
        Debug.Log("무적발동");
        if (invincibleTimer <= 0f)
        {
            isInvincible = false;
        }
    }

    //플레이어 위치 보내기 (디버그용)
    public void SendMyPosition()
    {
        PlayerSoundEvent.OnFootstep?.Invoke(transform.position);
        Debug.Log(transform.position);
    }

    //안전지대에 진입 시
    public void OnSanity()
    {
        if (currentAreaDetector.IsSafe)
        {
            UpdateSituation(PlayerSituation.Safe);
            return;
        }
        OnUnsafeArea();
    }

    //안전지대에서 벗어날 시
    private void OnUnsafeArea()
    {
        //몬스터가 근쳐에 있으면 true
        if (currentAreaDetector.IsMonster)
        {
            OnChase();
            return;
        }
        else
        {
            OnLightOrDark();
        }

    }

    //몬스터가 근처에있을 때
    private void OnChase()
    {
        UpdateSituation(PlayerSituation.Chase);
        isInvincible = true;
        invincibleTimer = statController.CurrentInvincibilityTime;
    }

    //광원 유무에 따른 상태변화
    private void OnLightOrDark()
    {
        if (currentAreaDetector.IsLight)
        {
            UpdateSituation(PlayerSituation.Normal);
        }
        else
        {
            UpdateSituation(PlayerSituation.Dark);
        }
    }
}
