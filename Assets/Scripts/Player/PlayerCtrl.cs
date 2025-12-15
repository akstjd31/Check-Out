using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public enum PlayerMoveState
{
    Idle, Walk, Run
}

public class PlayerCtrl : MonoBehaviour
{
    private PlayerStatHolder playerStatHolder;
    private PlayerStat stat;
    private Vector2 moveInput;
    private bool isExhausted;
    private bool isRunning;
    [SerializeField] private Button sessionBtn; // 테스트용 버튼
    
    private void Awake()
    {
        playerStatHolder = this.GetComponent<PlayerStatHolder>();
        stat = playerStatHolder.Stat;

        sessionBtn.onClick.AddListener(OnClickLoadingStateButton);
    }

    private void FixedUpdate()
    {
        Vector3 move = new Vector3(moveInput.x, 0, moveInput.y);
        this.transform.Translate(move * stat.MoveSpeed * Time.fixedDeltaTime);
    }

    private void Update()
    {
        
    }

    public void OnMove(InputAction.CallbackContext context)
    {
        moveInput = context.ReadValue<Vector2>();
    }

    public void OnRun(InputAction.CallbackContext context)
    {
        if (isExhausted) return;
        isRunning = context.ReadValueAsButton();
        Debug.Log("달리기");
    }

    // private void EnterExhaust()
    // {
    //     isExhausted = true;
    //     exhausttimer = exhaust;
    //     isRunning = false; // 강제 걷기
    //     Debug.Log("탈진");
    // }

    public void OnClickLoadingStateButton() => GameManager.Instance.ChangeState(GameState.Loading);
}
