using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class InventoryManager : MonoBehaviour
{
    private static InventoryManager _Instance;
    private ToolTip toolTip;
    /// <summary>
    /// 判断是否显示ToolTip
    /// </summary>
    private bool isShowToolTip = false;
    private Canvas canvas;
    public Vector2 vector2 = new Vector2(5, -20);

    public ItemUI PickedItem { get; set; } //鼠标选中的物体
    /// <summary>
    /// 判断当前鼠标上是否有物体
    /// </summary>
    private bool isPickedItem = false; 
    public bool IsPickedItem
    {
        get { return isPickedItem; }
    }

    public static InventoryManager Instance
    {
        get
        {
            if (_Instance == null)
                _Instance = GameObject.Find("InventoryManager").GetComponent<InventoryManager>();
            return _Instance;
        }
    }

    private void Awake()
    {
        ParseItemsJSON();
    }

    private void Start()
    {
        toolTip = GameObject.FindObjectOfType<ToolTip>();
        canvas = GameObject.Find("Canvas").GetComponent<Canvas>();
        PickedItem = GameObject.Find("PickedItem").GetComponent<ItemUI>();
        PickedItem.HideItemUI();
    }


    private void Update()
    {
        if (isPickedItem == true)
        {
            Vector2 position;
            RectTransformUtility.ScreenPointToLocalPointInRectangle(canvas.transform as RectTransform, Input.mousePosition, null, out position);
            PickedItem.SetItemUIPosition(position);
        }
         if (isShowToolTip == true)
        {
            Vector2 position;
            RectTransformUtility.ScreenPointToLocalPointInRectangle(canvas.transform as RectTransform, Input.mousePosition, null, out position);
            toolTip.SetToolTipPosition(position+ vector2);
        }
         if (isPickedItem == true && Input.GetMouseButtonDown(0) && UnityEngine.EventSystems.EventSystem.current.IsPointerOverGameObject(-1) == false)
        {
            isPickedItem = false;
            PickedItem.HideItemUI();
        }
    }

    /// <summary>
    /// 存放物品的集合
    /// </summary>
    private List<Item> itemList;

    /// <summary>
    /// 解析JSON
    /// </summary>
    private void ParseItemsJSON()
    {
        itemList = new List<Item>();
        TextAsset ta = Resources.Load<TextAsset>("items");
        JSONObject j = new JSONObject(ta.text);

        foreach (JSONObject temp in j.list)
        {
            string itemType = temp["type"].str;
            Item.ItemType type = (Item.ItemType)System.Enum.Parse(typeof(Item.ItemType), itemType);
            int id = (int)temp["id"].n;  //公用物品解析
            string name = temp["name"].str;
            //string Quality = temp["quality"].str;
            Item.ItemQuality quality = 
            (Item.ItemQuality)System.Enum.Parse(typeof(Item.ItemQuality), temp["quality"].str);
            string description = temp["description"].str;
            int capacity = (int)(temp["capacity"].n);
            int buyPrice = (int)(temp["buyPrice"].n);
            int sellPrice = (int)(temp["sellPrice"].n);
            string sprite = temp["sprite"].str;

            Item item = null;
            switch (type)
            {
                case Item.ItemType.Consumable:
                    int hp = (int)temp["hp"].n;
                    int mp = (int)temp["mp"].n;
                    item = new Consumable
                       (id, name, type, quality, description, capacity, buyPrice, sellPrice, sprite, hp, mp);
                    break;
                case Item.ItemType.Equipment:
                    int strength = (int)temp["strength"].n;
                    int intellect = (int)temp["intellect"].n;
                    int agility = (int)temp["agility"].n;
                    int stamina = (int)temp["stamina"].n;
                    Equipment.EquipmentType equipType = (Equipment.EquipmentType) System.Enum.Parse(typeof(Equipment.EquipmentType),temp["equipType"].str);
                    item = new Equipment
                       (id, name, type, quality, description, capacity, buyPrice, sellPrice, sprite,strength,intellect,agility,stamina,equipType);
                    break;
                case Item.ItemType.Weapon:
                    int damage = (int)temp["damage"].n;
                    Weapon.WeaponType weaponType = (Weapon.WeaponType)System.Enum.Parse(typeof(Weapon.WeaponType), temp["weaponType"].str);
                    item = new Weapon(id, name, type, quality, description, capacity, buyPrice, sellPrice, sprite,damage,weaponType);
                    break;
                case Item.ItemType.Material:
                    item = new Material(id, name, type, quality, description, capacity, buyPrice, sellPrice, sprite);
                    break;
            }
            itemList.Add(item);
            //Debug.Log(item);
        }
    }

    /// <summary>
    /// 通过ID返回Item
    /// </summary>
    /// <param name="id"></param>
    /// <returns></returns>
    public Item GetItemID(int id)
    {
        foreach (Item item in itemList)
        {
            if (item.ID == id)
            {
                return item;
            }
        }
        return null;
    }

    public void ShowToolTipContent(string content)
    {
        if (isPickedItem) return;
        isShowToolTip = true;
        toolTip.Show(content);
    }

    public void HideToolTipContent()
    {
        isShowToolTip = false;
        toolTip.Hide();
    }

    /// <summary>
    /// 捡起物品槽中指定数量的物品
    /// </summary>
    /// <param name="itemUI"></param>
    /// <param name="amount"></param>
    public void PickedItemUI(Item item,int amount)
    {
        PickedItem.SetItem(item,amount);//将当前物品信息复制给PickedItem(跟随鼠标移动)
        isPickedItem = true; //当前鼠标上有物品了
        PickedItem.ShowItemUI();//显示PickedItem
        toolTip.Hide();//隐藏物品信息框的提示
    }
    /// <summary>
    /// 减少鼠标上物体的数量并更新鼠标是否有物品
    /// </summary>
    public void ReducePickedItem(int amount=1)
    {
        PickedItem.ReduceAmount(amount);
        if (PickedItem.Amount <= 0)
        {
            isPickedItem = false;
            PickedItem.HideItemUI();
        }
    }

    /// <summary>
    /// 鼠标上所有的物品都放入物品槽内
    /// </summary>
    public void ReduceAllPickedItem()
    {
        PickedItem.ReduceAmount(PickedItem.Amount);
        isPickedItem = false;
        PickedItem.HideItemUI();
    }
    /// <summary>
    /// 存储数据
    /// </summary>
    public void SaveInventory()
    {
        KnapsackPanel.Instance.SaveInventoryData();
        ChestPanel.Instance.SaveInventoryData();
        CharacterPanel.Instance.SaveInventoryData();
        ForgePanel.Instance.SaveInventoryData();
    }
    /// <summary>
    /// 读取数据
    /// </summary>
    public void LoadInventory()
    {
        KnapsackPanel.Instance.LoadInventoryData();
        ChestPanel.Instance.LoadInventoryData();
        CharacterPanel.Instance.LoadInventoryData();
        ForgePanel.Instance.LoadInventoryData();
    }
}
