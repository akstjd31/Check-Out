using Unity.VisualScripting;
using UnityEngine;

public class Chest : Interactable
{
    [SerializeField] private GameObject storageUIObj;
    private StorageUI storageUI;

    private void Awake()
    {
        if (storageUIObj != null)
            storageUI = storageUIObj.GetComponentInChildren<StorageUI>();
    }

    public override void OnFocusEnter()
    {
        promptText = "Press [E] to Open Storage";
    }

    public override void OnFocusExit()
    {
        promptText = "";
    }

    public override void Interact()
    {
        // 창고 UI 열기
        OpenChest();
    }

    public void OpenChest()
    {
        storageUIObj.SetActive(true);
        Cursor.lockState = CursorLockMode.Confined;
        StorageManager.Instance.IsOpen = true;
        Cursor.visible = true;
    }

    public void CloseChest()
    {
        storageUIObj.SetActive(false);
        Cursor.lockState = CursorLockMode.Locked;
        StorageManager.Instance.IsOpen = false;
        Cursor.visible = false;
    }
}
