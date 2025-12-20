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

        input.OnScroll += QuickSlotFocusedByScroll;
        input.OnSelected += QuickSlotFocusedByButton;
    }

    // 마우스 휠을 이용한 퀵 슬롯 인덱스 변경 방식
    public void QuickSlotFocusedByScroll(int value)
    {
        int maxIndex = invenUI.GetInventoryMaxIndex();
        int nextIndex = focusedIndex + value;

        if (nextIndex < 0)
            nextIndex = maxIndex;
        else if (nextIndex > maxIndex)
            nextIndex = 0;

        ApplyFocusedSlot(nextIndex);
    }

    // 버튼(1 ~ 4)을 이용한 퀵 슬롯 인덱스 변경 방식
    public void QuickSlotFocusedByButton(int slotIndex)
    {
        int maxIndex = invenUI.GetInventoryMaxIndex();

        if (slotIndex < 0 || slotIndex > maxIndex)
            return;

        ApplyFocusedSlot(slotIndex);
    }

    // 공통 작업 (인벤토리 세팅)
    private void ApplyFocusedSlot(int index)
    {
        if (focusedIndex != index)
        {
            focusedIndex = index;
        }

        else
            focusedIndex = -1;

        InventoryManager.Instance.SelectInventory(focusedIndex);
        invenUI.SelectUI(focusedIndex);
        invenUI.UpdateAll();
    }
}
