using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KnapsackPanel : Inventory
{
    private static KnapsackPanel _instance;

    public static KnapsackPanel Instance
    {
        get
        {
            if (_instance == null)
                _instance = GameObject.Find("KnapsackPanel").GetComponent<KnapsackPanel>();
            return _instance;
        }
    }
}
