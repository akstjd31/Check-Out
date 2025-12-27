using System;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class InventoryUI : MonoBehaviour
{
    [SerializeField] GameObject uiPrefab;
    [SerializeField] GameObject[] uiObjs;
    [SerializeField] InventoryHoverUI hover;


    private Image selectBar;
    private int selectIndex = -1;               // 처음 시작에는 -1
    private Inventory inventory;
    private int invenSize;

    [SerializeField] bool IsStorageOpen = false;
    [SerializeField] bool IsStoreOpen = false;

    private void Awake()
    {
        inventory = FindAnyObjectByType<Inventory>();
        hover = FindAnyObjectByType<InventoryHoverUI>();

        if (inventory == null)
        {
            Debug.Log("UI - 인벤토리를 찾지 못했습니다");
        }
    }

    private void Start()
    {
        Init();
    }

    public void Init()
    {
        invenSize = inventory.GetDefaultInventorySize();
        SetInventoryUI(invenSize);
    }

    private void OnEnable()
    {
        inventory.OnSlotUpdated += UpdateUI;
    }

    private void OnDisable()
    {
        inventory.OnSlotUpdated -= UpdateUI;
    }

    public int GetInventoryMaxIndex() => invenSize - 1;

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
        if (!GameManager.Instance.CurrentState.Equals(GameState.Session))
            hover.transform.GetChild(0).gameObject.SetActive(false);

        Image ItemImage = uiObjs[index].transform.GetChild(0).GetComponent<Image>();
        Button button = uiObjs[index].transform.GetChild(0).GetComponent<Button>();
        Image selectBar = uiObjs[index].transform.GetChild(1).GetComponent<Image>();
        Image slotImage = uiObjs[index].GetComponent<Image>();
        EventTrigger trigger = uiObjs[index].GetComponent<EventTrigger>();

        if (slotImage == null || ItemImage == null || inventory == null) return;

        if (selectIndex == -1 || selectIndex != index)
        {
            selectBar.gameObject.SetActive(false);
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

        Sprite sprite = Resources.Load<Sprite>(inventory.slots[index].itemdata.imgPath);
        ItemImage.sprite = sprite;

        button.onClick.RemoveAllListeners();

        if (IsStorageOpen)
            OnStorageUI(button, index);

        else if (IsStoreOpen)
            OnStoreUI(button, index);

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

    public void OnButtonRefresh()
    {
        int invenIndex = 0;
        foreach (var uiObj in uiObjs)
        {
            Button button = uiObj.transform.GetChild(0).GetComponent<Button>();

            if (inventory.slots[invenIndex] != null)
            {
                if (IsStorageOpen)
                    OnStorageUI(button, invenIndex);

                else if (IsStoreOpen)
                    OnStoreUI(button, invenIndex);
            }
            
            invenIndex++;
        }
    }

    // 상점 오픈 시 버튼 할당
    public void OnStoreUI(Button button, int index)
    {
        button.onClick.RemoveAllListeners();
        button.onClick.AddListener(delegate { StoreManager.Instance.SellItem(index);
                                            SoundManager.Instance.PlayUIButtonClickSound(); });
    }

    // 창고 오픈 시 버튼 할당
    public void OnStorageUI(Button button, int index)
    {
        Debug.Log("버튼 할당됨!");
        button.onClick.RemoveAllListeners();
        button.onClick.AddListener(delegate { StorageManager.Instance.InventoryToStorage(index); 
                                            SoundManager.Instance.PlayUIButtonClickSound(); });
    }

    // UI가 선택되었을때 UI업데이트
    public void SelectUI(int index)
    {
        selectIndex = index;

        if (index == -1)
        {
            return;
        }
        
        Image slotImage = uiObjs[index].transform.GetComponent<Image>();
        Image selectBar = uiObjs[index].transform.GetChild(1).GetComponent<Image>();


        if (slotImage == null || inventory == null) return;

        slotImage.rectTransform.sizeDelta = new Vector2(150, 150);
        selectBar.gameObject.SetActive(true);
    }

    // 창고 상태
    public void StorageOpen() => IsStorageOpen = true;
    public void StorageClosed() => IsStorageOpen = false;

    // 상점 상태
    public void StoreChangeOpen() => IsStoreOpen = true;
    public void StoreChangeClosed() => IsStoreOpen = false;

}
