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
    Feather,BrokenFeather,Other,Encyclopedia, MemoryFeather
}

public class Item : MonoBehaviour,IPointerDownHandler, IPointerUpHandler
{
    public string itemName;
    public string information;

    public ItemType type;

    public bool isDreamizable;
    public int dreamizeCost;
    public Item dreamizedFeather;
    public ItemInfo dreamizedFeatherInfo;

    public string buffName;
    public float featherHealth;
    float featherMaxHealth;

    public string dialogName;

    [HideInInspector] public bool isEquiped;
    [HideInInspector] public string imageName;
    [HideInInspector] public Feather itemFeather;

    Image image;
    Transform canvas;
    Transform parent;
    bool isHover = false;
    EquipmentPanelManager equipmentPanelManager;
    Player player;
    EquipmentFeatherBuff equipmentFeatherBuff;
    ItemHealth itemHealth;

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
        if (itemInfo.isDreamizable && itemInfo.dreamizedFeather != null)
        {
            dreamizedFeatherInfo = itemInfo.dreamizedFeather;
        }

        imageName = itemInfo.imageName;
        image = GetComponent<Image>();
        Sprite sprite = Resources.Load<Sprite>("ItemIcon/" + itemInfo.imageName);
        //Debug.Log(sprite);
        image.sprite = sprite;
        //Resources.UnloadUnusedAssets();

        buffName = itemInfo.buffName;
        featherHealth = itemInfo.featherHealth;
        featherMaxHealth = itemInfo.featherMaxHealth;

        dialogName = itemInfo.dialogName;
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

        if (isDreamizable && dreamizedFeather != null)
        {
            if (dreamizedFeather != null)
            {
                info.dreamizedFeather = dreamizedFeather.GetItemInfo();
                Debug.Log("DreamizedFeatherInfo:"+info.dreamizedFeather);
            }
            else if (dreamizedFeatherInfo != null)
            {
                info.dreamizedFeather = dreamizedFeatherInfo;
            }
        }

        info.imageName = image.sprite.name;
        info.buffName = buffName;
        /// 羽的信息获取方式
        /// 1.预制体获取：
        /// itemFeather不存在，因此从预制体的featherHealth获取最大生命
        /// 2.实例获取：
        /// itemFeather存在，从feather实例获取当前生命与最大生命
        if (itemFeather == null)
        {
            info.featherHealth = featherHealth;
            info.featherMaxHealth = featherHealth;
        }
        else
        {
            info.featherHealth = itemFeather.health;
            info.featherMaxHealth = itemFeather.maxHealth;
        }

        info.dialogName = dialogName;

        return info;
    }

    /// <summary>
    /// 按下鼠标拖动
    /// </summary>
    /// <param name="eventData"></param>
    public void OnPointerDown(PointerEventData eventData)
    {
        //不允许拖动图鉴 不允许拖动/卸下艾莉之羽
        if (type == ItemType.Encyclopedia || itemName == "艾莉之羽")
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
        //手动触发时自动归位 不允许拖动图鉴 不允许拖动/卸下艾莉之羽
        if (eventData == null || type == ItemType.Encyclopedia || itemName == "艾莉之羽")
        {
            //程序手动触发
            Transpose(null);

            isHover = false;
            image.raycastTarget = true;
            return;
        }

        //保证是左键松开
        if (eventData.button == PointerEventData.InputButton.Left)
        {
            GameObject upGo = eventData.pointerCurrentRaycast.gameObject;
            Transpose(upGo);

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
            transform.SetAsLastSibling();
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
        if (player == null)
        {
            isEquiped = equipState;
            return;
        }

        //Debug.Log(buffName);
        //Debug.Log(isEquiped);
        //Debug.Log(type);
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
                //buff设置
                equipmentFeatherBuff = BuffContainer.GetBuffInstance(buffName) as EquipmentFeatherBuff;
                equipmentFeatherBuff.Init(player);
                equipmentFeatherBuff.feather.item = this;

                //实例获取羽信息
                if (itemFeather != null)
                {
                    equipmentFeatherBuff.feather = itemFeather as EquipmentFeather;
                }
                //预制体获取羽信息
                else
                {
                    equipmentFeatherBuff.feather.health = featherHealth;
                    equipmentFeatherBuff.feather.maxHealth = featherMaxHealth;

                    itemFeather = equipmentFeatherBuff.feather;
                }

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

        //装备界面血量
        if (itemHealth == null)
        {
            itemHealth = transform.GetChild(0).GetComponent<ItemHealth>();
        }
        if (itemHealth.feather == null)
        {
            Feather feather = null;
            if (player == null)
            {
                feather = new Feather();
                feather.maxHealth = featherMaxHealth;
                feather.health = featherHealth;
            }
            else if (itemFeather != null)
            {
                feather = itemFeather;
            }

            if (feather != null)
            {
                itemHealth.gameObject.SetActive(true);
                itemHealth.feather = feather;
            }
        }
    }
}
