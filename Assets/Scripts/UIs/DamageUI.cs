using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class DamageUI : MonoBehaviour
{
    private TextMeshProUGUI text;
    private float timer;
    private RectTransform rectTransform;
    Vector2 direction=new Vector2(1,1);
    private void Awake()
    {
        text = GetComponent<TextMeshProUGUI>();
        rectTransform = GetComponent<RectTransform>();
    }
   
    private void Update()
    {
        timer += Time.deltaTime;
        if (timer < 0.2f)
        {
            rectTransform.anchoredPosition += direction.normalized * 500 * Time.deltaTime;  
            rectTransform.localScale +=  3f* Time.deltaTime*rectTransform.localScale;
        }
        if (timer >= 0.8f)
        {
            Destroy(gameObject);
        }
    }
    public void SetText(string str, Color color)
    {
        text.text = str;
        text.color = color;
    }
}
