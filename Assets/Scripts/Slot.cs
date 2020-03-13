using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class Slot : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, IPointerDownHandler
{

    public GameObject ItemPrefab;
    /// <summary>
    /// 把Item放在自身下面，如果自身一下已经有Item了，amount++，
    /// 如果没有，根据ItemPrefab 去实例化一个Item,放在下面
    /// </summary>
    /// <param name="item"></param>
    public void CreateItem(Item item)
    {
        if (transform.childCount == 0)
        {
            GameObject itemPrefabs = GameObject.Instantiate(ItemPrefab);
            itemPrefabs.transform.SetParent(transform);
            itemPrefabs.transform.localPosition = Vector3.zero;
            itemPrefabs.transform.localScale = Vector3.one;
            transform.GetChild(0).GetComponent<ItemUI>().SetItem(item);
        }
        else
        {
            transform.GetChild(0).GetComponent<ItemUI>().AddAmount();
        }
    }

    public void CreateItem(Item item,int amount)
    {
        if (transform.childCount == 0)
        {
            GameObject itemPrefabs = GameObject.Instantiate(ItemPrefab);
            itemPrefabs.transform.SetParent(transform);
            itemPrefabs.transform.localPosition = Vector3.zero;
            itemPrefabs.transform.localScale = Vector3.one;
            transform.GetChild(0).GetComponent<ItemUI>().SetItem(item, amount);
        }
        else
        {
            Debug.LogWarning("有孩子了");
            return;
        }
    }
    /// <summary>
    /// 得到当前物品槽存储的物品类型
    /// </summary>
    /// <returns></returns>
    public Item.ItemType GetItemType()
    {
        return transform.GetChild(0).GetComponent<ItemUI>().item.Type;
    }

    /// <summary>
    /// 得到当前物品槽存储的物品ID
    /// </summary>
    /// <returns></returns>
    public int GetItemID()
    {
        return transform.GetChild(0).GetComponent<ItemUI>().item.ID;
    }

    /// <summary>
    /// 当前物品槽的物品容量是否已满
    /// </summary>
    /// <param name="item"></param>
    /// <returns></returns>
    public bool IsFilled()
    {
        ItemUI itemUI = transform.GetChild(0).GetComponent<ItemUI>();
        return itemUI.Amount >= itemUI.item.Capacity;

    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        if (transform.childCount > 0)
        {
            string content = transform.GetChild(0).GetComponent<ItemUI>().item.GetToolTipContent();
            InventoryManager.Instance.ShowToolTipContent(content);
        }
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        if (transform.childCount > 0)
        {
            InventoryManager.Instance.HideToolTipContent();
        }
    }

    public virtual void OnPointerDown(PointerEventData eventData)
    {
        if (eventData.button == PointerEventData.InputButton.Right )//点击鼠标右键
        {
            if (InventoryManager.Instance.IsPickedItem == true) return;//手上有东西时返回
            if (transform.childCount > 0)
            {
                ItemUI currentItemUI = transform.GetChild(0).GetComponent<ItemUI>();
                if (currentItemUI.item is Equipment || currentItemUI.item is Weapon)//如果当前物品为武器装备类型
                {
                    CharacterPanel.Instance.PutOn(currentItemUI);//将物品放入装备槽中
                    InventoryManager.Instance.HideToolTipContent();//隐藏信息提示框
                    CharacterPanel.Instance.UpdatePropertyText();//更新人物属性
                }
            }
        }

        if (eventData.button != PointerEventData.InputButton.Left) return;
        // 1.自身是空的
        //1.1  鼠标上有物体(即 IsPickedItem = true)
        //1.11  按下Ctrl         放置当前鼠标上物品的一个
        //1.12  没有按下Ctrl     放置当前鼠标上物品的全部

        //1.2  鼠标上没有有物体(即 IsPickedItem = false)
        //1.21  返回，不处理

        //2.自身不是空的
        //2.1  鼠标上有物体(即 IsPickedItem = true)
        //2.11  自身ID 等于 鼠标物品ID
        //按下Ctrl          物品一个一个放入物品槽中
        //没有按下Ctrl      物品全部放入物品槽中（判断Capacity是否够放  只能放一部分和全部都能你放下
        //2.12  自身ID 不等于 鼠标物品ID
        //PickedItem和当前物品交换

        //2.2  鼠标上没有物体 (即 IsPickedItem = false)
        //2.21  按下Ctrl         取得当前物品槽中物品的一半
        //2.22  没有按下Ctrl     把当前物品槽里面的物品全部放到鼠标上
        if (transform.childCount > 0) //自身不为空(有ItemUI)
        {
            ItemUI currentItemUI = transform.GetChild(0).GetComponent<ItemUI>();//当前物品的ItemUI
            if (InventoryManager.Instance.IsPickedItem == false) //鼠标上没有物品时
            {
                if (Input.GetKey(KeyCode.LeftControl))
                {
                    int tempAmount = (currentItemUI.Amount + 1) / 2; //当前物品一半的数量
                    InventoryManager.Instance.PickedItemUI(currentItemUI.item, tempAmount);//复制物品信息给PickedItem
                    int remainedAmount = currentItemUI.Amount - tempAmount; //当前物品剩余的数量
                    if (remainedAmount <= 0)
                        Destroy(currentItemUI.gameObject); //删除物品
                    else
                        currentItemUI.SetAmount(remainedAmount); //更新当前物品剩余的数量
                }
                else
                {
                    InventoryManager.Instance.PickedItemUI(currentItemUI.item, currentItemUI.Amount);
                    Destroy(currentItemUI.gameObject); //删除物品
                }
            }
            else  //鼠标上有物品时
            {
                if (currentItemUI.item.Name == InventoryManager.Instance.PickedItem.item.Name)//自身ID 等于 鼠标物品ID
                {
                    if (Input.GetKey(KeyCode.LeftControl))
                    {
                        if (currentItemUI.item.Capacity > currentItemUI.Amount)//当前物品容量大于当前物品已存在的个数
                        {
                            currentItemUI.AddAmount(); //数量加一
                            InventoryManager.Instance.ReducePickedItem();//PickedItem减一
                        }
                        else
                            return;
                    }
                    else
                    {
                        if (currentItemUI.item.Capacity >= InventoryManager.Instance.PickedItem.Amount + currentItemUI.Amount)
                        {   // ↑ 当 当前物品容量大于鼠标上PickedItem个数 + 已储存个数 时，可全部存放下去
                            currentItemUI.AddAmount(InventoryManager.Instance.PickedItem.Amount);
                            InventoryManager.Instance.ReduceAllPickedItem();
                        }
                        else  // 存放到满就无法存放
                        {
                            int tempAmount = currentItemUI.item.Capacity - currentItemUI.Amount;
                            currentItemUI.AddAmount(tempAmount);
                            InventoryManager.Instance.ReducePickedItem(tempAmount);
                        }
                    }
                }
                else // 交换鼠标上和物品槽中的物品
                {
                    Item tempItem = currentItemUI.item;
                    int tempAmount = currentItemUI.Amount;
                    currentItemUI.SetItem(InventoryManager.Instance.PickedItem.item, InventoryManager.Instance.PickedItem.Amount);
                    InventoryManager.Instance.PickedItem.SetItem(tempItem, tempAmount);
                }
            }
        }
        // 1.自身是空的
        //1.1  鼠标上有物体(即 IsPickedItem = true)
        //1.11  按下Ctrl         放置当前鼠标上物品的一个
        //1.12  没有按下Ctrl     放置当前鼠标上物品的全部
        //1.2  鼠标上没有有物体(即 IsPickedItem = false)
        //1.21  返回，不处理
        else // 自身Slot物品槽为空
        {
            if (InventoryManager.Instance.IsPickedItem == true)
            {
                if (Input.GetKey(KeyCode.LeftControl))
                {
                    CreateItem(InventoryManager.Instance.PickedItem.item);
                    InventoryManager.Instance.ReducePickedItem();
                }
                else
                {
                    CreateItem(InventoryManager.Instance.PickedItem.item, InventoryManager.Instance.PickedItem.Amount);
                    InventoryManager.Instance.ReduceAllPickedItem();
                }
            }
            else
                return;
        }
    }
}
