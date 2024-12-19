using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class ItemPlace : MonoBehaviour
{
    public bool isFeather;
    public bool isBrokenFeather;
    public bool isItem;
    public GameObject chooseFrame;

    public Item content = null;

    private void Start()
    {
    }

    /// <summary>
    /// 放置物品，如果已经有物品则交换
    /// </summary>
    /// <param name="item"></param>
    public void AddItem(Item item, Transform previousParent)
    {
        if (isItem)
        {
            Add(item, previousParent);
            item.isEquiped = false;
        }
        else if (item.type == ItemType.Feather && isFeather)
        {
            Add(item, previousParent);
            item.isEquiped = true;
        }
        else if (item.type == ItemType.BrokenFeather && isBrokenFeather)
        {
            Add(item, previousParent);
            item.isEquiped = true;
        }
        else
        {
            item.transform.SetParent(previousParent, false);
            item.transform.SetAsFirstSibling();
        }
    }

    void Add(Item item, Transform previousParent)
    {
        //添加
        item.transform.SetParent(transform, false);
        item.transform.SetAsFirstSibling();

        //交换
        if (content != null)
        {
            content.transform.SetParent(previousParent, false);
            content.transform.SetAsFirstSibling();
        }

        if (previousParent != null)
        {
            previousParent.GetComponent<ItemPlace>().content = content;
        }
        content = item;
    }

    /// <summary>
    /// 清除内容
    /// </summary>
    public void Clear()
    {
        if (content != null)
        {
            Destroy(content.gameObject);
        }
        content = null;

        chooseFrame.SetActive(false);
    }
}
