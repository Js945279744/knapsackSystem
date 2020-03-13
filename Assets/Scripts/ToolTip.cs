using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;


public class ToolTip : MonoBehaviour
{

    private Text TipText;
    private Text Content;
    private CanvasGroup canvasGroup;
    private float AlphaValue = 0;
    private float Smooth = 8;

    private void Start()
    {
        TipText = GetComponent<Text>();
        Content = transform.Find("Content").GetComponent<Text>();
        canvasGroup = GetComponent<CanvasGroup>();
        Hide();
    }

    // Update is called once per frame
    void Update () {
        if (canvasGroup.alpha != AlphaValue)
        {
            //canvasGroup.alpha =  Mathf.Lerp(canvasGroup.alpha, AlphaValue, Smooth * Time.deltaTime );
            //if (Mathf.Abs(canvasGroup.alpha - AlphaValue) <= 0.01f)
                canvasGroup.alpha = AlphaValue;
        }
	}

    public void Show(string str)
    {
        TipText.text = str;
        Content.text = str;
        AlphaValue = 1;
    }

    public void Hide()
    {
        AlphaValue = 0;
    }

    public void SetToolTipPosition(Vector3 pos)
    {
        transform.localPosition = pos;
    }
}
