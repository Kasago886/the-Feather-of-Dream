using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using UnityEditor;
using UnityEngine;
using UnityEngine.UI;

public class BuffUI : Scroll
{
    public Character target;
    private List<Buff> buffs;
    public override void Start()
    {
        base.Start();
        buffs = new List<Buff>();
    }
    public override void Update()
    {
        base.Update();
        foreach(Transform child in contentTransform)
        {
            Destroy(child.gameObject);
        }
        buffs = target.GetComponent<Character>().buffList;
        if (target != null)
        {
            foreach (Buff _buff in buffs) {
                RectTransform buff = itemTransform;
                buff.GetChild(0).GetComponent<Text>().text =$"{_buff.name}:";
               buff.GetChild(1).GetComponent<Image>().sprite = _buff.sprite;
               buff.GetChild(2).GetComponent<Text>().text = _buff.description;
               buff.GetChild(3).GetComponent<Text>().text = $"{(int)(_buff.timer)}s";
                Additem(buff);

            }
        }
    }
}
