using System;
using TMPro;
using UnityEditor.SceneManagement;
using UnityEngine;
using UnityEngine.UI;

public class HoverUI : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI nameText;
    [SerializeField] private TextMeshProUGUI typeText;
    [SerializeField] private TextMeshProUGUI descText;
    [SerializeField] private TextMeshProUGUI priceText;
    [SerializeField] private Image itemImage;

    public void OnEnter(Transform pos, ItemTableData data, Sprite sprite)
    {
        gameObject.SetActive(true);
        nameText.text = data.itemName;
        typeText.text = data.itemType;
        descText.text = data.itemDescription;
        string changePrice = priceText.text;
        changePrice = changePrice.Replace("{price}", data.sellPrice.ToString());
        priceText.text = changePrice;
        itemImage.sprite = sprite;
        nameText.text = data.itemName;
        
        transform.position = pos.position;
    }

    public void OnExit()
    {
        gameObject.SetActive(false);
    }
}
