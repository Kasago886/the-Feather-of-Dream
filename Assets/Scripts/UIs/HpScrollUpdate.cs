using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class HpScrollUpdate : MonoBehaviour
{
    public RectTransform scrollViewContent;
    private bool isFirstEnable = true;

    private void Update()
    {
        
    }
    public void UpdateTextWithDelayRefresh(Text textComponent, string newText)
    {
        textComponent.text = newText;
        StartCoroutine(RefreshLayoutNextFrame());
    }

    IEnumerator RefreshLayoutNextFrame()
    {
        yield return null; // µÈ´ýÒ»Ö¡
        LayoutRebuilder.ForceRebuildLayoutImmediate(scrollViewContent);
        Canvas.ForceUpdateCanvases();
    }
}
