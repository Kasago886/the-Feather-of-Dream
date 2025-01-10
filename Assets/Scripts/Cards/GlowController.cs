using UnityEngine;
using UnityEngine.UI;

public class GlowControl : MonoBehaviour
{
    private Image targetImage;
    [Header("请将Meteria\"GB1\"置入其中")]
    public Material glowMaterial;
    [Header("这个bool变量无需修改")]
    public bool useGlowEffect;
    [Header("发光边框颜色")]
    public Color glowColor = new Color(1, 1, 1, 1);
    [Header("发光边框宽度")]
    public float glowWidth = 0.05f;
    [Header("发光边框锐度")]
    public float edgeSharpness = 1.0f;
    void Start()
    {
        targetImage = GetComponent<Image>();
        if (targetImage == null)
        {
            return;
        }
        targetImage.material = glowMaterial;
        ApplyGlowEffect();
    }

    void Update()
    {
        ApplyGlowEffect();
    }

    void ApplyGlowEffect()
    {
        if (useGlowEffect && glowMaterial != null)
        {
            targetImage.material = glowMaterial;

            glowMaterial.SetColor("_GlowColor", glowColor);
            glowMaterial.SetFloat("_GlowWidth", glowWidth);
            glowMaterial.SetFloat("_EdgeSharpness", edgeSharpness);
        }
        else
        {
            targetImage.material = null;
        }
    }
}


