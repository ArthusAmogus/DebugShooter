using UnityEngine;

[CreateAssetMenu(fileName = "New Bullet", menuName = "Items/Bullet")]
public class ItemBulletClass : ItemClass
{
    [Header("Consumable Properties")]
    public int damage = 100;
    public enum DamageType { Physical, Explosion, Freeze}
    public DamageType damageType;


    public override ItemConsumableClass GetConsumable()
    {
        return null;
    }

    public override ItemClass GetItem()
    {
        return null;
    }

    public override ItemMiscClass GetMisc()
    {
        return null;
    }

    public override ItemBulletClass GetBullet()
    {
        return this;
    }
}
