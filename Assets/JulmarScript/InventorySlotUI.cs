using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.EventSystems;

public class InventorySlotUI : MonoBehaviour,
    IBeginDragHandler, IDragHandler, IEndDragHandler, IDropHandler,
    IPointerDownHandler, IPointerUpHandler, IPointerEnterHandler, IPointerExitHandler
{
    public Image icon;
    public TextMeshProUGUI quantityText;
    public TextMeshProUGUI labelText;
    [HideInInspector] public ItemClass assignedItem;
    [HideInInspector] public Button button;
    public CraftingMenu craftingMenu;

    private bool isHolding;
    private float holdTimer;
    private const float holdThreshold = 0.3f; // seconds

    private bool dragStarted;

    void Start()
    {
        button = GetComponent<Button>();
    }

    void Update()
    {
        if (isHolding && !dragStarted)
        {
            holdTimer += Time.unscaledDeltaTime;
            if (holdTimer >= holdThreshold)
            {
                // Begin drag after long press
                dragStarted = true;
                InventoryDragHandler.Instance.BeginDrag(this);
            }
        }
    }

    public void SetItem(InventorySlot slot)
    {
        assignedItem = slot.Item;
        icon.sprite = slot.Item.Icon;
        icon.enabled = true;
        labelText.text = slot.Item.Name;
        quantityText.text = slot.quantity > 1 ? slot.quantity.ToString() : "";
    }

    public void ClearSlot()
    {
        assignedItem = null;
        icon.sprite = null;
        icon.enabled = false;
        labelText.text = "";
        quantityText.text = "";
    }

    public void UseItem()
    {
        if (assignedItem.itemType == ItemClass.ItemType.Consumable)
        {
            PlayerMovement3D.Instance.gameObject.GetComponent<StatsSystem>().HP += assignedItem.GetConsumable().HealAmount;
            InventoryManager.Instance.RemoveItem(assignedItem, 1);
        }
    }

    // ---- Pointer Handling ----

    public void OnPointerDown(PointerEventData eventData)
    {
        if (assignedItem == null) return;
        isHolding = true;
        holdTimer = 0f;
        dragStarted = false;

        
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        isHolding = false;
        holdTimer = 0f;

        if (!dragStarted && assignedItem != null)
        {
            UseItem();
        }
    }

    public void OnBeginDrag(PointerEventData eventData)
    {
        // On PC drag starts immediately
        if (Application.platform != RuntimePlatform.Android && assignedItem != null)
        {
            InventoryDragHandler.Instance.BeginDrag(this);
        }
    }

    public void OnDrag(PointerEventData eventData)
    {
        // Handled in InventoryDragHandler
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        GameObject targetObj = eventData.pointerCurrentRaycast.gameObject;
        InventorySlotUI targetSlot = targetObj ? targetObj.GetComponentInParent<InventorySlotUI>() : null;
        InventoryDragHandler.Instance.EndDrag(targetSlot);

        isHolding = false;
        dragStarted = false;
        holdTimer = 0f;
    }

    public void OnDrop(PointerEventData eventData)
    {
        // This is called on the target slot automatically
    }


    // ---- Mouse Hover Functions ----

    public void OnPointerEnter(PointerEventData eventData)
    {
        if (assignedItem != null)
        {
            Debug.Log($"Mouse entered slot with: {assignedItem.Name}");
            craftingMenu.DisplayItemInfo(assignedItem);
        }
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        if (assignedItem != null)
        {
            Debug.Log($"Mouse exited slot with: {assignedItem.Name}");
            craftingMenu.DisplayItemInfo(null);
        }
    }
}
