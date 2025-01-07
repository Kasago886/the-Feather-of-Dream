using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Scroll : MonoBehaviour
{
    [HideInInspector] public  bool add;
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
        if (add)
        {
            Transform temp = Instantiate(itemTransform).transform;
            temp.SetParent(contentTransform);
            temp.localPosition = Vector3.zero;
            temp.localRotation = Quaternion.identity;
            temp.localScale = Vector3.one;
            add = false;
        }
    }
}