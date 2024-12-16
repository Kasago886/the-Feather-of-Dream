using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
[RequireComponent(typeof(PolygonCollider2D))]
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

    private float distance;
    void Update()
    {
        if (keyTrigger&&!mouseTrigger&&mode1)
        {
            distance=Vector3.Distance(GameObject.FindGameObjectWithTag(Consts.PlayerTag).transform.position, gameObject.transform.position);
            if(distance < maxDistance && Input.GetKeyDown(nameOfKey))
            {
                Interact();
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
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(keyTrigger && !mouseTrigger && mode2)
        {
            PlayerInteract.trigger.Add(gameObject);
        }
    }
    private void OnTriggerExit2D(Collider2D collision)
    {
        if (keyTrigger && !mouseTrigger && mode2)
        {
            int n = 0;
            for(int i = 0; i < PlayerInteract.trigger.Count; i++)
            {
                if (PlayerInteract.trigger[i] == gameObject)
                {
                    PlayerInteract.trigger.RemoveAt(i);
                }
            }
        }
    }
    public virtual void Interact()
    {

    }
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
                if (maxDistanceOfHorizontal == 0 && maxDistanceOfVertical == 0)
                {
                    Gizmos.DrawLine(pos, new Vector3(transform.position.x + r * Mathf.Cos(((i + 1) * Mathf.PI) / 180), transform.position.y + r * Mathf.Sin(((i + 1) * Mathf.PI) / 180), 0));
                    pos = new Vector3(transform.position.x + r * Mathf.Cos(((i + 1) * Mathf.PI) / 180), transform.position.y + r * Mathf.Sin(((i + 1) * Mathf.PI) / 180), 0);
                }
                else if (maxDistanceOfHorizontal == 0 && maxDistanceOfVertical != 0)
                {
                     if(r*Mathf.Sin((i + 1) * Mathf.PI)>=maxDistanceOfVertical)
                    {
                        Gizmos.DrawLine(pos, new Vector3(transform.position.x + r * Mathf.Cos(((i + 1) * Mathf.PI) / 180), transform.position.y + maxDistanceOfVertical * (Mathf.Sin((i + 1) * Mathf.PI) / Mathf.Abs(Mathf.Sin((i + 1) * Mathf.PI))), 0));
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
                    if(r*Mathf.Cos((i + 1) * Mathf.PI)>=maxDistanceOfHorizontal)
                    {
                        Gizmos.DrawLine(pos, new Vector3(transform.position.x + maxDistanceOfHorizontal*(Mathf.Cos((i + 1) * Mathf.PI)/Mathf.Abs(Mathf.Cos((i + 1) * Mathf.PI))), transform.position.y + r * Mathf.Sin(((i + 1) * Mathf.PI) / 180), 0));
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
