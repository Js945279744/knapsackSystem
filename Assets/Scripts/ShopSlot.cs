using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class ShopSlot : Slot
{

    public override void OnPointerDown(PointerEventData eventData)
    {
        if (eventData.button == PointerEventData.InputButton.Left && InventoryManager.Instance.IsPickedItem == false)
        {
            if (transform.childCount > 0)
            {
                ItemUI currentItemUI = transform.GetChild(0).GetComponent<ItemUI>();
                transform.parent.parent.SendMessage("ButItem", currentItemUI.item);
            }
        }
        if (InventoryManager.Instance.IsPickedItem == true && eventData.button == PointerEventData.InputButton.Left)
        {
            transform.parent.parent.SendMessage("SellItem");
        }
    }
}
