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
        Sprite sprite = target.GetComponentInChildren<SpriteRenderer>().sprite;
        Vector2 screenPos = Camera.main.WorldToScreenPoint(
            target.transform.position + new Vector3(
            0,
            target.transform.localScale.y * sprite.rect.height / sprite.pixelsPerUnit));
        transform.position = screenPos;
    }
}
