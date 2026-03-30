using UnityEngine;

[CreateAssetMenu(fileName = "New Misc", menuName = "Items/Misc")]
public class ItemMiscClass : ItemClass
{




    public override ItemConsumableClass GetConsumable()
    {
        return null;
    }

    public override ItemClass GetItem()
    {
        return this;
    }

    public override ItemMiscClass GetMisc()
    {
        return this;
    }

    public override ItemBulletClass GetBullet()
    {
        return null;
    }
}
