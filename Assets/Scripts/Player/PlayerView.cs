using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class PlayerView : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI objNameText;       // 바라보고 있는 오브젝트
    [SerializeField] private TextMeshProUGUI staminaText;       // 스태미나
    [SerializeField] private TextMeshProUGUI sanityText;        //정신력

    [SerializeField] private GameObject interactionUI;

    [SerializeField] private TextMeshProUGUI interactionText;   // 상호 작용 부가 설명
    [SerializeField] private TextMeshProUGUI situationText;     // 플레이어 시야 상태 텍스트
    [SerializeField] private TextMeshProUGUI moneyText;

    [SerializeField] private GameObject[] keyNotices; // 0 = G키 1 = 좌클릭 2 = R키

    [SerializeField] private TextMeshProUGUI leftClickText;
    [SerializeField] private TextMeshProUGUI rKeyText;

    [SerializeField] private Slider sanitySlider;
    [SerializeField] private Slider staminaSlider;

    public void UpdateStaminaText(int stamina) => staminaText.text = "스태미나: " + stamina;

    public void UpdateSanityText(int sanity) => sanityText.text = $"정신력: {sanity}%";

    public void UpdateInteractionText(string prompt) => interactionText.text = prompt;

    public void UpdateinteractionUI(bool satate) => interactionUI.SetActive(satate);

    public void UpdateObjNameText(string name) => objNameText.text = name;

    public void UpdatePlayerSituationText(string curSitu) => situationText.text = $"[{curSitu}]";

    public void UpdateMoneyText(int money) => moneyText.text = $"돈: {money.ToString("N0")}";

    public void UpdateSanitySlider(int sanity) => sanitySlider.value = sanity;

    public void UpdateStaminaSlider(int stamina) => staminaSlider.value = stamina;

    public void UpdateKeyNotice(ItemInstance item)
    {
        foreach (var keyNotice in keyNotices)
        {
            keyNotice.SetActive(false);
        }

        if (item == null) return;

        keyNotices[0].SetActive(true);

        foreach (var effect in item.effects)
        {
            switch (effect.ControlKey)
            {
                case "LeftClick":
                    keyNotices[1].SetActive(true);
                    leftClickText.text = $"{item.itemdata.itemName} 사용하기";
                    break;
                case "R":
                    keyNotices[2].SetActive(true);
                    rKeyText.text = $"{item.itemdata.itemName} 충전하기";
                    break;
            }
        }

    }


}
