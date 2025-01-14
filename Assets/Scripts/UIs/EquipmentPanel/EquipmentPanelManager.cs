using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class EquipmentPanelManager : MonoBehaviour
{
    public Text levelText;
    public Text expNumber;
    public RectTransform expProgress;
    public Text tenacity;
    public Text strength;
    public Text feather;
    public Text dream;
    public Transform featherEquipContent;
    public Transform brokenFeatherEquipContent;
    public Transform itemContent;

    public Text itemName;
    public Text itemInformation;

    public GameObject equipButton;
    public GameObject unequipButton;
    public GameObject dreamizeButton;
    public Text dreamizeCost;

    public Transform encyclopediaContent;

    public GameObject itemObj;
    public GameObject itemPlaceObj;

    public GameObject donotTouchPanel;

    [HideInInspector] public List<Item> itemsOnHand = new List<Item>();

    ArchiveManager archiveManager;
    ItemPlace selectedItemPlace = null;
    bool isShow = false;
    AnimationBoolManager animationBoolManager;

    // Start is called before the first frame update
    void Start()
    {
        archiveManager = FindAnyObjectByType<ArchiveManager>();
        animationBoolManager = GetComponent<AnimationBoolManager>();

        SetupPanel();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    /// <summary>
    /// 根据存档信息显示物品栏
    /// </summary>
    public void SetupPanel()
    {
        selectedItemPlace = null;
        //读取存档
        Archive archive = archiveManager.currentArchive;

        SetUpPlayerInfo();

        //featherEquip
        int count = 0;
        for (int i = 0; i < 5; i++)
        {
            Transform parent = featherEquipContent.GetChild(i);
            ItemPlace ip = parent.GetComponent<ItemPlace>();
            //清空
            ip.Clear();

            //防止超出索引
            if (count >= archive.equipedFeather.items.Length)
                continue;

            //对应位置
            if (i == archive.equipedFeather.items[count].position)
            {
                GenerateSingleItem(ip, archive.equipedFeather.items[count]);

                count++;
            }
        }

        //brokenFeatherEquip
        count = 0;
        for (int i = 0; i < 10; i++)
        {
            Transform parent = brokenFeatherEquipContent.GetChild(i);
            ItemPlace ip = parent.GetComponent<ItemPlace>();
            //清空
            ip.Clear();

            //防止超出索引
            if (count >= archive.equipedBrokenFeather.items.Length)
                continue;

            //对应位置
            if (i == archive.equipedBrokenFeather.items[count].position)
            {
                GenerateSingleItem(ip, archive.equipedBrokenFeather.items[count]);

                count++;
            }
        }

        //items
        //清空
        count = itemContent.childCount;
        for (int i = 0; i < count; i++)
        {
            Destroy(itemContent.GetChild(i).gameObject);
        }
        count = 0;
        //预留足够空位置(30个)
        int addition = 30;
        for (int i = 0; addition > 0 || count < archive.items.items.Length; i++)
        {
            //添加物品
            GameObject parent = Instantiate(itemPlaceObj, itemContent, false);
            ItemPlace ip = parent.GetComponent<ItemPlace>();

            //防止超出索引
            if (count >= archive.items.items.Length)
            {
                addition--;
                continue;
            }

            //对应位置
            if (i == archive.items.items[count].position)
            {
                GenerateSingleItem(ip, archive.items.items[count]);

                count++;
            }
            else
            {
                addition--;
            }
        }

        //encyclopedia
        //清空
        count = encyclopediaContent.childCount;
        for (int i = 0; i < count; i++)
        {
            Destroy(encyclopediaContent.GetChild(i).gameObject);
        }
        count = 0;
        for (int i = 0; count < archive.encyclopedia.items.Length; i++)
        {
            //添加物品
            GameObject parent = Instantiate(itemPlaceObj, encyclopediaContent, false);
            ItemPlace ip = parent.GetComponent<ItemPlace>();

            //对应位置
            if (i == archive.encyclopedia.items[count].position)
            {
                GenerateSingleItem(ip, archive.encyclopedia.items[count]);

                count++;
            }
        }

        //隐藏按钮
        equipButton.SetActive(false);
        dreamizeButton.SetActive(false);
        unequipButton.SetActive(false);
    }

    public void SetUpPlayerInfo()
    {
        //读取存档
        Archive archive = archiveManager.currentArchive;
        PlayerInfo playerInfo = archive.playerInfo;
        //Debug.Log(archive.level);

        levelText.text = "Lv." + playerInfo.level.ToString();
        //防止除以0
        if (playerInfo.maxExp == 0)
        {
            playerInfo.maxExp = 1;
        }
        expNumber.text = playerInfo.currentExp.ToString() + "/" + playerInfo.maxExp.ToString();
        expProgress.sizeDelta = new Vector2(300 * playerInfo.currentExp / playerInfo.maxExp, expProgress.sizeDelta.y);
        tenacity.text = playerInfo.tenacity.ToString();
        strength.text = playerInfo.strength.ToString();
        feather.text = playerInfo.feather.ToString();
        dream.text = playerInfo.dream.ToString();
    }

    /// <summary>
    /// 生成物品
    /// </summary>
    /// <param name="ip">物品所在位置</param>
    /// <param name="item">物品信息</param>
    /// <param name="isEquiped">物品是否被装备</param>
    void GenerateSingleItem(ItemPlace ip, ItemInfo item)
    {
        //初始化物品
        GameObject instance = Instantiate(itemObj);
        instance.GetComponent<Item>().Init(item);
        //绑定到ItemPlace
        ip.AddItem(instance.GetComponent<Item>(), null);
    }

    /// <summary>
    /// 物品被点击时的事件
    /// </summary>
    /// <param name="item"></param>
    public void OnClickItem(Item item, ItemPlace itemPlace)
    {
        //ChooseFrame
        itemPlace.chooseFrame.gameObject.SetActive(true);
        if (selectedItemPlace != null && selectedItemPlace != itemPlace)
        {
            selectedItemPlace.chooseFrame.gameObject.SetActive(false);
        }
        selectedItemPlace = itemPlace;

        //Information
        itemName.text = item.itemName;
        itemInformation.text = item.information;

        //equipButton
        if (item.isEquiped)
        {
            equipButton.SetActive(false);
            unequipButton.SetActive(true);
        }
        else
        {
            if (item.type == ItemType.Feather && FindAvailablePlace(featherEquipContent) != null)
            {
                equipButton.SetActive(true);
            }
            else if (item.type == ItemType.BrokenFeather && FindAvailablePlace(brokenFeatherEquipContent) != null)
            {
                equipButton.SetActive(true);
            }
            else
            {
                equipButton.SetActive(false);
            }

            unequipButton.SetActive(false);
        }

        //dreamizeButton
        if (item.type == ItemType.BrokenFeather)
        {
            dreamizeButton.SetActive(item.isDreamizable);
            if (item.isDreamizable)
            {
                if (item.dreamizeCost > archiveManager.currentArchive.playerInfo.dream)
                {
                    dreamizeButton.GetComponent<Button>().interactable = false;
                }
                else
                {
                    dreamizeButton.GetComponent<Button>().interactable = true;
                }
                dreamizeCost.text = "消耗：" + item.dreamizeCost.ToString();
            }
        }
        else
        {
            dreamizeButton.SetActive(false);
        }

    }

    /// <summary>
    /// 保存物品状态
    /// </summary>
    public Archive SaveItemsState(Archive inputArchive)
    {
        //equipedFeather
        inputArchive.equipedFeather.items = GetItemInfos(featherEquipContent).ToArray();
        //equipedBrokenFeather
        inputArchive.equipedBrokenFeather.items = GetItemInfos(brokenFeatherEquipContent).ToArray();
        //items
        inputArchive.items.items = GetItemInfos(itemContent).ToArray();
        //encyclopedia
        inputArchive.encyclopedia.items = GetItemInfos(encyclopediaContent).ToArray();

        return inputArchive;
    }

    /// <summary>
    /// 获取所有物品信息
    /// </summary>
    /// <param name="contentTransform"></param>
    /// <returns></returns>
    List<ItemInfo> GetItemInfos(Transform contentTransform)
    {
        //遍历所有子对象(itemPlace)
        int count = contentTransform.childCount;
        List<ItemInfo> list = new List<ItemInfo>();
        for (int i = 0; i < count; i++)
        {
            GameObject itemPlaceObj = contentTransform.GetChild(i).gameObject;
            if (itemPlaceObj.GetComponent<ItemPlace>().content != null)
            {
                //获取物品信息
                ItemInfo info = itemPlaceObj.GetComponent<ItemPlace>().content.GetItemInfo();
                info.position = i;
                list.Add(info);
            }
        }
        return list;
    }

    /// <summary>
    /// 切换展示状态
    /// </summary>
    public void SwitchShow()
    {
        if (isShow)
        {
            //在鼠标上的物品归位
            //foreach (Item item in itemsOnHand) //不能用foreach，因为OnPointerUp会删除列表中的元素，导致报错
            for (int i = itemsOnHand.Count-1; i >= 0 ; i--) //应当使用for循环倒序删除，保证索引不会出错
            {
                Item item = itemsOnHand[i];
                if (item != null)
                {
                    item.OnPointerUp(null);
                }
            }

            donotTouchPanel.SetActive(true);
            //SaveItemsState();
        }
        else
        {
            donotTouchPanel.SetActive(false);
            //SetupPanel();
        }

        animationBoolManager.SwitchValue("appear");
        isShow = !isShow;
    }

    /// <summary>
    /// 装备物品
    /// </summary>
    public void EquipItem()
    {
        if (selectedItemPlace != null)
        {
            if (selectedItemPlace.content != null)
            {
                Item item = selectedItemPlace.content;
                if (item.type == ItemType.Feather)
                {
                    FindAvailablePlace(featherEquipContent).AddItem(item, item.transform.parent);
                }
                else if (item.type == ItemType.BrokenFeather)
                {
                    FindAvailablePlace(brokenFeatherEquipContent).AddItem(item,item.transform.parent);
                }
                OnClickItem(item, item.GetComponentInParent<ItemPlace>());
            }
        }
    }

    /// <summary>
    /// 卸下物品
    /// </summary>
    public void UnequipItem()
    {
        if (selectedItemPlace != null)
        {
            if (selectedItemPlace.content != null)
            {
                Item item = selectedItemPlace.content;
                FindAvailablePlace(itemContent).AddItem(item, item.transform.parent);
                OnClickItem(item, item.GetComponentInParent<ItemPlace>());
            }
        }
    }

    /// <summary>
    /// 获取空的位置
    /// </summary>
    /// <param name="contentTransform"></param>
    /// <returns></returns>
    ItemPlace FindAvailablePlace(Transform contentTransform)
    {
        //遍历所有子对象(itemPlace)
        int count = contentTransform.childCount;
        for (int i = 0; i < count; i++)
        {
            ItemPlace itemPlace = contentTransform.GetChild(i).gameObject.GetComponent<ItemPlace>();
            
            if (itemPlace.content == null)
            {
                return itemPlace;
            }
        }
        return null;
    }
}
