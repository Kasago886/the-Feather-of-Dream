using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HpUi2_FollowEnemy : MonoBehaviour
{
    public GameObject HpPrefab;
    private Enemy enemy;
    private GameObject scrollView, viewPort, content;
    private ContentSizeFitter contentSizeFitter;
    private RectTransform scrollViewRectTransform, viewPortRectTransform, contentRectTransform;
    private ScrollRect scrollRect;
    private VerticalLayoutGroup layoutGroup;
    private SpriteRenderer spriteRenderer;
    private Sprite sprite;
    private Dictionary<HpUI,Feather> hpDic;
    void Start()
    {
        enemy = GetComponent<Enemy>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        if (spriteRenderer != null)
        {
            sprite = spriteRenderer.sprite;
        }
        scrollView = new GameObject(enemy.enemyName + "TemporaryEnemyHealthUi", typeof(ScrollRect));
        scrollView.AddComponent<Scroll>();
        viewPort = new GameObject("EnemyHealthViewPort", typeof(RectMask2D));
        content = new GameObject("HealthUi", typeof(RectTransform), typeof(ContentSizeFitter), typeof(LayoutGroup));
        scrollViewRectTransform = scrollView.GetComponent<RectTransform>();
        viewPortRectTransform = viewPort.GetComponent<RectTransform>();
        contentRectTransform = content.GetComponent<RectTransform>();
        scrollViewRectTransform.SetParent(GameObject.FindGameObjectWithTag("Canvas").GetComponent<RectTransform>());
        viewPortRectTransform.SetParent(scrollViewRectTransform);
        contentRectTransform.SetParent(viewPortRectTransform);
        scrollViewRectTransform.sizeDelta = new Vector2(90, 120);
        viewPortRectTransform.sizeDelta = new Vector2(90, 120);
        viewPortRectTransform.anchorMin = Vector2.zero;
        viewPortRectTransform.anchorMax = Vector2.one;
        viewPortRectTransform.pivot = new Vector2(0.5f, 0);
        contentRectTransform.anchorMax = Vector2.zero;
        contentRectTransform.anchorMin = Vector2.zero;
        contentRectTransform.pivot = Vector2.zero;
        contentRectTransform.sizeDelta = new Vector2(90, 0);
        contentSizeFitter = content.GetComponent<ContentSizeFitter>();
        layoutGroup = content.GetComponent<VerticalLayoutGroup>();
        scrollRect = scrollView.GetComponent<ScrollRect>();
        //水平任意
        contentSizeFitter.horizontalFit = ContentSizeFitter.FitMode.Unconstrained;
        //垂直调整
        contentSizeFitter.verticalFit = ContentSizeFitter.FitMode.PreferredSize;
        //物体之间间距
        layoutGroup.spacing = 10;
        //物体顶部间距
        layoutGroup.padding.top = 10;
        //物体底部间距
        layoutGroup.padding.bottom = 10;
        //分配身份
        scrollRect.content = contentRectTransform;
        scrollRect.viewport = viewPortRectTransform;
        enemy.hpScroll = scrollView.GetComponent<Scroll>();
    }

    // Update is called once per frame
    void Update()
    {
        if (sprite != null)
        {
            scrollViewRectTransform.position = Camera.main.WorldToScreenPoint(new Vector3(gameObject.transform.position.x, gameObject.transform.position.y + sprite.rect.height / 2, 0));
        }
        AddHpUi();
        Controller();
        
    }
    void AddHpUi()
    {
        if (enemy.unlockedFeathers.Count > content.transform.childCount)
        {
            GameObject hpUiObject = Instantiate(HpPrefab);
            RectTransform transform = hpUiObject.GetComponent<RectTransform>();
            transform.SetParent(contentRectTransform);
            HpUI hpUI = hpUiObject.GetComponent<HpUI>();
            hpUI.testHpMax = enemy.unlockedFeathers[content.transform.childCount - 1].maxHealth;
            hpUI.testHp = enemy.unlockedFeathers[content.transform.childCount - 1].health;
            hpUI.testTime = 1;
            hpDic.Add(hpUI, enemy.unlockedFeathers[content.transform.childCount - 1]);
        }
    }
    void Controller()
    {
        foreach (var hp in hpDic)
        {
            if (hp.Value != null)
            {
                hp.Key.testHp = hp.Value.health;
            }
            else
            {
                hpDic.Remove(hp.Key);
            }
        }
    }
}
