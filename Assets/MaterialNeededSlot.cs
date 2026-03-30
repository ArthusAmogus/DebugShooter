using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class MaterialNeededSlot : MonoBehaviour
{
    [SerializeField] Image itemIcon;
    [SerializeField] TextMeshProUGUI itemAmountText;

    public void SetItem(MaterialsNeededData materialsNeeded)
    {
        if (materialsNeeded != null)
        {
            itemIcon.enabled = true;
            itemIcon.sprite = materialsNeeded.item.Icon;
            if (materialsNeeded.amount > 1) itemAmountText.text = materialsNeeded.amount.ToString();
            else itemAmountText.text = "";
        }
        else
        {
            itemIcon.enabled = false;
            itemAmountText.text = "";
        }
    }
}
