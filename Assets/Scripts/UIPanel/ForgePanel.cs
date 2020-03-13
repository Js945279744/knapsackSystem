using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ForgePanel : Inventory {

    private static ForgePanel _instance;

    public static ForgePanel Instance
    {
        get
        {
            if (_instance == null)
                _instance = GameObject.Find("ForgePanel").GetComponent<ForgePanel>();
            return _instance;
        }
    }

    private List<Formula> formulaList;//用于存放formula配方类

    public override void Start()
    {
        base.Start();
        ParseForgeJSON();
    }

    /// <summary>
    /// 解析JSON
    /// </summary>
    private void ParseForgeJSON()
    {
        formulaList = new List<Formula>();
        TextAsset ta = Resources.Load<TextAsset>("Forge");
        JSONObject j = new JSONObject(ta.text);

        foreach (JSONObject item in j.list)
        {
            int Item1ID = (int)item["Item1ID"].n;
            int Item1Amount = (int)item["Item1Amount"].n;
            int Item2ID = (int)item["Item2ID"].n;
            int Item2Amount = (int)item["Item2Amount"].n;
            int ResID = (int)item["ResID"].n;

            Formula formula = new Formula(Item1ID, Item1Amount, Item2ID, Item2Amount, ResID);
            formulaList.Add(formula);
        }
    }

    /// <summary>
    /// 锻造物品
    /// </summary>
    public void ForgeItem()
    {
        List<int> haveMaterialIDList = new List<int>(); //储存当前锻造材料ID的集合
        foreach (Slot slot in slots)
        {
            if (slot.transform.childCount > 0)
            {
                ItemUI itemUI = slot.transform.GetChild(0).GetComponent<ItemUI>();
                int amount = itemUI.Amount;
                for (int i = 0; i < amount; i++)
                {
                    haveMaterialIDList.Add(itemUI.item.ID);
                }
            }
        }
        //生成满足条件的装备
        foreach (Formula formula in formulaList)
        {
            if (formula.Match(haveMaterialIDList) == true)
            {
                KnapsackPanel.Instance.StoreItem(formula.ResID);
                List<int> tempList =  formula.NeedMateriaList;
                foreach (int id in tempList) //这步到结束为更新锻造炉的材料数量
                {
                    foreach (Slot slot in slots)
                    {
                        if (slot.transform.childCount > 0)
                        {
                            ItemUI itemUI = slot.transform.GetChild(0).GetComponent<ItemUI>();
                            if (itemUI.item.ID == id)
                            {
                                itemUI.ReduceAmount();
                                if (itemUI.Amount <= 0)
                                {
                                    DestroyImmediate(itemUI.gameObject);
                                }
                                break;
                            }
                        }
                    }
                }
                break;
            }
        }
       
    }
}
