using System.Collections;
using System.Collections.Generic;
using System.Net;
using TMPro.EditorUtilities;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class CardUI : MonoBehaviour, IPointerDownHandler, IPointerUpHandler
{
    private Transform UIScrollView;
    private GameObject self;//暂时存在于UI视图里的东西
    private bool isDrag;
    public bool isBenefit;//是否为增益效果
    private Canvas canvas;
    GameObject _object;
    public void OnPointerDown(PointerEventData eventData)
    {
        self = Instantiate(gameObject, UIScrollView);
        self.transform.SetSiblingIndex(gameObject.transform.GetSiblingIndex());
        self.GetComponent<Image>().color = new Color(255, 255, 255, 0.1f);
        isDrag = true;
        transform.SetParent(canvas.transform);
        GetComponent<Image>().raycastTarget = false;
        
    }
    public void OnPointerUp(PointerEventData eventData)
    {
        if (isBenefit)
        {
            _object = GameObject.Find("Player");
        }
        else
        {
            _object = eventData.pointerCurrentRaycast.gameObject;
        }
        if (_object != null)
        {
            Effect(_object);
        }
        self.GetComponent<Image>().color = new Color(255, 255, 255, 1);
        Destroy(gameObject);

    }
    private void Start()
    {
        if (UIScrollView == null)
        {
            UIScrollView = gameObject.transform.parent;
        }
        if (canvas == null)
        {
            canvas = GameObject.Find("Canvas").GetComponent<Canvas>();
        }
    }
    public void Update()
    {
        if (isDrag)
        {
            transform.SetAsLastSibling();
            transform.position = Input.mousePosition;
            if (Input.GetMouseButtonDown(1))
            {
                self.GetComponent<Image>().color = new Color(255, 255, 255, 1);
                self.name = gameObject.name;
                Destroy(gameObject);
            }
        }
    }
    public void Effect(GameObject _object)
    {
        Debug.Log("biu");//效果
        Destroy(self);
    }
}
