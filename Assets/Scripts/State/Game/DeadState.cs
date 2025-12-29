using UnityEngine;

public class DeadState : IState
{
    private float timer;
    public void Enter()
    {
        FadeManager.Instance.FadeStartedInvoke();
        FadeManager.Instance.StartFadeIn();

        timer = 5f;
    }

    public void Update()
    {
        timer -= Time.deltaTime;

        if (timer < 0f)
        {
            GameManager.Instance.isGameOver = false;
            InventoryManager.Instance.ResetInventory();
            GameManager.Instance.ChangeState(GameState.Loading);
        }
    }

    public void Exit()
    {
        // FadeManager.Instance.FadeStartedInvoke();
        // FadeManager.Instance.StartFadeOut();
    }
}
