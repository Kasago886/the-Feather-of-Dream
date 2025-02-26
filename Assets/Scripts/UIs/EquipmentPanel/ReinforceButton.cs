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
    public Button button;
    [HideInInspector] public bool open;
    public void Click()
    {
        if (open)
        {
            textName.text=oriName;
            textInformation.text=oriInformation;
        }
        else
        {
            textName.text = itemName;
            textInformation.text=oriInformation;
        }
    }
}
