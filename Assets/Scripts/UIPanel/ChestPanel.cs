using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChestPanel : Inventory {

    private static ChestPanel _instance;

    public static ChestPanel Instance
    {
        get
        {
            if (_instance == null)
                _instance = GameObject.Find("ChestPanel").GetComponent<ChestPanel>();
            return _instance;
        }
    }

    public override void Start()
    {
        base.Start();
        Hide();
    }
}
