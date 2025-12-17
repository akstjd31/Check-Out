using UnityEngine;
using TMPro;

public class PlayerView : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI objNameText;       // 바라보고 있는 오브젝트
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

    public void UpdateObjNameText(string name)
    {
        objNameText.text = name;
    }
}
