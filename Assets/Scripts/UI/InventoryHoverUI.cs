using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class InventoryHoverUI : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI nameText;
    [SerializeField] private TextMeshProUGUI typeText;
    [SerializeField] private TextMeshProUGUI descText;
    [SerializeField] private TextMeshProUGUI priceText;
    [SerializeField] private Image itemImage;

    private string firstPriceText;

    private void Awake()
    {
        firstPriceText = priceText.text;
    }

    // 마우스가 위로 올라가져 있을때
    public void OnEnter(Transform pos, ItemInstance item, Sprite sprite)
    {
        this.transform.GetChild(0).gameObject.SetActive(true);
        nameText.text = item.itemdata.itemName;
        typeText.text = item.itemdata.itemType;
        descText.text = item.itemdata.itemDescription;
        priceText.text = firstPriceText;
        priceText.text = priceText.text.Replace("{price}", item.itemdata.sellPrice.ToString());
        itemImage.sprite = sprite;
        
        transform.position = pos.position;
    }

    // 마우스가 빠져나갔을 때
    public void OnExit()
    {
        this.transform.GetChild(0).gameObject.SetActive(false);
    }
}
