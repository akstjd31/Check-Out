using UnityEngine;
using TMPro;

public class PlayerView : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI objNameText;       // 바라보고 있는 오브젝트
    [SerializeField] private TextMeshProUGUI staminaText;       // 스태미나
    [SerializeField] private TextMeshProUGUI sanityText;        //정신력
    [SerializeField] private TextMeshProUGUI interactionText;   // 상호 작용 부가 설명
    [SerializeField] private TextMeshProUGUI situationText;     // 플레이어 시야 상태 텍스트
    [SerializeField] private TextMeshProUGUI moneyText;

    public void UpdateStaminaText(int stamina) => staminaText.text = "Stamina: " + stamina;

    public void UpdateSanityText(int sanity) => sanityText.text = $"Sanity: {sanity}%";

    public void UpdateInteractionText(string prompt) => interactionText.text = prompt;

    public void UpdateObjNameText(string name) => objNameText.text = name;

    public void UpdatePlayerSituationText(string curSitu) => situationText.text = $"[{curSitu}]";

    public void UpdateMoneyText(int money) => moneyText.text = $"Money: {money.ToString("N0")}";
}
