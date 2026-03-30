using UnityEngine;

public class InventoryController : MonoBehaviour
{
    [SerializeField] GameObject InventoryPanel;
    [SerializeField] GameObject ProgressPanel;
    [SerializeField] RotationWithMouse Rotation;
    [SerializeField] FPShooterSystem ShooterSystem;
    [SerializeField] CraftingMenu CraftingMenu;
    bool isInventoryOpen = false;

    public static InventoryController Instance { get; internal set; }

    private void OnEnable()
    {
        Instance = this;
    }

    private void Start()
    {
        CraftingMenu.CraftButton.interactable = false;
        CraftingMenu.DisplayItemInfo(null);
        CraftingMenu.DisplayCraftingInfo(null);
        InventoryPanel.SetActive(false);
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.E))
        {
            if (!isInventoryOpen)
                isInventoryOpen = true;
            else
                isInventoryOpen = false;
            ProgressPanel.SetActive(!isInventoryOpen);
            InventoryPanel.SetActive(isInventoryOpen);
            Rotation.enabled = !isInventoryOpen;
            ShooterSystem.enabled = !isInventoryOpen;
            CraftingMenu.DisplayItemInfo(null);
            CraftingMenu.CraftButton.interactable = true;
        }
    }
}
