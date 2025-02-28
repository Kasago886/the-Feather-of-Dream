using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class DamageUI : MonoBehaviour
{
    private TextMeshProUGUI text;
    private void Awake()
    {
        text = GetComponent<TextMeshProUGUI>();
    }
    private void Start()
    {
        Destroy(gameObject, 0.6f);
    }
    public void SetText(string str, Color color)
    {
        text.text = str;
        text.color = color;
    }
}
