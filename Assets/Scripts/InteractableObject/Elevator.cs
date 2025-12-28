using UnityEngine;

public class Elevator : Interactable
{
    private void Awake()
    {
        audioSource = this.GetComponent<AudioSource>();
    }
    public override void OnFocusEnter()
    {
        currentText = promptText[0];
    }

    public override void OnFocusExit()
    {
        currentText = "";
    }

    public override void Interact()
    {
        SoundManager.Instance.PlayElevatorButtonClickSound();

        // 씬 이동
        GameManager.Instance.ChangeState(GameState.Loading);
    }
}
