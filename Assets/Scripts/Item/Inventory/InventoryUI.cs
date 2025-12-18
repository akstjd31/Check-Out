using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class InventoryUI : MonoBehaviour
{
    [SerializeField] GameObject uiPrefab;
    [SerializeField] GameObject[] uiObjs;
    [SerializeField] HoverUI hover;

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
    public void SetInventoryUI(int size)
    {

        // 사이즈 기본 1 보장
        size = size <= 0 ? 1 : size;

        if (uiObjs.Length > 0) return;

        uiObjs = new GameObject[size];

        for (int i = 0; i < size; i++)
        {
            uiObjs[i] = Instantiate(uiPrefab, transform);
            uiObjs[i].name = $"Inventory_Slot_{i + 1}";
        }

        UpdateAll();

        SelectUI(0);
    }
    public void UpdateAll()
    {
        int index = 0;
        foreach (var uiObj in uiObjs)
        {
            UpdateUI(index++);
        }
    }

    // UI가 변경 되었을때
    public void UpdateUI(int index)
    {
        Image ItemImage = uiObjs[index].transform.GetChild(0).GetComponent<Image>();
        Button button = uiObjs[index].transform.GetChild(0).GetComponent<Button>();
        Image slotImage = uiObjs[index].GetComponent<Image>();
        EventTrigger trigger = uiObjs[index].GetComponent<EventTrigger>();

        Debug.Log(trigger);

        if (slotImage == null) return;

        if (ItemImage == null) return;

        if (inventory == null) return;

        if (selectIndex != index)
        {
            slotImage.rectTransform.sizeDelta = new Vector2(125, 125);
        }

        if (inventory.slots[index] == null)
        {
            button.onClick.RemoveAllListeners();
            if (trigger != null)
                trigger.triggers.Clear();
            ItemImage.sprite = null;
            return;
        }

        // 버튼에 이벤트 추가
        button.onClick.RemoveAllListeners();
        button.onClick.AddListener(delegate { StorageManager.Instance.InventoryToStorage(index); });

        Sprite sprite = Resources.Load<Sprite>(inventory.slots[index].imgPath);
        ItemImage.sprite = sprite;

        if (trigger != null)
        {
            trigger.triggers.Clear();

            // 마우스 올라갔을때 이벤트
            EventTrigger.Entry Enterentry = new EventTrigger.Entry();
            Enterentry.eventID = EventTriggerType.PointerEnter;
            Enterentry.callback.AddListener((data) => { hover.OnEnter(uiObjs[index].transform, inventory.slots[index], sprite); });

            // 마우스 빠져나갔을때 이벤트
            EventTrigger.Entry Exitentry = new EventTrigger.Entry();
            Exitentry.eventID = EventTriggerType.PointerExit;
            Exitentry.callback.AddListener((data) => { hover.OnExit(); });

            trigger.triggers.Add(Enterentry);
            trigger.triggers.Add(Exitentry);
        }
    }

    // UI가 선택되었을때 UI업데이트
    public void SelectUI(int index)
    {
        selectIndex = index;

        Image slotImage = uiObjs[index].transform.GetComponent<Image>();

        if (slotImage == null) return;

        if (inventory == null) return;

        slotImage.rectTransform.sizeDelta = new Vector2(150, 150);

    }
}
