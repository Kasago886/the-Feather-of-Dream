using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;
using static UnityEditor.Progress;
[RequireComponent(typeof(PolygonCollider2D))]
//注意如果使用的是mode2的话那么请安装上rigidbody2D组件
public class InteractiveObject : MonoBehaviour
{
    [Header("鼠标触发:")]
    [Tooltip("如果同时选择两种触发模式，就只进行鼠标触发")]
    public bool mouseTrigger;
    [Space(3)]
    [Header("按键触发:")]
    [Tooltip("如果同时选择两种触发模式，就只进行鼠标触发")]
    public bool keyTrigger;
    [Header("按键触发模式:")]
    [Tooltip("距离触发模式")]
    public bool mode1;
    [Header("可触发事件的最远距离:")]
    public float maxDistance;
    [Header("添加更多限制:")]
    public bool moreLimit;
    [Header("可触发事件的水平限制")]
    [Tooltip("小于可触发事件的最远距离")]
    public float maxDistanceOfHorizontal;
    [Header("可触发事件的垂直限制")]
    [Tooltip("小于可触发事件的最远距离")]
    public float maxDistanceOfVertical;
    [Space(9)]
    [Tooltip("碰撞触发模式")]
    public bool mode2;
    [Header("触发使用的按键:")]
    [Tooltip("小写字母")]
    public string nameOfKey;
    [Tooltip("提示按键的预制体")]
    public GameObject noticer;

