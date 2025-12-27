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
    private PlayerSoundController soundController;

    private void Awake()
    {
        input = this.GetComponent<PlayerInputHandler>();
        movement = this.GetComponent<PlayerMovement>();
        stamina = this.GetComponent<PlayerStamina>();
        state = this.GetComponent<PlayerStateMachine>();
        interactor = this.GetComponent<PlayerInteractor>();
        soundController = this.GetComponent<PlayerSoundController>();

        input.OnInteract += OnInteract;
    }

    private void FixedUpdate()
    {
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
            soundController.StopSound();
            return;
        }

        if (canRun)
        {
            state.ChangeState(PlayerState.Run);
        }
        else
        {
            state.ChangeState(PlayerState.Walk);
        }
        
        soundController.PlaySound(state.CurrentState);
    }

    private void OnInteract() => interactor.Interaction();
}
