using System.Collections;
using System.Collections.Generic;
using System.Text;
using UnityEngine;

/// <summary>
/// 储存类
/// </summary>
public class Inventory : MonoBehaviour
{

    protected Slot[] slots;
    private CanvasGroup canvasGroup;
    private float targetAlpha = 1;
    private float smooth = 3;
    private Player player;

    public virtual void Start()
    {
        slots = GetComponentsInChildren<Slot>();
        canvasGroup = GetComponent<CanvasGroup>();
        player = GameObject.FindGameObjectWithTag("Player").GetComponent<Player>();
    }

    private void Update()
    {
        if (canvasGroup.alpha != targetAlpha)
        {
            canvasGroup.alpha = Mathf.Lerp(canvasGroup.alpha, targetAlpha, smooth * Time.deltaTime);
            if (Mathf.Abs(canvasGroup.alpha - targetAlpha) < .1f)
            {
                canvasGroup.alpha = targetAlpha;
            }
        }
    }
    /// <summary>
    /// 通过ID存储物品
    /// </summary>
    /// <param name="id"></param>
    /// <returns></returns>
    public bool StoreItem(int id)
    {
        Item item = InventoryManager.Instance.GetItemID(id);
        return StoreItem(item);
    }

    /// <summary>
    /// 是否存储成功
    /// </summary>
    /// <param name="item"></param>
    /// <returns></returns>
    public bool StoreItem(Item item)
    {
        if (item == null) return false;

        if (item.Capacity == 1)
        {
            Slot slot = FindEmptySlot();
            if (slot == null)
            {
                Debug.LogWarning("没空的物品槽");
                return false;
            }
            else
            {
                slot.CreateItem(item);
            }
        }
        else
        {
            Slot slot = FindSameIDSlot(item);
            if (slot != null)
            {
                slot.CreateItem(item);
            }
            else //找新的空物品槽
            {
                Slot slot2 = FindEmptySlot();
                if (slot2 != null)
                    slot2.CreateItem(item);
                else
                {
                    Debug.LogWarning("没空的物品槽");
                    return false;
                }
            }
        }
        return true;
    }
    /// <summary>
    /// 找空的物品槽
    /// </summary>
    /// <returns></returns>
    private Slot FindEmptySlot()
    {
        foreach (Slot slot in slots)
        {
            if (slot.transform.childCount == 0)
                return slot;
        }
        return null;
    }
    /// <summary>
    /// 找相同ID的物品槽
    /// </summary>
    /// <returns></returns>
    private Slot FindSameIDSlot(Item item)
    {
        foreach (Slot slot in slots)
        {
            if (slot.transform.childCount >= 1 && slot.GetItemID() == item.ID && slot.IsFilled() == false)
                return slot;
        }
        return null;
    }

    public void Hide()
    {
        canvasGroup.blocksRaycasts = false;
        targetAlpha = 0;
    }

    public void Show()
    {
        canvasGroup.blocksRaycasts = true;
        targetAlpha = 1;
    }
    /// <summary>
    /// 物品的显示和隐藏
    /// </summary>
    public void ItemShowAndHide()
    {
        if (targetAlpha == 0)
            Show();
        else
            Hide();
    }

    /// <summary>
    /// 存储数据
    /// </summary>
    public void SaveInventoryData()
    {
        PlayerPrefs.SetInt("Money", player.Money);//储存金币数据

        StringBuilder sb = new StringBuilder();
        foreach (Slot slot in slots)
        {
            if (slot.transform.childCount > 0)
            {
                ItemUI itemUI = slot.transform.GetChild(0).GetComponent<ItemUI>();
                int amount = itemUI.Amount;
                Item item = itemUI.item;
                sb.Append(item.ID + "," + amount + '-');
            }
            else
            {
                sb.Append("0-");
            }
        }
        PlayerPrefs.SetString(this.gameObject.name, sb.ToString());
    }

    /// <summary>
    /// 加载数据
    /// </summary>
    public void LoadInventoryData()
    {
        if (PlayerPrefs.HasKey("Money")) // 加载金币数据
        {
            player.Money =  PlayerPrefs.GetInt("Money");
        }

        if (PlayerPrefs.HasKey(this.gameObject.name) == false) return;
        string str = PlayerPrefs.GetString(this.gameObject.name);
        string[] data = str.Split('-');
        for (int i = 0; i < data.Length - 1; i++)
        {
            if (data[i] != "0")
            {
                string[] tempData = data[i].Split(',');
                int id = int.Parse(tempData[0]);
                Item item = InventoryManager.Instance.GetItemID(id);
                int amount = int.Parse(tempData[1]);
                for (int j = 0; j < amount; j++)
                {
                    this.StoreItem(item);
                }
            }
        }
    }
}
