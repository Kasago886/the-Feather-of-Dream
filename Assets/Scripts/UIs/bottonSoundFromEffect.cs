using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class bottonSoundFromEffect : MonoBehaviour, IPointerEnterHandler, IPointerDownHandler
{
    public AudioClip clip;
    public bool isOnPointerEnter;
    AudioSource source;
    Button button;
    bool isbutton = false;

    // Start is called before the first frame update
    void Start()
    {
        source = GameObject.Find("EffectSound").GetComponent<AudioSource>();
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
        if (isbutton && isOnPointerEnter)
        {
            if (button.interactable)
            {
                source.PlayOneShot(clip);
            }
        }
    }

    public void OnPointerDown(PointerEventData eventData)    //按钮按下
    {
        if (isbutton)
        {
            if (button.interactable)
            {
                source.PlayOneShot(clip);
            }
        }
    }
}
