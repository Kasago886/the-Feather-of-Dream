using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[Serializable]
public class Scroll : MonoBehaviour
{
    private ScrollRect scrollRect;
    private float[] rateArr;
    //获取Content的RectTransform
    [HideInInspector] public RectTransform contentTransform;
    //设置添加的预制体
    public RectTransform itemTransform;
    // Use this for initialization
    public virtual void Start()
    {
        //获取自身的ScrollRect组件
        scrollRect = GetComponent<ScrollRect>();
        contentTransform = transform.Find("Viewport").Find("Content").GetComponent<RectTransform>();
    }

    // Update is called once per frame
    public virtual void Update()
    {
    }
    public virtual Transform Additem(RectTransform item)
    {
        Transform temp = Instantiate(item).transform;
        temp.SetParent(contentTransform);
        temp.localPosition = Vector3.zero;
        temp.localRotation = Quaternion.identity;
        temp.localScale = Vector3.one;

        return temp;
    }

    public HpUI AddHp()
    {
        Transform temp = Instantiate(itemTransform).transform;
        temp.SetParent(contentTransform);
        temp.localPosition = Vector3.zero;
        temp.localRotation = Quaternion.identity;
        temp.localScale = Vector3.one;

        return temp.GetComponent<HpUI>();
    }

    public void ClearAllContent()
    {
        if (contentTransform == null)
        {
            Start();
        }

        for (int i = 0; i< contentTransform.childCount; i++)
        {
            Destroy(contentTransform.GetChild(i).gameObject);
        }
    }
}