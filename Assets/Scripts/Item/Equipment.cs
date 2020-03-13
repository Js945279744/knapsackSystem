using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 装备类
/// </summary>
public class Equipment : Item
{
    /// <summary>
    /// 力量
    /// </summary>
    public int Strength { get; set; }
    /// <summary>
    /// 智力
    /// </summary>
    public int Intellect { get; set; }
    /// <summary>
    /// 敏捷
    /// </summary>
    public int Agility { get; set; }
    /// <summary>
    /// 体力
    /// </summary>
    public int Stamina { get; set; }
    /// <summary>
    /// 装备类型
    /// </summary>
    public EquipmentType EquipType { get; set; }

    public Equipment(int id, string name, ItemType type, ItemQuality quality, string description,
        int capacity, int buyPrice, int sellPrice, string sprite, int strength, int intellect, 
        int agility, int stamina, EquipmentType equipType)
        : base(id, name, type, quality, description, capacity, buyPrice, sellPrice, sprite)
    {
        this.Strength = strength;
        this.Intellect = intellect;
        this.Agility = agility;
        this.Stamina = stamina;
        this.EquipType = equipType;
    }

    public enum EquipmentType
    {
        None,
        Head,
        Neck,
        Chest,
        Ring,
        Leg,
        Bracer,
        Boots,
        Trinket,
        Shoulder,
        Belt,
        OffHand
    }

    public override string GetToolTipContent()
    {
        string text = base.GetToolTipContent();
        string equipTypeText = "";
        switch (EquipType)
        {
            case EquipmentType.Head:
                equipTypeText = "Head";
                break;
            case EquipmentType.Neck:
                equipTypeText = "Neck";
                break;
            case EquipmentType.Chest:
                equipTypeText = "Chest";
                break;
            case EquipmentType.Ring:
                equipTypeText = "Ring";
                break;
            case EquipmentType.Leg:
                equipTypeText = "Leg";
                break;
            case EquipmentType.Bracer:
                equipTypeText = "Bracer";
                break;
            case EquipmentType.Boots:
                equipTypeText = "Boots";
                break;
            case EquipmentType.Trinket:
                equipTypeText = "Trinket";
                break;
            case EquipmentType.Shoulder:
                equipTypeText = "Shoulder";
                break;
            case EquipmentType.Belt:
                equipTypeText = "Belt";
                break;
            case EquipmentType.OffHand:
                equipTypeText = "OffHand"; 
                break;
        }
        string equipmentText = string.Format("{0}<color=blue><size=12>装备类型:{5}\n力量={1}\n智力={2}\n敏捷={3}\n体力={4}\n</size></color>"
            , text,Strength,Intellect,Agility,Stamina, equipTypeText);
        return equipmentText;
    }

}
