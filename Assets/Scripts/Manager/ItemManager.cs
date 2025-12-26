using System.Collections.Generic;
using UnityEngine;
using static UnityEditor.Progress;

public class ItemManager : Singleton<ItemManager>
{
    [SerializeField] private Item itemPrefab;
    [SerializeField] private ItemObj[] itemObjects;
    [SerializeField] private ItemObj[] handItemObjects;
    [SerializeField] int playerCount = 1;

    [SerializeField] private Transform itemPoolParent;
    [SerializeField] private Transform itemObjPoolParent;
    [SerializeField] private Transform handitemObjPoolParent;

    private ObjPool<Item> itemPool;
    private ObjPool<ItemObj> itemObjPool;
    private ObjPool<ItemObj> handItemObjPool;
    private Dictionary<int, ItemTableData> itemDataID;
    private Dictionary<int, EffectGroupTableData> effectDataID;
    //private Dictionary<ItemTableData, List<IItemEffect>> items;

    ItemObj itemobject;
    protected override void Awake()
    {
        base.Awake();
        itemPool = new ObjPool<Item>();
        itemObjPool = new ObjPool<ItemObj>();
        handItemObjPool = new ObjPool<ItemObj>();

        itemDataID = new Dictionary<int, ItemTableData>();
        effectDataID = new Dictionary<int, EffectGroupTableData>();
        //items = new Dictionary<ItemTableData, List<IItemEffect>>();
      
        Init();

        if (itemPrefab == null) return;

        itemPool.CreatePool(itemPrefab, 10, itemPoolParent);

        if (itemObjects == null) return;

        if (itemObjects.Length <= 0) return;

        foreach (var itemObj in itemObjects)
        {
            Debug.Log(itemObj.name);
            itemObjPool.CreatePool(itemObj, playerCount, itemObjPoolParent);
        }

        if (handItemObjects == null) return;

        if (handItemObjects.Length <= 0) return;

        foreach (var itemObj in handItemObjects)
        {
            Debug.Log(itemObj.name);
            handItemObjPool.CreatePool(itemObj, playerCount, handitemObjPoolParent);
        }



        //itemPool.CreatePool(flashLightPrefab, 1, transform);
    }

    public void Init()
    {
        var itemTable = TableManager.Instance.GetTable<int, ItemTableData>();
        var itemEffectTable = TableManager.Instance.GetTable<int, EffectGroupTableData>();

        if (itemTable == null)
        {
            Debug.Log("아이템 테이블이 null 입니다");
            return;
        }

        if (effectDataID == null)
        {
            Debug.Log("아이템 효과 테이블이 null 입니다");
            return;
        }

        // 아이템 테이블 세팅
        foreach (var targetId in TableManager.Instance.GetAllIds(itemTable))
        {
            var ItemTableData = itemTable[targetId];

            if (ItemTableData != null)
            {
                itemDataID[ItemTableData.id] = ItemTableData;
            }
        }

        // 아이템 효과 테이블 세팅
        foreach (var targetId in TableManager.Instance.GetAllIds(itemEffectTable))
        {
            var ItemEffectData = itemEffectTable[targetId];

            if (ItemEffectData != null)
            {
                effectDataID[ItemEffectData.id] = ItemEffectData;
            }
        }
    }

    public void Test(int itemid)
    {
        SpawnItem(itemid + 1000, transform.position);
    }

    // 아이템 새로 생성
    public Item SpawnItem(int itemID, Vector3 pos)
    {
        var instance = Createinstance(itemID);
        //List<ItemEffectData> effectData = GetItemEffectData(data.itemEffect);
        Item item = itemPool.GetObject(itemPrefab);
        item.transform.position = pos;
        item.Init(instance);
        return item;
    }

    // 기존 아이템 정보 유지한 상태로 바닥에 드롭
    public Item SpawnItem(ItemInstance itemInstance, Vector3 pos)
    {
        Item item = itemPool.GetObject(itemPrefab);
        item.transform.position = pos;
        item.Init(itemInstance);
        return item;
    }

