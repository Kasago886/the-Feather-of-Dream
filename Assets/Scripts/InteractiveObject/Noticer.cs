using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Noticer : MonoBehaviour
{
    public GameObject target;
    // Start is called before the first frame update
    void Start()
    {
        transform.SetParent(GameObject.FindAnyObjectByType<Canvas>().transform, false);
        transform.SetAsFirstSibling();
    }

    // Update is called once per frame
    void Update()
    {
        GameObject targetSpriteObj = target;

        SpriteRenderer spriteRenderer = target.GetComponent<SpriteRenderer>();
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

        Vector2 screenPos;
        if (spriteRenderer != null)
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
            Debug.Log(target.transform.position);
            Debug.Log(screenPos);
        }
        transform.position = screenPos;
    }
}
