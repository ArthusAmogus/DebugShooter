using UnityEngine;

[CreateAssetMenu(fileName = "New Consumable", menuName = "Items/Consumable")]
public class ItemConsumableClass : ItemClass
{
    [Header("Consumable Properties")]
    public int HealAmount=10;



    public override ItemConsumableClass GetConsumable()
    {
        return this;
    }

    public override ItemClass GetItem()
    {
        return this;
    }

    public override ItemMiscClass GetMisc()
    {
        return null;
    }

    public override ItemBulletClass GetBullet()
    {
        return null;
    }
}
