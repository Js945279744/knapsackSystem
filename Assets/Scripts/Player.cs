using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Player : MonoBehaviour
{
    private Text moneyText;
    private int money = 100;

    public int Money
    {
        get { return money; }
        set {
            money = value;
            moneyText.text = money.ToString();
        }
    }

    private void Start()
    {
        moneyText = GameObject.Find("Money/Text").GetComponent<Text>();
        moneyText.text = money.ToString();
    }

    #region 人物面板-基础属性
    private int strength = 10;
    private int intellect = 10;
    private int agility = 10;
    private int stamina = 10;
    private int damage = 10;

    public int Strength
    {
        get
        {
            return strength;
        }
    }

    public int Intellect
    {
        get
        {
            return intellect;
        }
    }
    public int Agility
    {
        get
        {
            return agility;
        }
    }
    public int Stamina
    {
        get
        {
            return stamina;
        }
    }
    public int Damage
    {
        get
        {
            return damage;
        }
    }
    #endregion
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.G))
        {
            int a = Random.Range(6, 14);
            KnapsackPanel.Instance.StoreItem(a);
        }

        if (Input.GetKeyDown(KeyCode.B))
        {
            ChestPanel.Instance.ItemShowAndHide();
        }

        if (Input.GetKeyDown(KeyCode.Y))
        {
            KnapsackPanel.Instance.ItemShowAndHide();
        }

        if (Input.GetKeyDown(KeyCode.C))
        {
            CharacterPanel.Instance.ItemShowAndHide();
        }
    }

    /// <summary>
    /// 花费金钱
    /// </summary>
    public bool ConsumeMoney(int amount)
    {
        if (money < amount) return false;
        money -= amount;
        moneyText.text = money.ToString();
        return true;
    }

    /// <summary>
    /// 赚取金钱
    /// </summary>
    public void EarnMoney(int amount)
    {
        money += amount;
        moneyText.text = money.ToString();
    }
}
