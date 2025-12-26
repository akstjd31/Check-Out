using TMPro;
using Unity.VisualScripting;
using UnityEngine;

[RequireComponent(typeof(PlayerInputHandler))]
[RequireComponent(typeof(PlayerMovement))]
[RequireComponent(typeof(PlayerStamina))]
[RequireComponent(typeof(PlayerStateMachine))]
[RequireComponent(typeof(PlayerInteractor))]

// 각 플레이어에게 필요한 클래스들을 이곳에서 조율
public class PlayerCtrl : MonoBehaviour
{
    private PlayerInputHandler input;
    private PlayerMovement movement;
    private PlayerStamina stamina;
    private PlayerStateMachine state;
    private PlayerInteractor interactor;
    private SoundDistance soundDistance;

    private void Awake()
    {
        input = this.GetComponent<PlayerInputHandler>();
        movement = this.GetComponent<PlayerMovement>();
        stamina = this.GetComponent<PlayerStamina>();
        state = this.GetComponent<PlayerStateMachine>();
        interactor = this.GetComponent<PlayerInteractor>();
        soundDistance = this.GetComponent<SoundDistance>();

        input.OnInteract += OnInteract; 
    }

    private void Update()
    {
        // if (!FadeController.Instance.IsFadeEnded)
        //     return;

        HandleMovement();
        HandleState();
    }

    // 이동
    private void HandleMovement()
    {
        if (input.MoveInput == Vector3.zero)
            return;

        movement.Move(input.MoveInput);
    }

    // 상태 갱신을 위한 작업
    private void HandleState()
    {
        bool isMoving = input.MoveInput != Vector3.zero;
        bool canRun = input.IsRunPressed && !stamina.IsExhausted;

        stamina.UpdateStamina(canRun, isMoving);

        if (!isMoving)
        {
            state.ChangeState(PlayerState.Idle);
        }
        else if (canRun)
        {
            // 소리가 진행 중이 아닐 때 뛰는 소리 재생
            if (!soundDistance.IsPlaying())
            {
                soundDistance.PlayClip(0, false);
            }
            state.ChangeState(PlayerState.Run);
        }
        else
        {
            // 소리가 진행 중이 아닐 때 걷는 소리 재생
            if (!soundDistance.IsPlaying())
            {
                soundDistance.PlayClip(1, false);
            }
            state.ChangeState(PlayerState.Walk);
        }
    }

    private void OnInteract() => interactor.Interaction();
}
