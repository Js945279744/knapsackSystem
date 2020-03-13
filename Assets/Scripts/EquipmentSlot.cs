using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class EquipmentSlot : Slot
{
    public Equipment.EquipmentType equipmentType;
    public Weapon.WeaponType weaponType;

    public override void OnPointerDown(PointerEventData eventData)
    {
        if (eventData.button == PointerEventData.InputButton.Right)
        {
            if (InventoryManager.Instance.IsPickedItem == true) return;
            if (transform.childCount > 0)
            {
                ItemUI currentItemUI = transform.GetChild(0).GetComponent<ItemUI>();
                CharacterPanel.Instance.PutOff(currentItemUI.item);
                DestroyImmediate(currentItemUI.gameObject);
                InventoryManager.Instance.HideToolTipContent();
                CharacterPanel.Instance.UpdatePropertyText();
            }
            else
                return;
        }
        if (eventData.button != PointerEventData.InputButton.Left) return;
        //1  手上 有东西
        //1.1   物品槽里 有装备  先判断类型 后交换
        //1.2   物品槽里 没有装备   先判断类型 后放入
        //2  手上 没有东西
        //2.1   物品槽里 有装备
        //2.2   物品槽里 没有装备
        if (InventoryManager.Instance.IsPickedItem == true)  //1  手上 有东西
        {
            ItemUI pickedItem = InventoryManager.Instance.PickedItem; //鼠标上的物品
            if (transform.childCount > 0) //有子类
            {
                ItemUI currentItemUI = transform.GetChild(0).GetComponent<ItemUI>(); //物品槽中的物体
                if (IsRightItem(pickedItem.item) == true)
                {
                    currentItemUI.ExchangeItemUI(pickedItem);
                }
            }
            else
            {
                if (IsRightItem(pickedItem.item) == true)
                {
                    this.CreateItem(pickedItem.item, pickedItem.Amount);
                    InventoryManager.Instance.ReducePickedItem();
                }
            }
            CharacterPanel.Instance.UpdatePropertyText();
        }
        else   //2  手上 没有东西
        {
            if (transform.childCount > 0) //物品槽有物品
            {
                ItemUI currentItemUI = transform.GetChild(0).GetComponent<ItemUI>(); //物品槽中的物体
                InventoryManager.Instance.PickedItemUI(currentItemUI.item, currentItemUI.Amount);
                Destroy(currentItemUI.gameObject);
                CharacterPanel.Instance.UpdatePropertyText();
            }
            else
                return;
        }
    }

    /// <summary>
    /// 判断是否为正确的物品(武器或装备类型是否相同)
    /// </summary>
    /// <param name="itemUI"></param>
    /// <returns></returns>
    public bool IsRightItem(Item item)
    {
        if ((item is Equipment && ((Equipment)item).EquipType == this.equipmentType)
            || (item is Weapon && ((Weapon)item).WPType == this.weaponType))
        {
            return true;
        }
        return false;
    }
}
