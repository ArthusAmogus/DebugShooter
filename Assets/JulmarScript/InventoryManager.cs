using Firebase.Database;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using UnityEngine;
using UnityEngine.Networking;

[System.Serializable]

public class InventoryData
{
    public int userID;
    public List<InventorySlotData> slot = new();
}

[System.Serializable]

public class InventorySlotData
{
    public int itemID;
    public string itemName;
    public int quantity;
}

public class InventoryManager : MonoBehaviour
{
    [Header("Inventory Properties")]
    [SerializeField] private Transform SlotHolder;
    public InventorySlotUI[] UISlots;
    public List<InventorySlot> InvSlots = new();
    [SerializeField] CraftingMenu craftingMenu;
    DatabaseReference reference;
    public int userID;


    public static InventoryManager Instance { get; internal set; }

    private void OnEnable()
    {
        Instance = this;
        reference = FirebaseDatabase.DefaultInstance.RootReference;
    }


    private void Start()
    {
        UISlots = SlotHolder.GetComponentsInChildren<InventorySlotUI>();
        foreach (var slotUI in UISlots)
        {
            slotUI.craftingMenu = craftingMenu;
        }
        DisplayInventory();
    }

    public void DisplayInventory()
    {
        for (int i = 0; i < UISlots.Length; i++)
        {
            if (i < InvSlots.Count)
            {
                UISlots[i].SetItem(InvSlots[i]);
            }
            else
            {
                UISlots[i].ClearSlot();
            }
        }
    }

    public void AddItem(ItemClass item, int amount)
    {
        for (int i = 0; i < amount; i++)
        {
            var amountLimit = item.itemType switch
            {
                ItemClass.ItemType.Consumable => 1,
                ItemClass.ItemType.Bullet => 16,
                ItemClass.ItemType.Misc => 64,
                _ => 16,
            };
            InventorySlot existingSlot = InvSlots.FirstOrDefault(slot => slot.Item == item && slot.quantity < amountLimit);
            if (existingSlot != null)
            {
                existingSlot.quantity++;
            }
            else
            {
                InvSlots.Add(new InventorySlot(item));
            }
        }
        DisplayInventory();
    }

    public void RemoveItem(ItemClass item, int amount)
    {
        for (int i = 0; i < amount; i++)
        {
            InventorySlot slotToRemove = InvSlots.LastOrDefault(Slot => Slot.Item == item);

            if (slotToRemove != null)
            {
                slotToRemove.quantity--;
                if (slotToRemove.quantity <= 0)
                {
                    InvSlots.Remove(slotToRemove);
                }
            }

            DisplayInventory();
        }
    }

    public int GatherItem(ItemClass item)
    { 
        int total = 0;
        foreach (var slot in InvSlots)
        {
            if (slot.Item == item)
            {
                total += slot.quantity;
            }
        }
        return total;
    }

    public void SaveInventory()
    {
        InventoryData data = new()
        {
            userID = 101
        };
        foreach (var slot in InvSlots)
        {
            data.slot.Add(new InventorySlotData
            {
                itemID = slot.Item.itemID,
                itemName = slot.Item.Name,
                quantity = slot.quantity
            });
        }

        UserDataObject.Instance.userData.inventorySlotData = data.slot;
    }

    public void LoadInventory()
    {
        InvSlots.Clear();
        foreach (var slotData in UserDataObject.Instance.userData.inventorySlotData)
        {
            ItemClass foundItem = Resources.LoadAll<ItemClass>("")
            .FirstOrDefault(i => i.Name == slotData.itemName);

            if (foundItem != null)
            {
                InvSlots.Add(new InventorySlot(foundItem, slotData.quantity));
            }
            else
            {
                Debug.Log("Not found: " + slotData.itemName);
            }
            DisplayInventory();
        }
    }


}
