using Unity.VisualScripting;
using UnityEngine;

public class Chest : Interactable
{
    [SerializeField] private GameObject storageUIObj;
    [SerializeField] private StorageUI storageUI;

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
        storageUIObj.SetActive(true);
        
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
    }
}
