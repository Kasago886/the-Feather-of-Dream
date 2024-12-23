using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

[Serializable]
public enum ItemType
{
    Feather,BrokenFeather,Other
}

public class Item : MonoBehaviour,IPointerDownHandler, IPointerUpHandler
{
    public string itemName;
    public string information;

    public ItemType type;

    public bool isDreamizable;
    public int dreamizeCost;

    [HideInInspector] public bool isEquiped;

    Image image;
    Transform canvas;
    Transform parent;
    bool isHover = false;
    EquipmentPanelManager equipmentPanelManager;

    /// <summary>
    /// 根据数据初始化
    /// </summary>
    /// <param name="itemInfo"></param>
    public void Init(ItemInfo itemInfo)
    {
        itemName = itemInfo.itemName;
        information = itemInfo.information;
        type = itemInfo.type;
        isDreamizable = itemInfo.isDreamizable;
        dreamizeCost = itemInfo.dreamizeCost;

        image = GetComponent<Image>();
        Sprite sprite = Resources.Load<Sprite>("ItemIcon/"+itemInfo.imageName);
        Debug.Log(sprite);
        image.sprite = sprite;
        Resources.UnloadUnusedAssets();
    }

    /// <summary>
    /// 获取数据
    /// </summary>
    /// <returns></returns>
    public ItemInfo GetItemInfo()
    {
        ItemInfo info = new ItemInfo();
        info.itemName = itemName;
        info.type = type;
        info.information = information;
        info.isDreamizable = isDreamizable;
        info.dreamizeCost = dreamizeCost;
        info.imageName = image.sprite.name;
        return info;
    }

    /// <summary>
    /// 按下鼠标拖动
    /// </summary>
    /// <param name="eventData"></param>
    public void OnPointerDown(PointerEventData eventData)
    {
        equipmentPanelManager.OnClickItem(this, GetComponentInParent<ItemPlace>());
        equipmentPanelManager.itemsOnHand.Add(this);

        parent = transform.parent;
        transform.SetParent(canvas);
        transform.SetAsLastSibling();

        isHover = true;
        //取消自身射线检测便于检测松开鼠标时的格子
        image.raycastTarget = false;
    }

    /// <summary>
    /// 松开鼠标放置
    /// </summary>
    /// <param name="eventData"></param>
    public void OnPointerUp(PointerEventData eventData)
    {
        GameObject upGo;
        if (eventData != null)
        {
            upGo = eventData.pointerCurrentRaycast.gameObject;
        }
        else
        {
            upGo = null;
        }
        
        Transpose(upGo);

        isHover = false;
        image.raycastTarget = true;
    }

    /// <summary>
    /// 尝试放置物品
    /// </summary>
    /// <param name="upGo">目标ItemPlace</param>
    public void Transpose(GameObject upGo)
    {
        //放置位置
        bool isTransposed = false;
        if (upGo != null)
        {
            if (upGo.GetComponent<ItemPlace>() != null)
            {
                upGo.GetComponent<ItemPlace>().AddItem(this, parent);
                isTransposed = true;
            }
        }

        //回到原位
        if (!isTransposed)
        {
            transform.SetParent(parent);
            transform.SetAsFirstSibling();
        }

        equipmentPanelManager.OnClickItem(this, GetComponentInParent<ItemPlace>());
        equipmentPanelManager.itemsOnHand.Remove(this);
        transform.localPosition = Vector3.zero;
    }

    // Start is called before the first frame update
    void Start()
    {
        image = GetComponent<Image>();
        canvas = GameObject.Find("Canvas").transform;
        equipmentPanelManager = FindObjectOfType<EquipmentPanelManager>();
    }

    // Update is called once per frame
    void Update()
    {
        if (isHover)
        {
            transform.position = Input.mousePosition;
        }
    }
}
