using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CharacterPanel : Inventory
{
    private static CharacterPanel _instance;
    private Text tex;
    private Player player;

    public override void Start()
    {
        base.Start();
        player = GameObject.FindGameObjectWithTag("Player").GetComponent<Player>();
        tex = transform.Find("PropertyPanel/Text").GetComponent<Text>();
        tex.text = "<size=13>力量:10\n智力:10\n敏捷:10\n体力:10\n攻击:10\n</size>";
    }

    public static CharacterPanel Instance
    {
        get
        {
            if (_instance == null)
                _instance = GameObject.Find("CharacterPanel").GetComponent<CharacterPanel>();
            return _instance;
        }
    }

    /// <summary>
    /// 将相同类型的装备/武器放入对应的装备/武器物品槽中
    /// </summary>
    /// <param name="item"></param>
    public void PutOn(ItemUI itemUI)
    {
        foreach (Slot slot in slots)
        {
            EquipmentSlot equipmentSlot = (EquipmentSlot)slot;
            if (equipmentSlot.IsRightItem(itemUI.item))
            {
                if (equipmentSlot.transform.childCount > 0)
                {
                    Item tempItem;
                    ItemUI currentItem = equipmentSlot.transform.GetChild(0).GetComponent<ItemUI>();
                    tempItem = currentItem.item;
                    currentItem.SetItem(itemUI.item);
                    DestroyImmediate(itemUI.gameObject);
                    KnapsackPanel.Instance.StoreItem(tempItem);
                }
                else
                {
                    equipmentSlot.CreateItem(itemUI.item);
                    DestroyImmediate(itemUI.gameObject);
                }
                //break;
            }
        }
    }

    /// <summary>
    /// 将装备/武器放入背包的物品槽中
    /// </summary>
    /// <param name="item"></param>
    public void PutOff(Item item)
    {
        KnapsackPanel.Instance.StoreItem(item);
    }

    /// <summary>
    /// 更新人物属性
    /// </summary>
    public void UpdatePropertyText()
    {
        int strength = 0,intellect = 0,agility = 0,stamina = 0,damage = 0;
        foreach (EquipmentSlot slot in slots)
        {
            if (slot.transform.childCount > 0)
            {
                Item item = slot.transform.GetChild(0).GetComponent<ItemUI>().item;
                if (item is Equipment)
                {
                    Equipment e = (Equipment)item;
                    strength += e.Strength;
                    intellect += e.Intellect;
                    agility += e.Agility;
                    stamina += e.Stamina;
                }
                else if (item is Weapon)
                {
                    Weapon w = (Weapon)item;
                    damage += w.Damage;
                }
            }
        }
        strength += player.Strength;
        intellect += player.Intellect;
        agility += player.Agility;
        stamina += player.Stamina;
        damage += player.Damage;
        tex.text = string.Format("<size=13>力量:{0}\n智力:{1}\n敏捷:{2}\n体力:{3}\n攻击:{4}\n</size>"
            ,strength, intellect, agility, stamina, damage);
    }
}
