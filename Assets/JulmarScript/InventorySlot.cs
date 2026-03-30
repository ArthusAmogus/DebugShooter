using UnityEngine;

[System.Serializable]
public class InventorySlot
{
    public ItemClass Item;
    public int quantity;


    public InventorySlot(ItemClass item, int quantity = 1)
    {
        this.Item = item;
        this.quantity = quantity;
    }
}
