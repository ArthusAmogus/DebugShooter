using TMPro;
using UnityEngine;
using System.Collections.Generic;
using UnityEngine.UI;

public class CraftingMenu : MonoBehaviour
{
    [SerializeField] GameObject      ItemInfo_Panel;
    [SerializeField] TextMeshProUGUI ItemInfo_ItemName;
    [SerializeField] TextMeshProUGUI ItemInfo_ItemDescription;
    [SerializeField] TextMeshProUGUI Crafting_ItemName;
    [SerializeField] TextMeshProUGUI Crafting_ItemDescription;
    [SerializeField] InventoryManager InventoryManager;
    public Button CraftButton;
    public ItemClass SelectedItem;
    [SerializeField] GameObject CraftHalo;

    [SerializeField] MaterialNeededSlot[] MaterialSlots;
    [SerializeField] Transform MaterialSlotPanel;

    private void Start()
    {
        MaterialSlots = MaterialSlotPanel.GetComponentsInChildren<MaterialNeededSlot>();
    }

    public void DisplayItemInfo(ItemClass item)
    {
        if (item != null)
        {
            ItemInfo_Panel.SetActive(true);
            ItemInfo_ItemName.text = item.Name;
            ItemInfo_ItemDescription.text = item.Description;
        }
        else
        {
            ItemInfo_Panel.SetActive(false);
        }
    }

    public void DisplayCraftingInfo(ItemClass item)
    {
        if (item != null)
        {
            SelectedItem = item;
            CraftButton.interactable = true;
            Crafting_ItemName.text = item.Name;
            Crafting_ItemDescription.text = item.Description;

            for (int i = 0; i < MaterialSlots.Length; i++)
            {
                if (i < item.MaterialsNeeded.Count)
                {
                    MaterialSlots[i].SetItem(item.MaterialsNeeded[i]);
                }
                else
                {
                    MaterialSlots[i].SetItem(null);
                }
            }
        }
        else
        {
            Crafting_ItemName.text = "";
            Crafting_ItemDescription.text = "";
            for (int i = 0; i < MaterialSlots.Length; i++)
            {
                MaterialSlots[i].SetItem(null);
            }
        }
    }

    public void CraftItem()
    {
        if (SelectedItem != null)
        {
            // Check if player has enough materials
            bool canCraft = true;
            foreach (var material in SelectedItem.MaterialsNeeded)
            {
                int playerQuantity = InventoryManager.GatherItem(material.item);
                if (playerQuantity < material.amount)
                {
                    canCraft = false;
                    break;
                }
            }
            if (canCraft)
            {
                // Deduct materials from inventory
                foreach (var material in SelectedItem.MaterialsNeeded)
                {
                    for (int i = 0; i < material.amount; i++)
                    {
                        InventoryManager.RemoveItem(material.item, material.amount);
                    }
                }
                CraftHalo.SetActive(true);
                // Add crafted item to inventory
                InventoryManager.AddItem(SelectedItem, SelectedItem.CraftQuantity);
                // Refresh UI
                InventoryManager.DisplayInventory();
            }
            else
            {
                ItemInfo_Panel.SetActive(true);
                ItemInfo_ItemName.text = "";
                ItemInfo_ItemDescription.text = "You do not have enough materials to craft this item.";
            }
        }
    }

    public void ClearCraftingInfo()
    {
        DisplayCraftingInfo(null);
        DisplayItemInfo(null);
    }
}

