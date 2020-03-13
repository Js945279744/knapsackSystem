using System.Collections;
using System.Collections.Generic;
using UnityEngine;
/// <summary>
/// 锻造配方类
/// </summary>
public class Formula
{

    public int Item1ID { get; private set; } //锻造需要的第一份材料 
    public int Item1Amount { get; private set; }  //锻造需要的第一份材料的个数
    public int Item2ID { get; private set; } //锻造需要的第二份材料 
    public int Item2Amount { get; private set; } //锻造需要的第二份材料的个数 

    public int ResID { get; private set; } //锻造成功所生成的物品
    private List<int> needMateriaList = new List<int>();
    public List<int> NeedMateriaList
    {
        get {
            return needMateriaList;
        }
    }

    public Formula(int Item1ID, int Item1Amount, int Item2ID, int Item2Amount, int ResID)
    {
        this.Item1ID = Item1ID;
        this.Item1Amount = Item1Amount;
        this.Item2ID = Item2ID;
        this.Item2Amount = Item2Amount;
        this.ResID = ResID;

        for (int i = 0; i < Item1Amount; i++)
        {
            needMateriaList.Add(Item1ID);
        }
        for (int i = 0; i < Item2Amount; i++)
        {
            needMateriaList.Add(Item2ID);
        }
    }

    /// <summary>
    /// 当前材料是否匹配秘籍需要的材料
    /// </summary>
    public bool Match(List<int> haveMaterialIDList)
    {
        foreach (int item in needMateriaList)
        {
            Debug.Log(item);
            bool isRemove = haveMaterialIDList.Remove(item);
            if (isRemove == false)
            {
                return false;
            }
        }
        return true;
    }
}
