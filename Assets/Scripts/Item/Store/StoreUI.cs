using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class StoreUI : MonoBehaviour
{
    [SerializeField] GameObject uiPrefab;
    [SerializeField] GameObject contentObj;
    [SerializeField] GameObject[] uiObjs;
    [SerializeField] ShopHoverUI hover;
    [SerializeField] Button xButton;

    private Store store;
    private InventoryUI inventoryUI;

    private void Awake()
    {
        store = FindAnyObjectByType<Store>();
        hover = FindAnyObjectByType<ShopHoverUI>();
        inventoryUI = FindAnyObjectByType<InventoryUI>();

        if (store == null)
        {
            //debug.Log("UI - 창고를 찾지 못했습니다");
        }
    }

    private void Start()
    {
        if (StoreManager.Instance != null)
        {
            SetStoreUI(StoreManager.Instance.GetItemListSize());
        }

        xButton.onClick.AddListener(SoundManager.Instance.PlayUIButtonClickSound);
        xButton.onClick.AddListener(GameManager.Instance.Player.GetComponent<PlayerInputHandler>().ReleaseIgnoreInput);
    }

    public void OnEnable()
    {
        inventoryUI.StoreChangeOpen();
        inventoryUI.OnButtonRefresh();
    }

    public void OnDisable()
    {
        inventoryUI.StoreChangeClosed();
        inventoryUI.OnButtonRefresh();
    }

    // 로딩 때 창고 ui 세팅
    public void SetStoreUI(int size)
    {
        // 사이즈 기본 1 보장
        size = size <= 0 ? 1 : size;

        if (uiObjs.Length > 0) return;

        uiObjs = new GameObject[size];

        for (int i = 0; i < size; i++)
        {
            uiObjs[i] = Instantiate(uiPrefab, contentObj.transform);
            uiObjs[i].name = $"Store_Slot_{i + 1}";
        }

        UpdateAll();
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
        var uiObj = uiObjs[index];

        Image ItemImage = uiObj.transform.GetChild(1).GetComponent<Image>();
        Button button = uiObj.GetComponent<Button>();
        Image typeImage = uiObj.transform.GetChild(0).GetComponent<Image>();
        Image slotImage = uiObj.GetComponent<Image>();
        EventTrigger trigger = uiObj.GetComponent<EventTrigger>();

        var storeItem = store.shopList[index];

        if (slotImage == null)
            return;

        if (ItemImage == null) return;

        if (store == null) return;

        if (storeItem == null)
        {
            button.onClick.RemoveAllListeners();

            if (trigger != null)
                trigger.triggers.Clear();
            ItemImage.sprite = null;

            return;
        }

        var itemdata = ItemManager.Instance.GetItemData(storeItem.itemId);

        if (itemdata.itemType == "Gadget")
        {
            typeImage.gameObject.SetActive(true);
            Color color = Color.pink;
            color.a = 0.5f;
            typeImage.color = color;
        }

        else if (itemdata.itemType == "Consumable")
        {
            typeImage.gameObject.SetActive(true);
            Color color = Color.blue;
            color.a = 0.5f;
            typeImage.color = color;
        }

        else
        {
            typeImage.gameObject.SetActive(false);
        }

        button.onClick.RemoveAllListeners();
        button.onClick.AddListener(delegate { StoreManager.Instance.BuyItem(storeItem);
                                            SoundManager.Instance.PlayUIButtonClickSound(); });

        var item = ItemManager.Instance.GetItemData(storeItem.itemId);

        Sprite sprite = Resources.Load<Sprite>(item.imgPath);

        ItemImage.sprite = sprite;

        if (trigger != null)
        {
            trigger.triggers.Clear();

            // 마우스 올라갔을때 이벤트
            EventTrigger.Entry Enterentry = new EventTrigger.Entry();
            Enterentry.eventID = EventTriggerType.PointerEnter;
            Enterentry.callback.AddListener((data) => { hover.OnEnter(uiObj.transform, storeItem, sprite); });

            // 마우스 빠져나갔을때 이벤트
            EventTrigger.Entry Exitentry = new EventTrigger.Entry();
            Exitentry.eventID = EventTriggerType.PointerExit;
            Exitentry.callback.AddListener((data) => { hover.OnExit(); });

            //debug.Log("이벤트 추가됨");

            trigger.triggers.Add(Enterentry);
            trigger.triggers.Add(Exitentry);
        }
    }
}
