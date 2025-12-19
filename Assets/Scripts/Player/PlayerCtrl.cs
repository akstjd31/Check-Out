using TMPro;
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

    private void Awake()
    {
        input = this.GetComponent<PlayerInputHandler>();
        movement = this.GetComponent<PlayerMovement>();
        stamina = this.GetComponent<PlayerStamina>();
        state = this.GetComponent<PlayerStateMachine>();
        interactor = this.GetComponent<PlayerInteractor>();

        input.OnInteract += OnInteract; 
    }

    private void Update()
    {
        HandleMovement();
        HandleState();
    }

    private void HandleMovement()
    {
        if (input.MoveInput == Vector3.zero)
            return;

        movement.Move(input.MoveInput);
    }

    private void HandleState()
    {
        bool isMoving = input.MoveInput != Vector3.zero;
        bool canRun = input.IsRunPressed && !stamina.IsExhausted;

        stamina.UpdateStamina(canRun, isMoving);

        if (!isMoving)
            state.ChangeState(PlayerState.Idle);
        else if (canRun)
            state.ChangeState(PlayerState.Run);
        else
            state.ChangeState(PlayerState.Walk);
    }

    private void OnInteract() => interactor.Interaction();
}
