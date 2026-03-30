using UnityEngine;

public class ReloadingSystem : MonoBehaviour
{
    [SerializeField] FPShooterSystem shooterSystem;
    [SerializeField] InventoryManager inventoryManager;
    public ItemClass itemRequired;
    [SerializeField] bool canReload = true;
    [SerializeField] GameObject ReloadHalo;

    [Header("Header")]
    [SerializeField] bool unliAmmo = false;

    public static ReloadingSystem Instance { get; internal set; }

    private void Awake()
    {
        Instance = this;
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.R))
        {
            if (canReload && shooterSystem.CurCap < shooterSystem.MaxCap && inventoryManager.GatherItem(itemRequired) > 0 || unliAmmo)
            {
                canReload = false;
                ReloadHalo.SetActive(true);
            }
        }
    }

    public void Reload()
    {
        for (int i = shooterSystem.MaxCap; i > 0; i--)
        {
            if (!unliAmmo)
            {
                if (shooterSystem.CurCap >= shooterSystem.MaxCap)
                    break;
                if (inventoryManager.GatherItem(itemRequired) > 0)
                {
                    inventoryManager.RemoveItem(itemRequired, 1);
                    shooterSystem.CurCap++;
                }
            }
            else
            {
                if (shooterSystem.CurCap >= shooterSystem.MaxCap)
                    break;
                shooterSystem.CurCap++;
            }

        }
        canReload = true;
    }

}
