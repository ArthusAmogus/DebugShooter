using System;
using System.Collections.Generic;
using UnityEngine;

public abstract class ItemClass : ScriptableObject
{
    

    [Header("Item Properties")]
    public string Name;
    public int itemID;
    public Sprite Icon;
    public ItemType itemType;

    public enum ItemType
    {
        Consumable,
        Misc,
        Bullet
    }

    public string Description;
    //public bool isCraftable;
    public List<MaterialsNeededData> MaterialsNeeded = new List<MaterialsNeededData>();
    public int CraftQuantity;

    public abstract ItemClass GetItem();
    public abstract ItemConsumableClass GetConsumable();
    public abstract ItemMiscClass GetMisc();
    public abstract ItemBulletClass GetBullet();
}
