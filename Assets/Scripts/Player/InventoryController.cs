using System.Drawing;
using UnityEngine;

/// <summary>
/// 플레이어 인벤토리(퀵슬롯)를 담당하는 클래스
/// </summary>

[RequireComponent(typeof(PlayerInputHandler))]
public class InventoryController : MonoBehaviour
{
    private PlayerInputHandler input;
    private InventoryUI invenUI;
    private int focusedIndex = 0;

    private void Awake()
    {
        input = this.GetComponent<PlayerInputHandler>();
        invenUI = FindAnyObjectByType<InventoryUI>();
        
        input.OnScroll += QuickSlotFocused;
    }
    
    public void QuickSlotFocused(int value)
    {
        focusedIndex += value;
        if (focusedIndex < 0)
            focusedIndex = invenUI.GetInventoryMaxIndex();
        else if (focusedIndex > invenUI.GetInventoryMaxIndex())
            focusedIndex = 0;

        InventoryManager.Instance.SelectInventory(focusedIndex);
        invenUI.SelectUI(focusedIndex);
        invenUI.UpdateAll();
    }
}
