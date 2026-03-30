using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class InventoryDragHandler : MonoBehaviour
{
    public static InventoryDragHandler Instance;

    [HideInInspector] public InventorySlotUI draggedSlot;

    [Header("References")]
    public Image dragIcon;
    private RectTransform dragIconRect;
    public Canvas canvas;
    public InventoryManager manager;

    void Awake()
    {
        Instance = this;

        dragIcon.raycastTarget = false;
        dragIcon.enabled = false;
        dragIconRect = dragIcon.GetComponent<RectTransform>();
    }

    void Update()
    {
        if (dragIcon.enabled)
        {
            RectTransformUtility.ScreenPointToLocalPointInRectangle(canvas.transform as RectTransform,
            Input.mousePosition,
            canvas.renderMode == RenderMode.ScreenSpaceOverlay ? null : canvas.worldCamera, out Vector2 pos);

            dragIconRect.anchoredPosition = pos;
        }
    }

    public void BeginDrag(InventorySlotUI slot)
    {
        if (slot.assignedItem == null) return;
        draggedSlot = slot;

        dragIcon.sprite = slot.icon.sprite;
        dragIcon.enabled = true;
        dragIcon.SetNativeSize();
    }

    public void EndDrag(InventorySlotUI targetSlot)
    {
        // Always disable drag icon
        dragIcon.enabled = false;

        // Nothing to do if nothing is being dragged
        if (draggedSlot == null || manager == null)
            return;

        // Prevent self-drop or null target
        if (targetSlot == null || targetSlot == draggedSlot)
        {
            draggedSlot = null;
            return;
        }

        int fromIndex = System.Array.IndexOf(manager.UISlots, draggedSlot);
        int toIndex = System.Array.IndexOf(manager.UISlots, targetSlot);

        // Safety: Make sure both indices are valid
        if (fromIndex < 0 || toIndex < 0)
        {
            draggedSlot = null;
            return;
        }

        // Safety: Make sure indices are within range of inventorySlots
        if (fromIndex >= manager.InvSlots.Count || toIndex >= manager.InvSlots.Count)
        {
            draggedSlot = null;
            return;
        }

        // Prevent swapping with empty slot if you want (optional)
        var fromSlot = manager.InvSlots[fromIndex];
        var toSlot = manager.InvSlots[toIndex];

        if (fromSlot == null || toSlot == null)
        {
            Debug.LogWarning("[InventoryDragHandler] One or both slots are null. Canceling swap.");
            draggedSlot = null;
            return;
        }

        // Perform safe swap
        var temp = manager.InvSlots[fromIndex];
        manager.InvSlots[fromIndex] = manager.InvSlots[toIndex];
        manager.InvSlots[toIndex] = temp;

        manager.DisplayInventory();

        draggedSlot = null;
    }


    public void CancelDrag()
    {
        dragIcon.enabled = false;
        draggedSlot = null;
    }
}
