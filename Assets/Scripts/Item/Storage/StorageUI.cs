using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class StorageUI : MonoBehaviour
{
    [SerializeField] GameObject uiPrefab;
    [SerializeField] GameObject[] uiObjs;
    [SerializeField] StorageHoverUI hover;

    private Storage storage;
    private InventoryUI inventoryUI;
    private int storageSize;


    private void Awake()
    {
        storage = FindAnyObjectByType<Storage>();
        inventoryUI = FindAnyObjectByType<InventoryUI>();
        if (storage == null)
        {
            Debug.Log("UI - 창고를 찾지 못했습니다");
        }
    }

    private void Start()
    {
        if (storage != null)
        {
            storageSize = storage.GetDefaultStorageSize();
            storage.SetStorage(storageSize);
            SetStorageUI(storageSize);
        }
    }

    private void OnEnable()
    {
        inventoryUI.StorageChangeState();

        StorageManager.Instance.OnStorageChanged += UpdateUI;
    }

    private void OnDisable()
    {
        inventoryUI.StorageChangeState();

        StorageManager.Instance.OnStorageChanged -= UpdateUI;
    }

    // 로딩 때 창고 ui 세팅
    public void SetStorageUI(int size)
    {
        // 사이즈 기본 1 보장
        size = size <= 0 ? 1 : size;

        if (uiObjs.Length > 0) return;

        uiObjs = new GameObject[size];

        for (int i = 0; i < size; i++)
        {
            uiObjs[i] = Instantiate(uiPrefab, transform);
            uiObjs[i].name = $"Storage_Slot_{i + 1}";
        }

        UpdateAll();
    }

    private void UpdateAll()
    {
        int index = 0;
        foreach (var uiObj in uiObjs)
        {
            UpdateUI(index++);
        }
    }

    // UI가 변경 되었을때
    private void UpdateUI(int index)
    {
        hover.gameObject.SetActive(false);

        Image ItemImage = uiObjs[index].transform.GetChild(0).GetComponent<Image>();
        Button button = uiObjs[index].transform.GetChild(0).GetComponent<Button>();
        Image slotImage = uiObjs[index].GetComponent<Image>();
        EventTrigger trigger = uiObjs[index].GetComponent<EventTrigger>();

        if (slotImage == null) return;

        if (ItemImage == null) return;

        if (storage == null) return;

        if (storage.storageList[index] == null)
        {
            button.onClick.RemoveAllListeners();
            if (trigger != null)
                trigger.triggers.Clear();
            ItemImage.sprite = null;

            return;
        }
        
        button.onClick.AddListener(delegate { StorageManager.Instance.StorageToInventory(index); });

        Sprite sprite = Resources.Load<Sprite>(storage.storageList[index].imgPath);

        ItemImage.sprite = sprite;

        if (trigger != null)
        {
            trigger.triggers.Clear();

            // 마우스 올라갔을때 이벤트
            EventTrigger.Entry Enterentry = new EventTrigger.Entry();
            Enterentry.eventID = EventTriggerType.PointerEnter;
            Enterentry.callback.AddListener((data) => { hover.OnEnter(uiObjs[index].transform, storage.storageList[index], sprite); });

            // 마우스 빠져나갔을때 이벤트
            EventTrigger.Entry Exitentry = new EventTrigger.Entry();
            Exitentry.eventID = EventTriggerType.PointerExit;
            Exitentry.callback.AddListener((data) => { hover.OnExit(); });

            Debug.Log("이벤트 추가됨");

            trigger.triggers.Add(Enterentry);
            trigger.triggers.Add(Exitentry);
        }
    }
}
