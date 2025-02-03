using UnityEngine;
using UnityEngine.UI;

public class DynamicTextBackground : MonoBehaviour
{
    public Text textComponent;  // 引用Text组件
    public Image backgroundImage; // 引用Image组件
    public float padding = 10f;  // 文本与背景之间的间距

    void Start()
    {
        // 初始化时设置背景尺寸
        UpdateBackgroundSize();
    }

    public void UpdateText(string newText)
    {
        textComponent.text = newText;
        UpdateBackgroundSize();
    }

    private void UpdateBackgroundSize()
    {
        // 获取Text的尺寸
        Vector2 textSize = textComponent.GetComponent<RectTransform>().sizeDelta;

        // 根据文本大小和padding计算背景大小
        Vector2 backgroundSize = textSize + new Vector2(padding, padding);

        // 更新Image的尺寸
        backgroundImage.GetComponent<RectTransform>().sizeDelta = backgroundSize;

        // 使Image居中于Text
        backgroundImage.rectTransform.anchoredPosition = textComponent.rectTransform.anchoredPosition;
    }
}

