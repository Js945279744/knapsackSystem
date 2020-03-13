using System.Collections;
using System.Collections.Generic;
using UnityEngine;
/// <summary>
/// 武器类
/// </summary>
public class Weapon : Item {

	public int Damage { get; set; }

    public WeaponType WPType { get; set; }

    public Weapon(int id, string name, ItemType type, ItemQuality quality, string description,
        int capacity, int buyPrice, int sellPrice, string sprite, int damage, WeaponType weaponType)
        : base(id, name, type, quality, description, capacity, buyPrice, sellPrice, sprite)
    {
        this.Damage = damage;
        this.WPType = weaponType;
    }

    public enum WeaponType
    {
        None,
        OffHand,
        MainHand
    }

    public override string GetToolTipContent()
    {
        string weaponType = "";
        switch (WPType)
        {
            case WeaponType.OffHand:
                weaponType = "副手";
                break;
            case WeaponType.MainHand:
                weaponType = "主手";
                break;
            default:
                break;
        }
        string text = base.GetToolTipContent();
        string weaponText = string.Format("{0}<color=blue><size=12>攻击={1}\n武器类型:{2}</size></color>", text, Damage, weaponType);
        return weaponText;
    }
}
