using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Noticer : MonoBehaviour
{
    public GameObject target;

    GameObject targetSpriteObj;
    SpriteRenderer spriteRenderer;
    // Start is called before the first frame update
    void Start()
    {
        Canvas canvas = GameObject.Find("Canvas").GetComponent<Canvas>();
        transform.SetParent(canvas.transform, false);
        transform.SetAsFirstSibling();

        targetSpriteObj = target;
        spriteRenderer = target.GetComponent<SpriteRenderer>();
        if (spriteRenderer == null)
        {
            for (int i = 0; i < target.transform.childCount; i++)
            {
                Transform child = target.transform.GetChild(i);

                spriteRenderer = child.GetComponent<SpriteRenderer>();
                if (spriteRenderer != null)
                {
                    targetSpriteObj = child.gameObject;
                    break;
                }
            }
        }
    }

    // Update is called once per frame
    void Update()
    {
        Vector2 screenPos;
        if (false && spriteRenderer != null )//有bug，不准备修了
        {
            Sprite sprite = spriteRenderer.sprite;
            screenPos = Camera.main.WorldToScreenPoint(
                target.transform.position + new Vector3(
                0,
                targetSpriteObj.transform.localScale.y * sprite.rect.height / sprite.pixelsPerUnit));
        }
        else
        {
            screenPos = Camera.main.WorldToScreenPoint(target.transform.position);
            //Debug.Log(target.transform.position);
            //Debug.Log(screenPos);
        }
        transform.position = screenPos;
    }
    /*
    private void OnDestroy()
    {
        Debug.Log("Noticer destroyed. Target: " + (target ? target.name : "null") 
            + "\nParent: " + transform.parent);
    }*/
}
