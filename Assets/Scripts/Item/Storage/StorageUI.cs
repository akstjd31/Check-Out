using System;
using UnityEngine;
using UnityEngine.UI;

public class StorageUI : MonoBehaviour
{
    [SerializeField] GameObject uiPrefab;
    [SerializeField] GameObject[] uiObjs;

    private int selectIndex;
    private Storage storage;

    void Start()
    {
        storage = FindAnyObjectByType<Storage>();

        if (storage == null)
        {
            Debug.Log("UI - 창고를 찾지 못했습니다");
        }
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
            uiObjs[i].name = $"SSlot_{i + 1}";
        }

        int index = 0;

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
        Image ItemImage = uiObjs[index].transform.GetChild(0).GetComponent<Image>();
        Button button = uiObjs[index].transform.GetChild(0).GetComponent<Button>();
        Image slotImage = uiObjs[index].transform.GetComponent<Image>();

        if (slotImage == null) return;

        if (ItemImage == null) return;

        if (storage == null) return;

        if (storage.storageList[index] == null)
        {
            button.onClick.RemoveAllListeners();
            ItemImage.sprite = null;
            return;
        }

        button.onClick.AddListener(delegate { StorageManager.Instance.StorageToInventory(index); });

        Sprite sprite = Resources.Load<Sprite>(storage.storageList[index].imgPath);
        ItemImage.sprite = sprite;
    }
}
