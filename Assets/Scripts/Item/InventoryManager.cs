using System;
using System.Threading;
using UnityEditor.Search;
using UnityEngine;
using UnityEngine.Experimental.GlobalIllumination;

public class InventoryManager : Singletone<InventoryManager>
{
    public Item[] items;

    public int InventorySize = 1;

    [SerializeField] private GameObject player;
    [SerializeField ]private Transform playerHandTransform;

    private Item currentItem;

    private void Awake()
    {
        items = new Item[InventorySize];
    }

    // 아이템을 주었을 시
    public void GetItem(Item item)
    {
        if (items == null)
        {
            Debug.Log("인벤토리 자체가 없습니다");
            return;
        }
           

        if (item == null)
        {
            Debug.Log("아이템이 없습니다");
            return;
        }
            

        // 인벤토리에 넣을려고 시도
        for (int i = 0; i < items.Length; i++)
        {
            if (items[i] != null)
                continue;

            // 인벤토리가 비어있을 시
            else if (items[i] == null)
            {
                items[i] = item;
                item.transform.parent = transform;
                item.gameObject.SetActive(false);
                break;
            }

            Debug.Log("인벤토리창이 꽉 찾습니다");
        }
    }
    
    // 아이템을 떨구었을 시
    public void DropItem(int index)
    {
        if (items == null)
            return;

        if (items[index] == null)
            return;

        items[index].transform.position = player.transform.position;

        items[index] = null;
    }

    // 손에 표시할 아이템
    public void HandItem(Item item)
    {                                     
        if (item == null)
            return;

        item.gameObject.SetActive(true);
        item.transform.position = playerHandTransform.position;
    }
    
    // 휠을 올리거나 내려서 인벤토리 선택 했을 시
    public void SelectInventory(int index)
    {
        if (index < 0 || index > InventorySize)
            return;

        Item currentItem = items[index];

        Debug.Log(currentItem);

        if (currentItem == null)
        {
            Debug.Log("인벤토리에 아이템이 없습니다");
            return;
        }
          
        // 인벤토리에 아이템이 존재할시 손에 표시
        HandItem(currentItem);

        return;
    }
    public void UpdateInventory(int index)
    {

    }


}