    public ItemObj SpawnItemObj(int prefabId, Vector3 pos)
    {
        foreach(var itemobj in itemObjects)
        {
            if (itemobj.ItemId == prefabId)
            {
                itemobject = itemObjPool.GetObject(itemobj);
            }
        }

        if (itemobject != null)
        {
            itemobject.transform.position = pos;
        }

        return itemobject;
    }

    public ItemObj SpawnHandItemObj(int prefabId, Vector3 pos)
    {
        foreach (var itemobj in handItemObjects)
        {
            if (itemobj.ItemId == prefabId)
            {
                itemobject = handItemObjPool.GetObject(itemobj);
            }
        }

        if (itemobject != null)
        {
            itemobject.transform.position = pos;
        }

        return itemobject;
    }

    // 손에 가져올 아이템

    public void ReturnItem(Item item)
    {
        Debug.Log(item);
        itemPool.ReturnObject(itemPrefab, item);
    }

    public void ReturnObjItem(ItemObj item)
    {
        foreach (var obj in itemObjects)
        {
            if ( obj.ItemId == item.ItemId )
            {
                Debug.Log("반환함");
                item.transform.parent = itemObjPoolParent;
                itemObjPool.ReturnObject(obj, item);
                return;
            }
        }
    }

    public void ReturnObjHandItem(ItemObj item)
    {
        foreach (var obj in handItemObjects)
        {
            if (obj.ItemId == item.ItemId)
            {
                Debug.Log("반환함");
                item.transform.parent = itemObjPoolParent;
                handItemObjPool.ReturnObject(obj, item);
                return;
            }
        }
    }

    // 스포너에 의해 생성된 아이템들 씬 전환 전에 회수하는 작업 
    public void ReturnAllItem()
    {
        var items = GetComponentsInChildren<Item>(true);

        foreach (var item in items)
        {
            if (item != null)
                itemPool.ReturnObject(itemPrefab, item);
        }
    }

    public ItemInstance Createinstance(int itemId)
    {
        var data = GetItemData(itemId);
        var effects = GetItemEffectData(data.itemEffect);

        return new ItemInstance(data, effects);
    }

    public ItemTableData GetItemData(int itemID)
    {
        if (itemDataID.TryGetValue(itemID, out var item) == false) return null;

        return item;
    }

    public List<ItemEffect> GetItemEffectData(int effectGroup)
    {
        List<ItemEffect> effectList = new List<ItemEffect>();

        if (effectGroup <= 0)
        {
            return effectList;
        }

        foreach (var item in effectDataID)
        {
            if (effectDataID.TryGetValue(item.Key, out var itemEffect) == false) return null;

            if (itemEffect.effectGroup == effectGroup)
            {
                switch(itemEffect.effectName)
                {
                    case "SanityIncrease":
                        effectList.Add(new SanityIncrease(itemEffect.effectName,itemEffect.value1, itemEffect.value2, itemEffect.ControlKey));
                        break;
                    case "StaminaIncrease":
                        effectList.Add(new StaminaIncrease(itemEffect.effectName,itemEffect.value1, itemEffect.value2, itemEffect.ControlKey));
                        break;
                    case "ConsumableDuration":
                        effectList.Add(new ConsumableDuration(itemEffect.effectName,itemEffect.value1, itemEffect.value2, itemEffect.ControlKey));
                        break;
                    case "GadgetDuration":
                        effectList.Add(new GadgetDuration(itemEffect.effectName,itemEffect.value1, itemEffect.value2, itemEffect.ControlKey));
                        break;
                    case "GadgetReload":
                        effectList.Add(new GadgetReload(itemEffect.effectName,itemEffect.value1, itemEffect.value2, itemEffect.ControlKey));
                        break;
                    case "Light":
                        effectList.Add(new ItemLight(itemEffect.effectName,itemEffect.value1, itemEffect.value2, itemEffect.ControlKey));
                        break;
                }
            }
        }

        return effectList;
    }
}
