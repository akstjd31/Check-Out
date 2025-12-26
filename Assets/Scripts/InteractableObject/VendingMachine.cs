using UnityEngine;

public class VendingMachine : Interactable
{
    [SerializeField] private GameObject shopUIObj;
    private StoreUI storeUI;

    private void Awake()
    {
        if (shopUIObj != null)
            storeUI = shopUIObj.GetComponentInChildren<StoreUI>();
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
        // 상점 UI 열기
        OpenShop();
    }

    public void OpenShop()
    {
        if (!StoreManager.Instance.IsInitialized)
            StoreManager.Instance.Init();
            
        shopUIObj.SetActive(true);
        Cursor.lockState = CursorLockMode.Confined;
        StoreManager.Instance.IsOpen = true;
        Cursor.visible = true;
    }

    public void CloseShop()
    {
        shopUIObj.SetActive(false);
        Cursor.lockState = CursorLockMode.Locked;
        StoreManager.Instance.IsOpen = false;
        Cursor.visible = false;
    }
}
