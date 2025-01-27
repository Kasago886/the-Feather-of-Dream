using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TextPanal : MonoBehaviour
{
    private RectTransform rectTransform;
    private bool b;
    private void Start()
    {
        rectTransform = GetComponent<RectTransform>();
    }
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.C))
        {
            b = !b;
        }
        if (b)
        {
            rectTransform.localPosition = Camera.main.WorldToScreenPoint(new Vector2(Camera.main.transform.position.x - Camera.main.orthographicSize * Camera.main.aspect / 2, Camera.main.transform.position.y + Camera.main.orthographicSize * 3 / 5));
        }
        if (!b)
        {
            rectTransform.localPosition = Camera.main.WorldToScreenPoint(new Vector2(Camera.main.transform.position.x - Camera.main.orthographicSize * Camera.main.aspect / 2, Camera.main.transform.position.y + Camera.main.orthographicSize * 7 / 5));
        }
    }
}
