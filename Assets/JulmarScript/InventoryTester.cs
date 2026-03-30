using UnityEngine;

public class InventoryTester : MonoBehaviour
{
    [SerializeField] InventoryManager inventoryManager;

    [SerializeField] private ItemClass ItemTest1;
    [SerializeField] private ItemClass ItemTest2;
    [SerializeField] private ItemClass ItemTest3;

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            inventoryManager.AddItem(ItemTest1, 1);
        }

        if (Input.GetKeyDown(KeyCode.Alpha2))
        {
            inventoryManager.AddItem(ItemTest2, 1);
        }

        if (Input.GetKeyDown(KeyCode.Alpha3))
        {
            inventoryManager.AddItem(ItemTest3, 1);
        }



        if (Input.GetKeyDown(KeyCode.Alpha8))
        {
            inventoryManager.RemoveItem(ItemTest1, 1);
        }

        if (Input.GetKeyDown(KeyCode.Alpha9))
        {
            inventoryManager.RemoveItem(ItemTest2, 1);
        }

        if (Input.GetKeyDown(KeyCode.Alpha0))
        {
            inventoryManager.RemoveItem(ItemTest3, 1);
        }
    }

}
