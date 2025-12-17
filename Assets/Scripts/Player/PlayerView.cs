using UnityEngine;
using TMPro;

public class PlayerView : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI staminaText;
    [SerializeField] private TextMeshProUGUI interactionText;

    public void UpdateStaminaText(int stamina)
    {
        staminaText.text = "Stamina: " + stamina;
    }

    public void UpdateInteractionText(string text)
    {
        interactionText.text = text;
    }
}
