using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ContentHeightAdjustor : MonoBehaviour
{
    RectTransform rect;
    GridLayoutGroup grid;

    // Start is called before the first frame update
    void Start()
    {
        rect = GetComponent<RectTransform>();
        grid = GetComponent<GridLayoutGroup>();
    }

    // Update is called once per frame
    void Update()
    {
        rect.sizeDelta = new Vector2(rect.sizeDelta.x, (transform.childCount/4 + 1)*100);
    }
}
