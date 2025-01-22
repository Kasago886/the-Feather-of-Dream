using System.Collections.Generic;
using System.Reflection.Emit;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using UnityEngine.UI;
[RequireComponent(typeof(EventTrigger))]
[RequireComponent(typeof(GlowControl))]
public class Card : MonoBehaviour
{
    public int id;
    new public string name;
    public int rarity;
    public string description;
    public string backGroundStory;
    //作用对象
    public bool effortOnPlayer;//作用于玩家
    public bool effortOnEnmey;//作用于敌方
    public bool effortOnOneEnemy;//作用于一个敌方
    public bool effortOnMoreEnemies;//作用于多个敌方
    public int theNumberOfEffortedEnemies;//作用的敌方个数
    public bool effortOnPlayerAndEnemy;//作用所有对象
    //作用方式
    public bool click;
    public bool isRandom;
    public bool dragOnCharactor;
    public float minDistance;//最小距离
    public UnityEvent whatHappenOnDrag;
    public UnityEvent whatHappenWhenMouseEnter;
    public UnityEvent whatHappenWhenMouseExit;
    public UnityEvent whatHappenWhenBeChoosen;
    public UnityEvent effects;//卡牌效果
    public Buff[] buffs;
    public string[] buffNames;
    //作用玩家
    public UnityEvent whatHappenOnDragPlayer;
    public UnityEvent whatHappenWhenMouseEnterPlayer;
    public UnityEvent whatHappenWhenMouseExitPlayer;
    public UnityEvent whatHappenWhenBeChoosenPlayer;
    public UnityEvent effectsPlayer;//卡牌效果
    public Buff[] buffsPlayer;
    public string[] buffNamesPlayer;
    //作用敌方
    public UnityEvent whatHappenOnDragEnemy;
    public UnityEvent whatHappenWhenMouseEnterEnemy;
    public UnityEvent whatHappenWhenMouseExitEnemy;
    public UnityEvent whatHappenWhenBeChoosenEnemy;
    public UnityEvent effectsEnemy;//卡牌效果
    public Buff[] buffsEnemy;
    public string[] buffNamesEnemy;
    private bool choose, endChoose;
    private List<Collider2D> effortTarget;
    private List<Enemy> finalTarget;
    private int effortNumber;
    private RectTransform rectTransform,scale;
    private GlowControl glowControl;
    private GameObject captions;
    private TextMesh textMesh;
    private bool shake;
    private float shakeTimer;
    private Vector3 oriPlace;
    private Transform parent;
    private void Awake()
    {
        scale = GetComponent<RectTransform>();
    }
    // Start is called before the first frame update
    void Start()
    {
        parent = gameObject.transform.parent;
        effortTarget = new List<Collider2D>();
        finalTarget = new List<Enemy>();
        rectTransform = GetComponent<RectTransform>();
        rectTransform.rotation = Quaternion.Euler(new Vector3(0, 0, 13));
        glowControl = GetComponent<GlowControl>();
        for (int i = 0; i < gameObject.transform.childCount; i++)
        {
            if (gameObject.transform.GetChild(i).name == "name")
            {
                gameObject.transform.GetChild(i).GetComponent<Text>().text = name;
            }
            if (gameObject.transform.GetChild(i).name == "description")
            {
                gameObject.transform.GetChild(i).GetComponent<Text>().text = description;
            }
            if (gameObject.transform.GetChild(i).name == "backGroundStory")
            {
                gameObject.transform.GetChild(i).GetComponent<Text>().text = backGroundStory;
            }
        }
        oriPlace = rectTransform.localPosition;
    }
    // Update is called once per frame
    void Update()
    {
        ChooseWhenClick();
        BothChooseWhenClick();
        if (shake && Time.time % 2 <= 1)
        {
            shakeTimer += Time.deltaTime;
            transform.rotation = Quaternion.Euler(new Vector3(0, 0, 13 + Mathf.Sin(shakeTimer * 4 * Mathf.PI * 3)));
        }
    }
    /// <summary>
    /// 这是敌人作用于玩家的方法，你只需要在编写敌人Ai时引用该方法即可
    /// </summary>
    /// <param name="enemyName">
    /// 输入敌人的名字
    /// </param>
    public void EnemyHasEffectOnPlayer(string enemyName)
    {
        captions = new GameObject("captions");
        captions.transform.SetAsFirstSibling();
        captions.AddComponent<TextMesh>();
        textMesh = captions.GetComponent<TextMesh>();
        textMesh.characterSize = 0.5f;
        textMesh.anchor = TextAnchor.MiddleCenter;
        captions.transform.position = new Vector3(Camera.main.transform.position.x, Camera.main.transform.position.y + Camera.main.orthographicSize * 4 / 5, 0f);
        effects?.Invoke();
        Player player = GameObject.FindGameObjectWithTag(Consts.PlayerTag).GetComponent<Player>();
        List<string> buffCaptions = new List<string>();
        for (int i = 0; i < buffs.Length; i++)
        {
            player.AddBuff(buffs[i]);
            buffCaptions.Add(buffs[i].ToString());
        }
        for (int j = 0; j < buffNames.Length; j++)
        {
            player.AddBuff(buffNames[j]);
            buffCaptions.Add(buffNames[j]);
        }
        int stringNumber = 0;
        if (Time.time % 2 == 0)
        {
            stringNumber++;
        }
        if (stringNumber < buffCaptions.Count)
        {
            textMesh.text = enemyName + "对你使用了" + buffCaptions[stringNumber];
        }
        else
        {
            Destroy(textMesh);
        }
    }
    /// <summary>
    /// 当玩家鼠标进入该物体中的时候执行该函数
    /// </summary>
    public void PointerEnter()
    {
        if (!effortOnPlayerAndEnemy)
        {
            whatHappenWhenMouseEnter?.Invoke();
        }
        else
        {
            whatHappenWhenMouseEnterPlayer?.Invoke();
            whatHappenWhenMouseEnterEnemy?.Invoke();
        }
        rectTransform.localScale = new Vector3(rectTransform.localScale.x * 1.3f, rectTransform.localScale.y * 1.3f, rectTransform.localScale.z);
        glowControl.useGlowEffect = true;
        shake = true;
        shakeTimer = 0;
    }
    /// <summary>
    /// 当玩家鼠标离开该物体中的时候执行该函数
    /// </summary>
    public void PointerExit()
    {
        if (!effortOnPlayerAndEnemy)
        {
            whatHappenWhenMouseExit?.Invoke();
        }
        else
        {
            whatHappenWhenMouseExitPlayer?.Invoke();
            whatHappenWhenMouseExitEnemy?.Invoke();
        }
        rectTransform.localScale = new Vector3(rectTransform.localScale.x / 1.3f, rectTransform.localScale.y / 1.3f, rectTransform.localScale.z);
        glowControl.useGlowEffect = false;
        shake = false;
    }
    /// <summary>
    /// 当玩家点击该物体的时候执行该函数
    /// </summary>
    public void PointerClick()
    {
        if (click)
        {
            if (effortNumber == finalTarget.Count && finalTarget.Count > 0 && isRandom)
            {
                endChoose = true;
            }
            if (!endChoose)
            {
                if (!isRandom)
                {
                    EffortWhenClickRandomly();
                    BothEffortWhenClickRandomly();
                }
                else
                {
                    if (!choose)
                    {
                        captions = new GameObject("captions");
                        captions.transform.SetAsFirstSibling();
                        captions.AddComponent<TextMesh>();
                        textMesh = captions.GetComponent<TextMesh>();

                        textMesh.characterSize = 0.5f;
                        textMesh.anchor = TextAnchor.MiddleCenter;
                        captions.transform.position = new Vector3(Camera.main.transform.position.x, Camera.main.transform.position.y + Camera.main.orthographicSize * 4 / 5, 0f);
                        choose = true;
                        if (effortOnPlayerAndEnemy)
                        {
                            GetBothInCamera();
                        }
                        else
                        {
                            GetWhatInCamera();
                        }
                    }
                    else
                    {
                        choose = false;
                    }
                }
            }
        }
    }
    /// <summary>
    /// 当玩家拖拽该物品的时候执行该函数
    /// </summary>
    public void Drag()
    {
        if (dragOnCharactor)
        {
            transform.position = Input.mousePosition;
            gameObject.transform.SetParent(GameObject.Find("CardPanel").transform);
            if (!effortOnPlayerAndEnemy)
            {
                whatHappenOnDrag?.Invoke();
            }
            else
            {
                whatHappenOnDragPlayer?.Invoke();
                whatHappenOnDragEnemy?.Invoke();
            }
        }
    }
    /// <summary>
    /// 当玩家拖拽结束的时候执行该函数
    /// </summary>
    public void EndDrag()
    {
        if (dragOnCharactor)
        {
            gameObject.transform.SetParent(parent);
            EffortWhenDragEnd();
            rectTransform.localPosition = oriPlace;
        }
    }
    private void EffortWhenClickRandomly()
    {
        if (effortOnPlayer)
        {
            effects?.Invoke();
            Player player = GameObject.FindGameObjectWithTag(Consts.PlayerTag).GetComponent<Player>();
            for (int i = 0; i < buffs.Length; i++)
            {
                player.AddBuff(buffs[i]);
            }
            for (int j = 0; j < buffNames.Length; j++)
            {
                player.AddBuff(buffNames[j]);
            }
            gameObject.SetActive(false);
            rectTransform.localScale = new Vector3(rectTransform.localScale.x / 1.3f, rectTransform.localScale.y / 1.3f, rectTransform.localScale.z);
        }
        if (effortOnEnmey)
        {
            List<int> enemiesWhoHaveBeenEfforted = new List<int>();
            Collider2D[] enemiesThatBeenChoose = Physics2D.OverlapAreaAll(new Vector2(Camera.main.transform.position.x - (Camera.main.orthographicSize * Camera.main.aspect), Camera.main.transform.position.y + Camera.main.orthographicSize),
                new Vector2(Camera.main.transform.position.x + (Camera.main.orthographicSize * Camera.main.aspect), Camera.main.transform.position.y - Camera.main.orthographicSize), LayerMask.GetMask(Consts.EnemyLayer));
            Plane[] planes = GeometryUtility.CalculateFrustumPlanes(Camera.main);
            List<GameObject> enemiesWhoInCameralist = new List<GameObject>();
            for (int i = 0; i < enemiesThatBeenChoose.Length; i++)
            {
                Bounds bounds = enemiesThatBeenChoose[i].bounds;
                if (GeometryUtility.TestPlanesAABB(planes, bounds))
                {
                    enemiesWhoInCameralist.Add(enemiesThatBeenChoose[i].gameObject);
                }
            }
            if (theNumberOfEffortedEnemies > enemiesWhoInCameralist.Count)
            {
                theNumberOfEffortedEnemies = enemiesWhoInCameralist.Count;
            }
            if (effortOnMoreEnemies)
            {
                for (int i = 0; i < theNumberOfEffortedEnemies; i++)
                {

                a:
                    int n = Random.Range(0, theNumberOfEffortedEnemies);
                    for (int j = 0; j < enemiesWhoHaveBeenEfforted.Count; j++)
                    {
                        if (n == enemiesWhoHaveBeenEfforted[j])
                        {
                            goto a;
                        }
                    }
                    Enemy enemy = enemiesWhoInCameralist[n].GetComponent<Enemy>();
                    for (int j = 0; j < buffs.Length; j++)
                    {
                        enemy.AddBuff(buffs[j]);
                    }
                    for (int j = 0; j < buffNames.Length; j++)
                    {
                        enemy.AddBuff(buffNames[j]);
                    }
                    effects?.Invoke();
                    enemiesWhoHaveBeenEfforted.Add(n);
                }
                gameObject.SetActive(false);
                rectTransform.localScale = new Vector3(rectTransform.localScale.x / 1.3f, rectTransform.localScale.y / 1.3f, rectTransform.localScale.z);
            }
            else if (!effortOnMoreEnemies)
            {
                int n = Random.Range(0, theNumberOfEffortedEnemies);
                Enemy enemy = enemiesWhoInCameralist[n].GetComponent<Enemy>();
                for (int j = 0; j < buffs.Length; j++)
                {
                    enemy.AddBuff(buffs[j]);
                }
                for (int j = 0; j < buffNames.Length; j++)
                {
                    enemy.AddBuff(buffNames[j]);
                }
                effects?.Invoke();
                gameObject.SetActive(false);
                rectTransform.localScale = new Vector3(rectTransform.localScale.x / 1.3f, rectTransform.localScale.y / 1.3f, rectTransform.localScale.z);
            }
        }

    }
    private void EffortWhenDragEnd()
    {
        if (effortOnPlayer)
        {
            if (Vector2.Distance(Camera.main.ScreenToWorldPoint(transform.position), GameObject.FindGameObjectWithTag(Consts.PlayerTag).transform.position) < minDistance)
            {
                Debug.Log("workonplayer");
                effects?.Invoke();
                Player player = GameObject.FindGameObjectWithTag(Consts.PlayerTag).GetComponent<Player>();
                for (int i = 0; i < buffs.Length; i++)
                {
                    player.AddBuff(buffs[i]);
                }
                for (int j = 0; j < buffNames.Length; j++)
                {
                    player.AddBuff(buffNames[j]);
                }
                gameObject.SetActive(false);
                rectTransform.localScale = new Vector3(rectTransform.localScale.x / 1.3f, rectTransform.localScale.y / 1.3f, rectTransform.localScale.z);
            }
        }
        if (effortOnEnmey)
        {
            List<int> enemiesWhoHaveBeenEfforted = new List<int>();
            Collider2D[] enemiesThatBeenChoose = Physics2D.OverlapAreaAll(new Vector2(Camera.main.transform.position.x - (Camera.main.orthographicSize * Camera.main.aspect), Camera.main.transform.position.y + Camera.main.orthographicSize),
                new Vector2(Camera.main.transform.position.x + (Camera.main.orthographicSize * Camera.main.aspect), Camera.main.transform.position.y - Camera.main.orthographicSize), LayerMask.GetMask(Consts.EnemyLayer));
            Plane[] planes = GeometryUtility.CalculateFrustumPlanes(Camera.main);
            List<GameObject> enemiesWhoInCameralist = new List<GameObject>();
            for (int i = 0; i < enemiesThatBeenChoose.Length; i++)
            {
                Bounds bounds = enemiesThatBeenChoose[i].bounds;
                if (GeometryUtility.TestPlanesAABB(planes, bounds))
                {
                    enemiesWhoInCameralist.Add(enemiesThatBeenChoose[i].gameObject);
                }
            }
            if (theNumberOfEffortedEnemies > enemiesWhoInCameralist.Count)
            {
                theNumberOfEffortedEnemies = enemiesWhoInCameralist.Count;
            }
            bool work = false; ;
            for (int i = 0; i < enemiesWhoInCameralist.Count; i++)
            {
                if (Vector2.Distance(Camera.main.ScreenToWorldPoint(transform.position), enemiesWhoInCameralist[i].transform.position) < minDistance)
                {
                    work = true;
                    break;
                }
            }
            if (work)
            {
                int theNearestNumber = -1;
                for (int i = 0; i < enemiesWhoInCameralist.Count; i++)
                {
                    if (theNearestNumber != -1)
                    {
                        if (Vector2.Distance(transform.position, enemiesWhoInCameralist[i].transform.position) < Vector2.Distance(transform.position, enemiesWhoInCameralist[theNearestNumber].transform.position))
                        {
                            theNearestNumber = i;
                        }
                    }
                    else if (theNearestNumber == -1)
                    {
                        theNearestNumber = i;
                    }
                }
                if (theNearestNumber != -1)
                {
                    if (effortOnMoreEnemies)
                    {
                        for (int i = 0; i < theNumberOfEffortedEnemies - 1; i++)
                        {

                        a:
                            int n = Random.Range(0, theNumberOfEffortedEnemies);
                            for (int j = 0; j < enemiesWhoHaveBeenEfforted.Count; j++)
                            {
                                if (n == enemiesWhoHaveBeenEfforted[j] || n == theNearestNumber)
                                {
                                    goto a;
                                }
                            }
                            Enemy enemy = enemiesWhoInCameralist[n].GetComponent<Enemy>();
                            for (int j = 0; j < buffs.Length; j++)
                            {
                                enemy.AddBuff(buffs[j]);
                            }
                            for (int j = 0; j < buffNames.Length; j++)
                            {
                                enemy.AddBuff(buffNames[j]);
                            }
                            effects?.Invoke();
                            enemiesWhoHaveBeenEfforted.Add(n);
                        }
                    }
                    Enemy enemy0 = enemiesWhoInCameralist[theNearestNumber].GetComponent<Enemy>();
                    for (int j = 0; j < buffs.Length; j++)
                    {
                        enemy0.AddBuff(buffs[j]);
                    }
                    for (int j = 0; j < buffNames.Length; j++)
                    {
                        enemy0.AddBuff(buffNames[j]);
                    }
                    effects?.Invoke();
                }
                gameObject.SetActive(false);
                rectTransform.localScale = new Vector3(rectTransform.localScale.x / 1.3f, rectTransform.localScale.y / 1.3f, rectTransform.localScale.z);
            }
        }

    }
    private void GetWhatInCamera()
    {

        if (effortOnPlayer)
        {
            effortTarget.AddRange(Physics2D.OverlapAreaAll(new Vector2(Camera.main.transform.position.x - (Camera.main.orthographicSize * Camera.main.aspect), Camera.main.transform.position.y + Camera.main.orthographicSize),
                    new Vector2(Camera.main.transform.position.x + (Camera.main.orthographicSize * Camera.main.aspect), Camera.main.transform.position.y - Camera.main.orthographicSize), LayerMask.GetMask(Consts.PlayerLayer)));
        }
        else if (effortOnEnmey)
        {
            effortTarget.AddRange(Physics2D.OverlapAreaAll(new Vector2(Camera.main.transform.position.x - (Camera.main.orthographicSize * Camera.main.aspect), Camera.main.transform.position.y + Camera.main.orthographicSize),
                    new Vector2(Camera.main.transform.position.x + (Camera.main.orthographicSize * Camera.main.aspect), Camera.main.transform.position.y - Camera.main.orthographicSize), LayerMask.GetMask(Consts.EnemyLayer)));
            effortNumber = 1;
            if (effortOnOneEnemy)
            {
                effortNumber = 1;
            }
            else if (effortOnMoreEnemies)
            {
                if (theNumberOfEffortedEnemies < effortTarget.Count)
                {
                    effortNumber = theNumberOfEffortedEnemies;
                }
                else
                {
                    effortNumber = effortTarget.Count;
                }
            }
        }
    }
    private void ChooseWhenClick()
    {
        if (!choose)
        {
            effortTarget.Clear();
            if (captions != null)
            {
                Destroy(captions);
            }
        }
        if (choose && isRandom)
        {
            if (effortOnPlayer)
            {
                if (textMesh != null)
                {
                    textMesh.text = "请点击玩家";
                }
                if (captions.transform.position != new Vector3(Camera.main.transform.position.x, Camera.main.transform.position.y + Camera.main.orthographicSize * 4 / 5, 0f))
                {
                    captions.transform.position = new Vector3(Camera.main.transform.position.x, Camera.main.transform.position.y + Camera.main.orthographicSize * 4 / 5, 0f);
                }
                if (Input.GetMouseButtonDown(0))
                {
                    Bounds bound = effortTarget[0].bounds;
                    if (bound.Contains(new Vector3(Camera.main.ScreenToWorldPoint(Input.mousePosition).x, Camera.main.ScreenToWorldPoint(Input.mousePosition).y, 0)))
                    {
                        Debug.Log("workOnPlayer");
                        whatHappenWhenBeChoosen?.Invoke();
                        effects?.Invoke();
                        Player player0 = effortTarget[0].GetComponent<Player>();
                        for (int i = 0; i < buffs.Length; i++)
                        {
                            player0.AddBuff(buffs[i]);
                        }
                        for (int j = 0; j < buffNames.Length; j++)
                        {
                            player0.AddBuff(buffNames[j]);
                        }
                        Destroy(captions);
                        gameObject.SetActive(false);
                        rectTransform.localScale = new Vector3(rectTransform.localScale.x / 1.3f, rectTransform.localScale.y / 1.3f, rectTransform.localScale.z);
                    }
                    else
                    {
                        choose = false;
                    }
                }
            }
            if (effortOnEnmey)
            {
                if (textMesh != null)
                {
                    if (effortNumber == finalTarget.Count)
                    {
                        textMesh.text = "请点击卡牌";
                    }
                    else
                    {
                        textMesh.text = "请选择敌人，已选择" + finalTarget.Count.ToString() + "/" + effortNumber.ToString();
                    }
                }
                if (Input.GetMouseButtonDown(0))
                {
                    bool b = false;
                    if (effortNumber > finalTarget.Count)
                    {
                        for (int i = 0; i < effortTarget.Count; i++)
                        {
                            Bounds bound = effortTarget[i].bounds;
                            if (bound.Contains(new Vector3(Camera.main.ScreenToWorldPoint(Input.mousePosition).x, Camera.main.ScreenToWorldPoint(Input.mousePosition).y, 0)))
                            {
                                b = true;
                                whatHappenWhenBeChoosen?.Invoke();
                                finalTarget.Add(effortTarget[i].GetComponent<Enemy>());
                                effortTarget.RemoveAt(i);
                                break;
                            }
                        }
                    }
                    if (!b)
                    {
                        choose = false;
                    }
                }

            }
        }
        if (endChoose)
        {
            effects?.Invoke();
            for (int i = 0; i < finalTarget.Count; i++)
            {

                for (int j = 0; j < buffs.Length; j++)
                {
                    finalTarget[i].AddBuff(buffs[j]);
                }
                for (int j = 0; j < buffNames.Length; j++)
                {
                    finalTarget[i].AddBuff(buffNames[j]);
                }
            }
            Debug.Log("IGet");
            Destroy(captions);
            gameObject.SetActive(false);
            rectTransform.localScale = new Vector3(rectTransform.localScale.x / 1.3f, rectTransform.localScale.y / 1.3f, rectTransform.localScale.z);
        }
    }
    private void BothEffortWhenClickRandomly()
    {
        if (effortOnPlayerAndEnemy)
        {
            effectsPlayer?.Invoke();
            Player player = GameObject.FindGameObjectWithTag(Consts.PlayerTag).GetComponent<Player>();
            for (int i = 0; i < buffs.Length; i++)
            {
                player.AddBuff(buffsPlayer[i]);
            }
            for (int j = 0; j < buffNames.Length; j++)
            {
                player.AddBuff(buffNamesPlayer[j]);
            }
            List<int> enemiesWhoHaveBeenEfforted = new List<int>();
            Collider2D[] enemiesThatBeenChoose = Physics2D.OverlapAreaAll(new Vector2(Camera.main.transform.position.x - (Camera.main.orthographicSize * Camera.main.aspect), Camera.main.transform.position.y + Camera.main.orthographicSize),
                new Vector2(Camera.main.transform.position.x + (Camera.main.orthographicSize * Camera.main.aspect), Camera.main.transform.position.y - Camera.main.orthographicSize), LayerMask.GetMask(Consts.EnemyLayer));
            Plane[] planes = GeometryUtility.CalculateFrustumPlanes(Camera.main);
            List<GameObject> enemiesWhoInCameralist = new List<GameObject>();
            for (int i = 0; i < enemiesThatBeenChoose.Length; i++)
            {
                Bounds bounds = enemiesThatBeenChoose[i].bounds;
                if (GeometryUtility.TestPlanesAABB(planes, bounds))
                {
                    enemiesWhoInCameralist.Add(enemiesThatBeenChoose[i].gameObject);
                }
            }
            if (theNumberOfEffortedEnemies > enemiesWhoInCameralist.Count)
            {
                theNumberOfEffortedEnemies = enemiesWhoInCameralist.Count;
            }
            if (effortOnMoreEnemies)
            {
                for (int i = 0; i < theNumberOfEffortedEnemies; i++)
                {

                a:
                    int n = Random.Range(0, theNumberOfEffortedEnemies);
                    for (int j = 0; j < enemiesWhoHaveBeenEfforted.Count; j++)
                    {
                        if (n == enemiesWhoHaveBeenEfforted[j])
                        {
                            goto a;
                        }
                    }
                    Enemy enemy = enemiesWhoInCameralist[n].GetComponent<Enemy>();
                    for (int j = 0; j < buffs.Length; j++)
                    {
                        enemy.AddBuff(buffsEnemy[j]);
                    }
                    for (int j = 0; j < buffNames.Length; j++)
                    {
                        enemy.AddBuff(buffNamesEnemy[j]);
                    }
                    effectsEnemy?.Invoke();
                    enemiesWhoHaveBeenEfforted.Add(n);
                }
                gameObject.SetActive(false);
                rectTransform.localScale = new Vector3(rectTransform.localScale.x / 1.3f, rectTransform.localScale.y / 1.3f, rectTransform.localScale.z);
            }
            else if (!effortOnMoreEnemies)
            {
                int n = Random.Range(0, theNumberOfEffortedEnemies);
                Enemy enemy = enemiesWhoInCameralist[n].GetComponent<Enemy>();
                for (int j = 0; j < buffs.Length; j++)
                {
                    enemy.AddBuff(buffsEnemy[j]);
                }
                for (int j = 0; j < buffNames.Length; j++)
                {
                    enemy.AddBuff(buffNamesEnemy[j]);
                }
                effectsEnemy?.Invoke();
                gameObject.SetActive(false);
                rectTransform.localScale = new Vector3(rectTransform.localScale.x / 1.3f, rectTransform.localScale.y / 1.3f, rectTransform.localScale.z);
            }
        }
    }
    private void BothEffortWhenDragEnd()
    {
        if (effortOnPlayerAndEnemy)
        {
            if (Vector2.Distance(Camera.main.ScreenToWorldPoint(transform.position), GameObject.FindGameObjectWithTag(Consts.PlayerTag).transform.position) < minDistance)
            {
                effectsPlayer?.Invoke();
                Player player = GameObject.FindGameObjectWithTag(Consts.PlayerTag).GetComponent<Player>();
                for (int i = 0; i < buffs.Length; i++)
                {
                    player.AddBuff(buffsPlayer[i]);
                }
                for (int j = 0; j < buffNames.Length; j++)
                {
                    player.AddBuff(buffNamesPlayer[j]);
                }              
            }
            List<int> enemiesWhoHaveBeenEfforted = new List<int>();
            Collider2D[] enemiesThatBeenChoose = Physics2D.OverlapAreaAll(new Vector2(Camera.main.transform.position.x - (Camera.main.orthographicSize * Camera.main.aspect), Camera.main.transform.position.y + Camera.main.orthographicSize),
                new Vector2(Camera.main.transform.position.x + (Camera.main.orthographicSize * Camera.main.aspect), Camera.main.transform.position.y - Camera.main.orthographicSize), LayerMask.GetMask(Consts.EnemyLayer));
            Plane[] planes = GeometryUtility.CalculateFrustumPlanes(Camera.main);
            List<GameObject> enemiesWhoInCameralist = new List<GameObject>();
            for (int i = 0; i < enemiesThatBeenChoose.Length; i++)
            {
                Bounds bounds = enemiesThatBeenChoose[i].bounds;
                if (GeometryUtility.TestPlanesAABB(planes, bounds))
                {
                    enemiesWhoInCameralist.Add(enemiesThatBeenChoose[i].gameObject);
                }
            }
            if (theNumberOfEffortedEnemies > enemiesWhoInCameralist.Count)
            {
                theNumberOfEffortedEnemies = enemiesWhoInCameralist.Count;
            }
            bool work = false; ;
            for (int i = 0; i < enemiesWhoInCameralist.Count; i++)
            {
                if (Vector2.Distance(Camera.main.ScreenToWorldPoint(transform.position), enemiesWhoInCameralist[i].transform.position) < minDistance)
                {
                    work = true;
                    break;
                }
            }
            if (work)
            {
                int theNearestNumber = -1;
                for (int i = 0; i < enemiesWhoInCameralist.Count; i++)
                {
                    if (theNearestNumber != -1)
                    {
                        if (Vector2.Distance(transform.position, enemiesWhoInCameralist[i].transform.position) < Vector2.Distance(transform.position, enemiesWhoInCameralist[theNearestNumber].transform.position))
                        {
                            theNearestNumber = i;
                        }
                    }
                    else if (theNearestNumber == -1)
                    {
                        theNearestNumber = i;
                    }
                }
                if (theNearestNumber != -1)
                {
                    if (effortOnMoreEnemies)
                    {
                        for (int i = 0; i < theNumberOfEffortedEnemies - 1; i++)
                        {

                        a:
                            int n = Random.Range(0, theNumberOfEffortedEnemies);
                            for (int j = 0; j < enemiesWhoHaveBeenEfforted.Count; j++)
                            {
                                if (n == enemiesWhoHaveBeenEfforted[j] || n == theNearestNumber)
                                {
                                    goto a;
                                }
                            }
                            Enemy enemy = enemiesWhoInCameralist[n].GetComponent<Enemy>();
                            for (int j = 0; j < buffs.Length; j++)
                            {
                                enemy.AddBuff(buffsEnemy[j]);
                            }
                            for (int j = 0; j < buffNames.Length; j++)
                            {
                                enemy.AddBuff(buffNamesEnemy[j]);
                            }
                            effectsEnemy?.Invoke();
                            enemiesWhoHaveBeenEfforted.Add(n);
                        }
                    }
                    Enemy enemy0 = enemiesWhoInCameralist[theNearestNumber].GetComponent<Enemy>();
                    for (int j = 0; j < buffs.Length; j++)
                    {
                        enemy0.AddBuff(buffsEnemy[j]);
                    }
                    for (int j = 0; j < buffNames.Length; j++)
                    {
                        enemy0.AddBuff(buffNamesEnemy[j]);
                    }
                    effectsEnemy?.Invoke();
                }
                gameObject.SetActive(false);
                rectTransform.localScale = new Vector3(rectTransform.localScale.x / 1.3f, rectTransform.localScale.y / 1.3f, rectTransform.localScale.z);
            }
        }

    }
    private void GetBothInCamera()
    {
        if (effortOnPlayerAndEnemy)
        {
            effortTarget.AddRange(Physics2D.OverlapAreaAll(new Vector2(Camera.main.transform.position.x - (Camera.main.orthographicSize * Camera.main.aspect), Camera.main.transform.position.y + Camera.main.orthographicSize),
                    new Vector2(Camera.main.transform.position.x + (Camera.main.orthographicSize * Camera.main.aspect), Camera.main.transform.position.y - Camera.main.orthographicSize), LayerMask.GetMask(Consts.PlayerLayer)));
            effortTarget.AddRange(Physics2D.OverlapAreaAll(new Vector2(Camera.main.transform.position.x - (Camera.main.orthographicSize * Camera.main.aspect), Camera.main.transform.position.y + Camera.main.orthographicSize),
                    new Vector2(Camera.main.transform.position.x + (Camera.main.orthographicSize * Camera.main.aspect), Camera.main.transform.position.y - Camera.main.orthographicSize), LayerMask.GetMask(Consts.EnemyLayer)));
            effortNumber = 1;
            if (effortOnOneEnemy)
            {
                effortNumber = 1;
            }
            else if (effortOnMoreEnemies)
            {
                if (theNumberOfEffortedEnemies < effortTarget.Count)
                {
                    effortNumber = theNumberOfEffortedEnemies;
                }
                else
                {
                    effortNumber = effortTarget.Count;
                }
            }
        }
    }
    private void BothChooseWhenClick()
    {
        if (!choose)
        {
            effortTarget.Clear();
            if (captions != null)
            {
                Destroy(captions);
            }
        }
        if (choose && isRandom)
        {
            if (effortOnPlayerAndEnemy)
            {               
                if (textMesh != null)
                {
                    if (effortNumber == finalTarget.Count)
                    {
                        textMesh.text = "请点击卡牌";
                    }
                    else
                    {
                        textMesh.text = "请选择敌人，已选择" + finalTarget.Count.ToString() + "/" + effortNumber.ToString();
                    }
                }
                if (Input.GetMouseButtonDown(0))
                {
                    bool b = false;
                    if (effortNumber > finalTarget.Count)
                    {
                        for (int i = 0; i < effortTarget.Count; i++)
                        {
                            Bounds bound = effortTarget[i].bounds;
                            if (bound.Contains(new Vector3(Camera.main.ScreenToWorldPoint(Input.mousePosition).x, Camera.main.ScreenToWorldPoint(Input.mousePosition).y, 0)))
                            {
                                b = true;
                                whatHappenWhenBeChoosen?.Invoke();
                                finalTarget.Add(effortTarget[i].GetComponent<Enemy>());
                                effortTarget.RemoveAt(i);
                                break;
                            }
                        }
                    }
                    if (!b)
                    {
                        choose = false;
                    }
                }

            }
        }
        if (endChoose)
        {
            effectsEnemy?.Invoke();
            for (int i = 0; i < finalTarget.Count; i++)
            {

                for (int j = 0; j < buffs.Length; j++)
                {
                    finalTarget[i].AddBuff(buffsEnemy[j]);
                }
                for (int j = 0; j < buffNames.Length; j++)
                {
                    finalTarget[i].AddBuff(buffNamesEnemy[j]);
                }
            }
            effectsPlayer?.Invoke();
            Player player = GameObject.FindGameObjectWithTag(Consts.PlayerTag).GetComponent<Player>();
            for (int i = 0; i < buffs.Length; i++)
            {
                player.AddBuff(buffsPlayer[i]);
            }
            for (int j = 0; j < buffNames.Length; j++)
            {
                player.AddBuff(buffNamesPlayer[j]);
            }
            if (textMesh != null)
            {
                textMesh.text = "已默认选择玩家";
            }
            Destroy(captions,1f);
            gameObject.SetActive(false);
            rectTransform.localScale = new Vector3(rectTransform.localScale.x / 1.3f, rectTransform.localScale.y / 1.3f, rectTransform.localScale.z);
        }
    }
    private void OnDrawGizmos()
    {
        if (dragOnCharactor && minDistance > 0)
        {
            for (int j = 0; j < 360; j++)
            {
                Gizmos.color = Color.red;
                Gizmos.DrawLine(new Vector3(Camera.main.ScreenToWorldPoint(transform.position).x + minDistance * Mathf.Cos((j * Mathf.PI) / 180), Camera.main.ScreenToWorldPoint(transform.position).y + minDistance * Mathf.Sin((j * Mathf.PI) / 180), 0), new Vector3(Camera.main.ScreenToWorldPoint(transform.position).x + minDistance * Mathf.Cos(((j + 1) * Mathf.PI) / 180), Camera.main.ScreenToWorldPoint(transform.position).y + minDistance * Mathf.Sin(((j + 1) * Mathf.PI) / 180), 0));
                Gizmos.DrawLine(Camera.main.WorldToScreenPoint(new Vector3(Camera.main.ScreenToWorldPoint(transform.position).x + minDistance * Mathf.Cos((j * Mathf.PI) / 180), Camera.main.ScreenToWorldPoint(transform.position).y + minDistance * Mathf.Sin((j * Mathf.PI) / 180), 0)), Camera.main.WorldToScreenPoint(new Vector3(Camera.main.ScreenToWorldPoint(transform.position).x + minDistance * Mathf.Cos(((j + 1) * Mathf.PI) / 180), Camera.main.ScreenToWorldPoint(transform.position).y + minDistance * Mathf.Sin(((j + 1) * Mathf.PI) / 180), 0)));
            }
            Gizmos.DrawLine(new Vector3(Camera.main.ScreenToWorldPoint(transform.position).x - minDistance / 4, Camera.main.ScreenToWorldPoint(transform.position).y, 0), new Vector3(Camera.main.ScreenToWorldPoint(transform.position).x + minDistance / 4, Camera.main.ScreenToWorldPoint(transform.position).y, 0));
            Gizmos.DrawLine(new Vector3(Camera.main.ScreenToWorldPoint(transform.position).x, Camera.main.ScreenToWorldPoint(transform.position).y - minDistance / 4, 0), new Vector3(Camera.main.ScreenToWorldPoint(transform.position).x, Camera.main.ScreenToWorldPoint(transform.position).y + minDistance / 4, 0));
        }
    }
    private void OnDisable()
    {
        rectTransform.localScale=scale.localScale;  
        if (glowControl != null)
        {
            glowControl.useGlowEffect = false;
        }
        if (effortTarget != null)
        {
            effortTarget.Clear();
        }
        if (finalTarget != null)
        {
            finalTarget.Clear();
        }
        effortNumber = 0;
        choose = false;
        endChoose = false;
        shake = false;
    }
}
