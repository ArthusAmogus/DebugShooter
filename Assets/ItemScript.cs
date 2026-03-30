using System.Collections;
using UnityEngine;

public class ItemScript : MonoBehaviour
{
    public ItemClass item;
    [SerializeField] SpriteRenderer icon;

    private void OnEnable()
    {
        GameManager.Instance.DroppedItems.Add(gameObject);
        StartCoroutine(DelayedStart());
    }

    IEnumerator DelayedStart()
    {
        yield return new WaitForEndOfFrame();
        icon = gameObject.GetComponent<SpriteRenderer>();
        icon.sprite = item.Icon;
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            if (item!=null) collision.gameObject.GetComponent<InventoryManager>()?.AddItem(item, 1);
            Destroy(gameObject);
        }
    }
}
