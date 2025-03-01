using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DamageUIManager : MonoBehaviour
{
    public static void ShowText(string str,Vector2 pos,Color color)
    {
        GameObject uiShowText = Resources.Load<GameObject>("DamageUI");

        Vector2 screenPosition = Camera.main.WorldToScreenPoint(pos);
        GameObject text= Instantiate(uiShowText, screenPosition, Quaternion.identity);
        text.transform.SetParent(GameObject.Find("Canvas").transform);
        text.GetComponent<DamageUI>().SetText(str, color);
    }
}
