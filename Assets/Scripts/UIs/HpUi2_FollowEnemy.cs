using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HpUi2_FollowEnemy : MonoBehaviour
{
    private Enemy enemy;
    private GameObject scrollView, viewPort, content;
    private ContentSizeFitter contentSizeFitter;
    private RectTransform scrollViewRectTransform,viewPortRectTransform, contentRectTransform;
    private ScrollRect scrollRect;
    private VerticalLayoutGroup layoutGroup;
    private int limit;
    void Start()
    {
        enemy = GetComponent<Enemy>();
    }

    // Update is called once per frame
    void Update()
    {
        if (limit < 1)
        {
            limit++;
            scrollView=new GameObject(enemy.enemyName+"TemporaryEnemyHealthUi",typeof(ScrollRect));
            viewPort = new GameObject("EnemyHealthViewPort", typeof(RectMask2D));
            content = new GameObject("HealthUi",typeof(RectTransform),typeof(ContentSizeFitter),typeof(LayoutGroup));
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
            contentRectTransform.anchorMax= Vector2.zero;
            contentRectTransform.anchorMin= Vector2.zero;
            contentRectTransform.pivot= Vector2.zero ;
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
        }
        if(limit==1)
        {
            scrollViewRectTransform.position=Camera.main.WorldToScreenPoint(gameObject.transform.position);

        }
    }
    private Image AddHealthUi()
    {
        if(content!=null)
        {
            GameObject health = new GameObject("HealthBar", typeof(RectTransform), typeof(Image));
            RectTransform healthRectTransform=health.GetComponent<RectTransform>();
            Image image = healthRectTransform.GetComponent<Image>();
            healthRectTransform.SetParent(contentRectTransform);
            healthRectTransform.sizeDelta = new Vector2(90, 9);
            image.color = Color.red;
            image.type = Image.Type.Filled;
            image.fillMethod=Image.FillMethod.Horizontal;
            return image;
        }
        return null;    
    }
}
