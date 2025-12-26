using System;
using System.Collections;
using UnityEditor;
using UnityEngine;
using static UnityEditor.Progress;

public class InventoryManager : Singleton<InventoryManager>
{
    // [SerializeField] private Transform playerHandTransform;

    [SerializeField] private Inventory inventory;
    [SerializeField] private InventoryUI invenUI;

    private InventoryController inventoryController;
    private ItemInstance currentItem;
    private ItemInstance pervItem;
    private int currentIndex = -1;

    ItemObj preObj;
    ItemObj itemObj;
    //private Vector3 playerPos;

    private string fileName = "InventorySaveData.json";

    private Transform playerHandTransform;
    private Transform player;

    protected override void Awake()
    {
        base.Awake();

        inventory = this.GetComponent<Inventory>();

        ResetInventory();
    }

    public void ResetInventory()
    {
        int size = inventory.GetDefaultInventorySize();
        inventory.SetInventory(size);
    }

    public void InventoryDebug()
    {
        Debug.Log($"인벤토리 크기: {inventory.slots.Length}");
    }

    // 인벤토리 저장 기능
    public bool SaveInventory()
    {
        SlotSaveData saveData = new SlotSaveData();

        for (int i = 0; i < inventory.slots.Length; i++)
        {
            if (inventory.slots[i] == null) continue;

            saveData.slots.Add(new SlotData
            {
                index = i,
                itemId = inventory.slots[i].itemdata.id,
                duration = inventory.slots[i].duration
            });
        }

        SaveLoadManager.Instance.Save(fileName, saveData);
        Debug.Log("인벤토리 데이터 저장 완료!");
        return true;
    }

    // 인벤토리 불러오기
    public bool LoadInventory()
    {
        SlotSaveData saveData =
            SaveLoadManager.Instance.Load<SlotSaveData>(fileName);

        if (saveData == null) return false;

        foreach (var slot in saveData.slots)
        {
            var instance = ItemManager.Instance.Createinstance(slot.itemId);

            instance.duration = slot.duration;

            inventory.GetItem(instance, slot.index);
            Debug.Log($"현재 불러온 아이템: {instance.itemdata.itemName}");
        }

        Debug.Log("인벤토리 데이터 로드 완료!");
        return true;
    }

    // 아이템 줍기
    public bool PickUpItem(ItemInstance item)
    {
        if (inventory == null)
        {
            Debug.Log("인벤토리가 업성");
            return false;
        }

        int inventoryIndex = -1;
        bool empty = inventory.CheckEmpty(out inventoryIndex);

        if (empty == false)
        {
            Debug.Log("인벤토리가 꽉 찼습니다");
            return false;
        }
        
        Debug.Log($"{inventoryIndex} 번째 칸의 인벤토리에 넣는 중");
        inventory.GetItem(item, inventoryIndex);
        SelectInventory(currentIndex);
        Debug.Log($"{item.itemdata.itemName}을 성공적으로 넣었습니다");

        return true;
    }

    // 테스트용
    public void TestPickUp(ItemInstance item)
    {
        PickUpItem(item);
    }

    // 아이템 버리기
    public void DropItem(int index)
    {
        if (inventory == null) return;

        if (GameManager.Instance.Player == null) return;

        ItemInstance item = inventory.MoveItem(index);

        if (item == null) return;

        Transform playerTrf = GameManager.Instance.Player.transform;
        Vector3 newPos = new Vector3
        (
            playerTrf.position.x,
            playerTrf.up.y,
            playerTrf.position.z
        );

        ItemManager.Instance.SpawnItem(item, newPos);

        currentItem = null;
    }

    public ItemInstance MoveItem(int index)
    {
        if (currentIndex == index)
        {
            currentItem.ChangeState(ItemState.Off);
            SelectInventory(index);
        }

        return inventory.MoveItem(index);
    }
    
