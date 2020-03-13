using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShopPanel : Inventory
{
    private static ShopPanel _instance;

    public static ShopPanel Instance
    {
        get
        {
            if (_instance == null)
                _instance = GameObject.Find("ShopPanel").GetComponent<ShopPanel>();
            return _instance;
        }
    }

    public int[] commodity;
    private Player player;

    public override void Start()
    {
        base.Start();
        Init();
        player = GameObject.FindGameObjectWithTag("Player").GetComponent<Player>();
    }

    private void Init()
    {
        foreach (int id in commodity)
        {
            this.StoreItem(id);
        }
    }
    /// <summary>
    /// 购买物品
    /// </summary>
    private void ButItem(Item item)
    {
        int amount = item.BuyPrice;
        if (player.ConsumeMoney(amount))
        {
            KnapsackPanel.Instance.StoreItem(item);
        }
    }

    /// <summary>
    /// 出售物品
    /// </summary>
    /// <param name="item"></param>
    private void SellItem()
    {
        ItemUI currentItemUI = InventoryManager.Instance.PickedItem;

        if (Input.GetKey(KeyCode.LeftControl))
        {
            int amount = currentItemUI.item.SellPrice;
            InventoryManager.Instance.ReducePickedItem(1);
            player.EarnMoney(amount);
        }
        else
        {
            int amount = currentItemUI.item.SellPrice * currentItemUI.Amount;
            InventoryManager.Instance.ReducePickedItem(currentItemUI.Amount);
            player.EarnMoney(amount);
        }
    }
}
