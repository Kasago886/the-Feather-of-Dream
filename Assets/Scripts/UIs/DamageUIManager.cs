using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DamageUIManager : MonoBehaviour
{
    public static DamageUIManager Instance { get; private set; }
    public GameObject uiShowText;
    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else if (Instance != this)
        {
            Destroy(gameObject);
        }
    }
    public void ShowText(string str,Vector2 pos,Color color)
    {
        Vector2 screenPosition = Camera.main.WorldToScreenPoint(pos);
        GameObject text= Instantiate(uiShowText, screenPosition, Quaternion.identity);
        text.transform.SetParent(GameObject.Find("Canvas").transform);
        text.GetComponent<DamageUI>().SetText(str, color);
    }
}