    // 인벤토리 슬롯 선택
    public void SelectInventory(int index)
    {
        if (inventory == null)
            return;

        player = GameManager.Instance.Player.transform;

        playerHandTransform = player.transform.GetChild(0).GetChild(1);

        if (index < 0 || index >= inventory.slots.Length)
        {
            currentIndex = index;

            if (currentItem != null)
            {
                currentItem.ChangeState(ItemState.Off);
            }

            else if (pervItem != null)
            {
                pervItem.ChangeState(ItemState.Off);
            }

            if (itemObj != null)
            {
                if (currentItem != null)
                {
                    currentItem.duration = itemObj.Returnduration();
                }

                else if (pervItem != null)
                {
                    pervItem.duration = itemObj.Returnduration();
                }

                ItemManager.Instance.ReturnObjItem(itemObj);
            }

            playerHandTransform.gameObject.SetActive(false);
            currentItem = null;
            return;
        }

        currentItem = inventory.slots[index];
        
        currentIndex = index;

        HandItem(currentItem);

        pervItem = currentItem;

        // 부가기능

        return;
    }

    public void UseItem(string key)
    {
        if (currentItem == null)
            return;

        if(currentItem.Use(key))
        {
            var currentItemType = (ItemType)Enum.Parse(typeof(ItemType), currentItem.itemdata.itemType);

            if (currentItemType == ItemType.Gadget && currentItem.state == ItemState.On)
            {
                itemObj.ChangeState(ObjState.On);
            }

            else if (currentItemType == ItemType.Gadget && currentItem.state == ItemState.Off)
            {
                itemObj.ChangeState(ObjState.Off);
            }

            else if (currentItemType == ItemType.Consumable && currentItem.state == ItemState.On)
            {
                Debug.Log("소모품 아이템이 사용됨");
                var item = inventory.MoveItem(currentIndex);
                
                if (item == null) return;

                ConsumableItemUse(item);
            }
        }
    }

    public void ConsumableItemUse(ItemInstance item)
    {
        

        ItemObj groundItem;

        var player = GameManager.Instance.Player.transform;
        foreach (var itemeffect in item.effects)
        {
            switch(item.itemdata.id)
            {
                case 1103:
                    groundItem = ItemManager.Instance.SpawnItemObj(item.itemdata.id, player.position);
                    groundItem.SetItemInfo(item);
                    groundItem.ChangeState(ObjState.On);
                    break;
                default:
                    groundItem = ItemManager.Instance.SpawnItemObj(item.itemdata.id, player.position);
                    break;
            }
        }

        
        // item의 id로 아이템이 무엇인지 탐지
        // groundItem 컴포넌트들 빛이라던지 소리 애니메이션
        // item안에 있는 용량? 지속시간 만큼 부여 파티클시스템이라던지
        // item의 상태가 off가 되었을때 종료
    }

    // 손에 들고있는 아이템
    public void HandItem(ItemInstance item)
    {
        

        if (player == null)
            return;

        if (playerHandTransform == null)
            return;
        
        if (item == null)
        {
            if (itemObj != null && pervItem != null)
            {
                pervItem.duration = itemObj.Returnduration();
                pervItem.ChangeState(ItemState.Off);
                ItemManager.Instance.ReturnObjItem(itemObj);
            }

            playerHandTransform.gameObject.SetActive(false);
            return;
        }  

        if (pervItem != null && item != pervItem)
        {
            pervItem.duration = itemObj.Returnduration();
            pervItem.ChangeState(ItemState.Off);
        }

        if (itemObj != null)
        {
            ItemManager.Instance.ReturnObjItem(itemObj);
        }

        playerHandTransform.gameObject.SetActive(true);

        Debug.Log($"{item.itemdata.id} 입니다.");

        itemObj = ItemManager.Instance.SpawnItemObj(item.itemdata.id, playerHandTransform.position);
        itemObj.transform.parent = playerHandTransform;
        itemObj.transform.rotation = playerHandTransform.rotation;
        itemObj.SetItemInfo(item);
        
    }

    public void UpdateInventory(int index)
    {
    
    }


    public void RemoveInventoryItem()
    {
        if (inventory == null) return;
        if (GameManager.Instance.Player == null) return;

        ItemInstance item = inventory.MoveItem(currentIndex);
    }

    public Inventory GetInvetory() { return inventory; }
}
