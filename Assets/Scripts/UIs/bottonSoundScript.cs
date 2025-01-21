using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

[RequireComponent(typeof(AudioSource))]
public class bottonSoundScript : MonoBehaviour, IPointerEnterHandler, IPointerDownHandler
{
    AudioSource source;
    Button button;
    bool isbutton = false;

    // Start is called before the first frame update
    void Start()
    {
        source = gameObject.GetComponent<AudioSource>();
        button = gameObject.GetComponent<Button>();

        if (button != null)
        {
            isbutton = true;
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void OnPointerEnter(PointerEventData eventData)    //鼠标移入
    {
        if (isbutton)
        {
            if (button.interactable)
            {
                source.Play();
            }
        }
    }

    public void OnPointerDown(PointerEventData eventData)    //按钮按下
    {
        if (isbutton)
        {
            if (button.interactable)
            {
                source.Play();
            }
        }
    }
}