    private float distance;
    private int id;
    private GameObject player;
    protected virtual void Start()
    {
        id=Random.Range(-100000,100000);
        player = GameObject.FindGameObjectWithTag(Consts.PlayerTag);

        SetNoticer();
        HideNoticer();

        if (mouseTrigger)
        {
            noticer.GetComponentInChildren<Text>().text = "Click";
            ShowNoticer();
        }
    }
    protected virtual void Update()
    {
        KeyTriggerMode1();
    }
    /// <summary>
    /// 这个方法是用于距离判定的
    /// </summary>
    void KeyTriggerMode1()
    {
        if (keyTrigger && !mouseTrigger && mode1&&!moreLimit)
        {
            distance = Mathf.Abs(Vector3.Distance(player.transform.position, gameObject.transform.position));
            if (distance < maxDistance)
            {
                ShowNoticer();
                if (Input.GetKeyDown(nameOfKey))
                {
                    Interact();
                }
            }
            else
            {
                HideNoticer();
            }
        }
        else if (keyTrigger && !mouseTrigger && mode1 && moreLimit)
        {
            distance = Mathf.Abs(Vector3.Distance(player.transform.position, gameObject.transform.position));
            if (maxDistanceOfHorizontal != 0&&maxDistanceOfVertical==0)
            {
                if (distance < maxDistance && Mathf.Abs(player.transform.position.x - transform.position.x) <= maxDistanceOfHorizontal)
                {
                    ShowNoticer();
                    if (Input.GetKeyDown(nameOfKey))
                    {
                        Interact();
                    }
                }
                else
                {
                    HideNoticer();
                }
            }
            if(maxDistanceOfVertical != 0&&maxDistanceOfHorizontal==0)
            {
                if (distance < maxDistance && Mathf.Abs(player.transform.position.y - transform.position.y) <= maxDistanceOfVertical)
                {
                    ShowNoticer();
                    if (Input.GetKeyDown(nameOfKey))
                    {
                        Interact();
                    }
                }
                else
                {
                    HideNoticer();
                }
            }
        }
    }
    private void OnMouseDown()
    {
        if (mouseTrigger)
        {
            Interact();
        }
    }
    /// <summary>
    /// 这个方法是玩家进入触发器后，将自身添加到玩家控制交互的链表的方法
    /// </summary>
    /// <param name="collision"><>
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(keyTrigger && !mouseTrigger && mode2&&collision.tag==Consts.PlayerTag)
        {
            PlayerInteract.trigger.Add(id,gameObject);
            ShowNoticer() ;
        }
    }
    /// <summary>
    /// 这个方法是玩家离开触发器后，将自身从玩家控制交互的链表清除的方法
    /// </summary>
    /// <param name="collision"></param>
    private void OnTriggerExit2D(Collider2D collision)
    {
        if (keyTrigger && !mouseTrigger && mode2)
        {
            PlayerInteract.trigger.Remove(id);
            HideNoticer() ;
        }
    }
    /// <summary>
    /// 这个方法是需要覆写的触发后发生作用的方法
    /// </summary>
    public virtual void Interact()
    {

    }

    /// <summary>
    /// 按键提示
    /// </summary>
    public void ShowNoticer()
    {
        noticer.SetActive(true);
    }
    public void HideNoticer()
    {
        noticer.SetActive(false);
    }
    public void SetNoticer()
    {
        GameObject instance = Instantiate(noticer);
        instance.GetComponent<Noticer>().target = gameObject;
        instance.GetComponentInChildren<Text>().text = nameOfKey.ToUpper();
        noticer = instance;
    }

    /// <summary>
    /// 这个方法是用来观测距离触发的可触发范围的
    /// </summary>
    private void OnDrawGizmos()
    {
        if (!moreLimit)
        {
            float r = maxDistance;
            for (int i = 0; i < 360; i++)
            {
                Gizmos.DrawLine(new Vector3(transform.position.x + r * Mathf.Cos((i * Mathf.PI) / 180), transform.position.y + r * Mathf.Sin((i * Mathf.PI) / 180), 0), new Vector3(transform.position.x + r * Mathf.Cos(((i + 1) * Mathf.PI) / 180), transform.position.y + r * Mathf.Sin(((i + 1) * Mathf.PI) / 180), 0));
            }
        }
        else
        {
            float r = maxDistance;
            Vector3 pos;
            if (maxDistanceOfHorizontal != 0)
            {
                pos = new Vector3(transform.position.x + maxDistanceOfHorizontal, transform.position.y, 0);
            }
            else
            {
                pos = new Vector3(transform.position.x + r,transform.position.y, 0);
            }
            for (int i = 0; i < 360; i++)
            {
                if ((maxDistanceOfHorizontal == 0 && maxDistanceOfVertical == 0)|| (maxDistanceOfHorizontal != 0 && maxDistanceOfVertical != 0))
                {
                    Gizmos.DrawLine(pos, new Vector3(transform.position.x + r * Mathf.Cos(((i + 1) * Mathf.PI) / 180), transform.position.y + r * Mathf.Sin(((i + 1) * Mathf.PI) / 180), 0));
                    pos = new Vector3(transform.position.x + r * Mathf.Cos(((i + 1) * Mathf.PI) / 180), transform.position.y + r * Mathf.Sin(((i + 1) * Mathf.PI) / 180), 0);
                }
                else if (maxDistanceOfHorizontal == 0 && maxDistanceOfVertical != 0)
                {
                     if(Mathf.Abs(r * Mathf.Sin((i + 1) * Mathf.PI / 180)) >= maxDistanceOfVertical)
                    {
                        Gizmos.DrawLine(pos, new Vector3(transform.position.x + r * Mathf.Cos(((i + 1) * Mathf.PI) / 180), transform.position.y + maxDistanceOfVertical * (Mathf.Sin((i + 1) * Mathf.PI/180) / Mathf.Abs(Mathf.Sin((i + 1) * Mathf.PI / 180))), 0));
                        pos = new Vector3(transform.position.x + r * Mathf.Cos(((i + 1) * Mathf.PI) / 180), transform.position.y + maxDistanceOfVertical* (Mathf.Sin((i + 1) * Mathf.PI/180) / Mathf.Abs(Mathf.Sin((i + 1) * Mathf.PI/180))), 0);
                    }
                    else
                    {
                        Gizmos.DrawLine(pos, new Vector3(transform.position.x + r * Mathf.Cos(((i + 1) * Mathf.PI) / 180), transform.position.y + r * Mathf.Sin(((i + 1) * Mathf.PI) / 180), 0));
                        pos = new Vector3(transform.position.x + r * Mathf.Cos(((i + 1) * Mathf.PI) / 180), transform.position.y + r * Mathf.Sin(((i + 1) * Mathf.PI) / 180), 0);
                    }
                }
                else if(maxDistanceOfHorizontal!=0&&maxDistanceOfVertical==0)
                {
                    if(Mathf.Abs(r*Mathf.Cos((i + 1) * Mathf.PI/180))>=maxDistanceOfHorizontal)
                    {
                        Gizmos.DrawLine(pos, new Vector3(transform.position.x + maxDistanceOfHorizontal*(Mathf.Cos((i + 1) * Mathf.PI/180)/Mathf.Abs(Mathf.Cos((i + 1) * Mathf.PI/180))), transform.position.y + r * Mathf.Sin(((i + 1) * Mathf.PI) / 180), 0));
                        pos = new Vector3(transform.position.x + maxDistanceOfHorizontal* (Mathf.Cos((i + 1) * Mathf.PI/180) / Mathf.Abs(Mathf.Cos((i + 1) * Mathf.PI / 180))), transform.position.y + r * Mathf.Sin(((i + 1) * Mathf.PI) / 180),0);
                       
                    }
                    else
                    {
                        Gizmos.DrawLine(pos, new Vector3(transform.position.x + r * Mathf.Cos(((i + 1) * Mathf.PI) / 180), transform.position.y + r * Mathf.Sin(((i + 1) * Mathf.PI) / 180), 0));
                        pos = new Vector3(transform.position.x + r * Mathf.Cos(((i + 1) * Mathf.PI) / 180), transform.position.y + r * Mathf.Sin(((i + 1) * Mathf.PI) / 180), 0);
                    }
                }
            }
        }
    }
}
