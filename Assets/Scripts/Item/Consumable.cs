using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 消耗品类，HP、MP
/// </summary>
public class Consumable : Item
{

    public int HP { get; set; }
    public int MP { get; set; }

    public Consumable(int id, string name, ItemType type, ItemQuality quality, string description,
        int capacity, int buyPrice, int sellPrice, string sprite, int hp, int mp)
        : base(id, name, type, quality, description, capacity, buyPrice, sellPrice, sprite)
    {
        this.HP = hp;
        this.MP = mp;
    }

    public override string ToString()
    {
        string s = string.Format("id:" + ID + ",name:" + Name + ",ItemType:" + Type + ",ItemQuality:" + Quality
            + ",description:" + Description + ",capacity:" + Capacity + ",buyPrice:" + BuyPrice
            + ",sellPrice:" + SellPrice + ",sprite:" + Sprite + ",hp:" + HP + ",mp:" + MP);
        return s;
    }

    public override string GetToolTipContent()
    {
        string text = base.GetToolTipContent();
        string consumableText = string.Format("{0}<color=blue><size=12>hp={1}\nmp={2}</size></color>",text,HP,MP);
        return consumableText;
    }
}
