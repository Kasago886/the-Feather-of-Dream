using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using UnityEditor;
using UnityEngine;
using UnityEngine.UI;

public class BuffUI : Scroll
{
    public Character target;
    public override void Update()
    {
        foreach(Transform child in contentTransform)
        {
            Destroy(child.gameObject);
        }
        if(target != null)
        {
            for (int i = 0; target.GetComponent<Character>().buffList[i] != null; i++) {
                itemTransform.GetChild(0).GetComponent<Text>().text = "buffÃû³Æ";
               // itemTransform.GetChild(1).GetComponent<Image>().sprite = "buffÍ¼±ê";
               itemTransform.GetChild(2).GetComponent<Text>().text = $"{(int)(target.GetComponent<Character>().buffList[i].timer)}s";
                add = true;

            }
        }
    }
}
