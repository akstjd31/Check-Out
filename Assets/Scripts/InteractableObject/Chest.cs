using Unity.VisualScripting;
using UnityEngine;

public class Chest : Interactable
{
    [SerializeField] private GameObject storageUIObj;
    private StorageUI storageUI;

    private void Awake()
    {
        if (storageUIObj != null)
            storageUI = storageUIObj.GetComponent<StorageUI>();
    }

    public override void OnFocusEnter()
    {
        currentText = promptText[0];
    }

    public override void OnFocusExit()
    {
        currentText = "";
    }

    public override void Interact()
    {
        // 창고 UI 열기
        OpenChest();
    }

    public void OpenChest()
    {
        SoundManager.Instance.PlayStorageOpenSound();
        storageUIObj.transform.GetChild(0).gameObject.SetActive(true);
        storageUI.StorageOpen();

        Cursor.lockState = CursorLockMode.Confined;
        StorageManager.Instance.IsOpen = true;
        Cursor.visible = true;
    }

    public void CloseChest()
    {
        SoundManager.Instance.PlayStorageCloseSound();
        storageUIObj.transform.GetChild(0).gameObject.SetActive(false);
        storageUI.StorageClose();

        Cursor.lockState = CursorLockMode.Locked;
        StorageManager.Instance.IsOpen = false;
        Cursor.visible = false;
    }
}
