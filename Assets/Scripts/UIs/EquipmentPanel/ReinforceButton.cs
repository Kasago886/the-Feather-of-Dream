using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class ReinforceButton : MonoBehaviour
{
    [HideInInspector] public string itemName;
    [HideInInspector] public string information;
    [HideInInspector] public string oriName;
    [HideInInspector] public string oriInformation;
    public Text textName;
    public Text textInformation;
    public Text text;
    public Button button;
    [HideInInspector] public bool open;
    private ReinforceButton re;
    private void Start()
    {
        if(button != null)
        {
            re=button.GetComponent<ReinforceButton>();
        }
    }
    private void Update()
    {
        if(button!=null&&re!=null)
        {
            re.itemName = itemName;
            re.information = information;
            re.oriName = oriName;
            re.oriInformation = oriInformation;
            re.open = open;
            gameObject.SetActive(false);
        }
        if (text != null)
        {
            if (!open)
            {
                text.text = "Éý¼¶ÏêÇé";
            }
            else
            {
                text.text = "²ÐÓðÏêÇé";
            }
        }
    }
    public void Click()
    {
        if (open)
        {
            textName.text=oriName;
            textInformation.text=oriInformation;
            open = false;
        }
        else
        {
            textName.text = itemName;
            textInformation.text=information;
            open = true;
        }
    }
}
