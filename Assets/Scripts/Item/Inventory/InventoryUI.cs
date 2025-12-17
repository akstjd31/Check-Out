using UnityEngine;
using UnityEngine.UI;

public class InventoryUI : MonoBehaviour
{
    [SerializeField] GameObject uiPrefab;
    [SerializeField] GameObject[] uiObj;

    private int selectIndex;
    private Inventory inventory;

    void Start()
    {
        inventory = FindAnyObjectByType<Inventory>();

        if (inventory == null )
        {
            Debug.Log("UI - 인벤토리를 찾지 못했습니다");
        }
    }

    // 로딩 때 인벤토리 ui 세팅
    public void SetInventory(int size)
    {

        // 사이즈 기본 1 보장
        size = size <= 0 ? 1 : size;

        if (uiObj.Length > 0) return;

        uiObj = new GameObject[size];

        for (int i = 0; i < size; i++)
        {
            uiObj[i] = Instantiate(uiPrefab, transform);
            uiObj[i].name = $"Slot_{i + 1}";
            // 만들었다면 UI 업데이트 한번씩
        }

        int b = 0;

        foreach (var a in uiObj)
        {
            UpdateUI(b++);
        }

        SelectUI(0);
    }

    // UI가 변경 되었을때
    public void UpdateUI(int index)
    {
        Image ItemImage = uiObj[index].transform.GetChild(0).GetComponent<Image>();
        Image slotImage = uiObj[index].transform.GetComponent<Image>();

        if (slotImage == null) return;

        if (ItemImage == null) return;

        if (inventory == null) return;

        if (selectIndex != index)
        {
            slotImage.rectTransform.sizeDelta = new Vector2(100, 100);
        }

        if (inventory.slots[index] == null)
        {
            ItemImage.sprite = null;
            return;
        }

        Sprite sprite = Resources.Load<Sprite>(inventory.slots[index].imgPath);
        ItemImage.sprite = sprite;
    }

    // UI가 선택되었을때 UI업데이트
    public void SelectUI(int index)
    {
        selectIndex = index;

        Image slotImage = uiObj[index].transform.GetComponent<Image>();

        if (slotImage == null) return;

        if (inventory == null) return;

        slotImage.rectTransform.sizeDelta = new Vector2(120, 120);

    }
}
