using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TextPanal : MonoBehaviour
{
    public Transform content;
    private void Start()
    {
    }
    void Update()
    {
        if (content.childCount > 5)
        {
            Destroy(content.GetChild(0).gameObject);
        }
    }
}
