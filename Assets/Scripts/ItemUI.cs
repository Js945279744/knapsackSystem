using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ItemUI : MonoBehaviour {

	public int Amount { get; set; }
    public Item item { get; set; }

    private Text ItemText;
    private Image ItemImage;

    private Vector3 AnimationScale = new Vector3(1.3f, 1.3f, 1.3f);
    private float TargetScale = 1;
    private float Smoothing = 3.0f;

    private void Awake()
    {
        ItemText = GetComponentInChildren<Text>();
        ItemImage = GetComponent<Image>(); 
    }

    private void Update()
    {
        if (transform.localScale.x != TargetScale)
        {
            float scale = Mathf.Lerp(transform.localScale.x, TargetScale, Smoothing*Time.deltaTime);
            transform.localScale = new Vector3(scale, scale, scale);
            if (Mathf.Abs(AnimationScale.x - TargetScale) <= 0.03f)
                AnimationScale = Vector3.one;
        }
    }

    /// <summary>
    /// 当前物品 与 另一个物品 进行交换显示
    /// </summary>
    public void ExchangeItemUI(ItemUI itemUI)
    {
        Item itemTemp = itemUI.item;
        int amountTemp = itemUI.Amount;
        itemUI.SetItem(this.item, this.Amount);
        this.SetItem(itemTemp, amountTemp);
    }

    /// <summary>
    /// 设置当前物品信息
    /// </summary>
    /// <param name="item"></param>
    /// <param name="amount"></param>
    public void SetItem(Item item,int amount = 1)
    {
        transform.localScale = AnimationScale;
        this.item = item;
        this.Amount = amount;
        //update ui 
        ItemImage.sprite = Resources.Load<Sprite>(item.Sprite);
        if (item.Capacity > 1)
            ItemText.text = amount.ToString();
        else
            ItemText.text = "";
    }

    public void ReduceAmount(int amount = 1)
    {
        transform.localScale = AnimationScale;
        Amount -= amount;
        //update ui 
        if (item.Capacity > 1)
            ItemText.text = Amount.ToString();
        else
            ItemText.text = "";
    }

    public void AddAmount(int amount = 1)
    {
        transform.localScale = AnimationScale;
        Amount += amount;
        //update ui 
        if (item.Capacity > 1)
            ItemText.text = Amount.ToString();
        else
            ItemText.text = "";
    }

    public void SetAmount(int amount)
    {
        transform.localScale = AnimationScale;
        Amount = amount;
        if (item.Capacity > 1)
            ItemText.text = Amount.ToString();
        else
            ItemText.text = "";
    }

    public void HideItemUI()
    {
        gameObject.SetActive(false);
    }

    public void ShowItemUI()
    {
        gameObject.SetActive(true);
    }

    public void SetItemUIPosition(Vector3 vec)
    {
        transform.localPosition = vec;
    }
}
