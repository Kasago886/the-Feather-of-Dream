using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using UnityEngine.Events;

[Serializable]
public enum ItemType
{
    Feather,BrokenFeather,Other,Encyclopedia
}

public class Item : MonoBehaviour,IPointerDownHandler, IPointerUpHandler
{
    public string itemName;
    public string information;

    public ItemType type;

    public bool isDreamizable;
    public int dreamizeCost;

    public string buffName;
    public float featherHealth;

    [HideInInspector] public bool isEquiped;

    Image image;
    Transform canvas;
    Transform parent;
    bool isHover = false;
    EquipmentPanelManager equipmentPanelManager;
    Player player;
    EquipmentFeatherBuff equipmentFeatherBuff;

    /// <summary>
    /// 根据数据初始化
    /// </summary>
    /// <param name="itemInfo"></param>
    public void Init(ItemInfo itemInfo)
    {
        Start();

        itemName = itemInfo.itemName;
        information = itemInfo.information;
        type = itemInfo.type;
        isDreamizable = itemInfo.isDreamizable;
        dreamizeCost = itemInfo.dreamizeCost;

        image = GetComponent<Image>();
        Sprite sprite = Resources.Load<Sprite>("ItemIcon/"+itemInfo.imageName);
        //Debug.Log(sprite);
        image.sprite = sprite;
        Resources.UnloadUnusedAssets();

        buffName = itemInfo.buffName;
        featherHealth = itemInfo.featherHealth;

        if (type == ItemType.Feather)
        {
            equipmentFeatherBuff = BuffContainer.GetBuffInstance(buffName) as EquipmentFeatherBuff;
            equipmentFeatherBuff.Init(player);
            equipmentFeatherBuff.feather.item = this;
            equipmentFeatherBuff.feather.health = featherHealth;
        }
    }

    /// <summary>
    /// 获取数据
    /// </summary>
    /// <returns></returns>
    public ItemInfo GetItemInfo()
    {
        Start();

        ItemInfo info = new ItemInfo();
        info.itemName = itemName;
        info.type = type;
        info.information = information;
        info.isDreamizable = isDreamizable;
        info.dreamizeCost = dreamizeCost;
        info.imageName = image.sprite.name;
        info.buffName = buffName;
        info.featherHealth = featherHealth;
        return info;
    }

    /// <summary>
    /// 按下鼠标拖动
    /// </summary>
    /// <param name="eventData"></param>
    public void OnPointerDown(PointerEventData eventData)
    {
        //不允许拖动图鉴
        if (type == ItemType.Encyclopedia)
        {
            parent = transform.parent;
            return;
        }

        //保证是左键点击
        if (eventData.button == PointerEventData.InputButton.Left)
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
    }

    /// <summary>
    /// 松开鼠标放置
    /// </summary>
    /// <param name="eventData"></param>
    public void OnPointerUp(PointerEventData eventData)
    {
        if (eventData != null && type != ItemType.Encyclopedia)
        {
            //保证是左键松开
            if (eventData.button == PointerEventData.InputButton.Left)
            {
                GameObject upGo = eventData.pointerCurrentRaycast.gameObject;
                Transpose(upGo);

                isHover = false;
                image.raycastTarget = true;
            }
        }
        else
        {
            //程序手动触发
            Transpose(null);

            isHover = false;
            image.raycastTarget = true;
        }
        
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

    /// <summary>
    /// 设置装备状态
    /// </summary>
    /// <param name="equipState"></param>
    public void SetEquipState(bool equipState)
    {
        if (player == null)
        {
            player = FindAnyObjectByType<Player>();
        }

        //Debug.Log(buffName);
        //Debug.Log(isEquiped);
        //卸下
        if (isEquiped && !equipState)
        {
            if (type == ItemType.Feather)
            {
                player.RemoveBuff(equipmentFeatherBuff);

            }
            else if (type == ItemType.BrokenFeather)
            {
                player.RemoveBuff(buffName);
            }
        }
        //装备
        else if (!isEquiped && equipState)
        {
            if (type == ItemType.Feather)
            {
                player.AddBuff(equipmentFeatherBuff);
            }
            else if (type == ItemType.BrokenFeather)
            {
                player.AddBuff(buffName);
            }
        }

        isEquiped = equipState;
    }

    // Start is called before the first frame update
    void Start()
    {
        image = GetComponent<Image>();
        canvas = GameObject.Find("Canvas").transform;
        equipmentPanelManager = FindObjectOfType<EquipmentPanelManager>();
        player = FindAnyObjectByType<Player>();
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
