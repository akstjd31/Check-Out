using UnityEngine;
using TMPro;

public class PlayerView : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI objNameText;       // 바라보고 있는 오브젝트
    [SerializeField] private TextMeshProUGUI staminaText;       // 스태미나
    [SerializeField] private TextMeshProUGUI sanityText;        //정신력
    [SerializeField] private TextMeshProUGUI interactionText;   // 상호 작용 부가 설명
    [SerializeField] private TextMeshProUGUI slotIndexText;     // 슬롯 인덱스 텍스트 (테스트용)
    [SerializeField] private TextMeshProUGUI situationText;     // 플레이어 시야 상태 텍스트

    public void UpdateStaminaText(int stamina) => staminaText.text = "Stamina: " + stamina;

    public void UpdateSanityText(int sanity) => sanityText.text = $"Sanity: {sanity}%";

    public void UpdateInteractionText(string text) => interactionText.text = text;

    public void UpdateObjNameText(string name) => objNameText.text = name;

    //public void UpdateSlotIndexText(int idx) => slotIndexText.text = $"index [{idx}]";

    public void UpdatePlayerSituationText(string text) => situationText.text = $"[{text}]";
}
