using UnityEngine;
using UnityEngine.UI;

public class MainButtonSettings : MonoBehaviour
{
    [SerializeField] Button[] buttons;

    private void Awake()
    {
        if (buttons != null)
        {
            foreach (Button btn in buttons)
            {
                btn.onClick.AddListener(SoundManager.Instance.PlayUIButtonClickSound);
            }

            buttons[0].onClick.AddListener(GameManager.Instance.OnGameStartButton);

            buttons[1].onClick.AddListener(GameManager.Instance.OnGameDataLoadButton);
            
            buttons[2].onClick.AddListener(GameManager.Instance.OnGameExitButton);
        }
    }
}
