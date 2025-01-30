using System;
using System.Collections.Generic;
using System.Timers;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using UnityEngine.UI;
public enum targetMethod
{
    置入物体Transform,
    填入Vector3
}
[RequireComponent(typeof(EventTrigger))]
[RequireComponent(typeof(GlowControl))]
public class Card : MonoBehaviour
{
    public int id;
    new public string name;
    public int rarity;
    public string description;
    public string backGroundStory;

    //InspectorChoice
    public bool b0, b1, b2, b3, b4, b5;
    //使用对象
    public bool playerUse;
    public Material speacialMaterial;
    public bool enemyUse;
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
    //获取对象的方式
    public bool customMode;
    public targetMethod pleaseChooseOneMethod;
    public Transform targetTransform;
    public Vector3 theNumberOfTargetPosition;
    public float getObjectDistanceInX;
    public float getObjectDistanceInY;

    private bool choose, endChoose;
    private List<Collider2D> effortTarget;
    private List<Enemy> finalTarget;
    private int effortNumber;
    private RectTransform rectTransform;
    private GlowControl glowControl;
    private bool shake;
    private float shakeTimer;
    private Vector3 oriPlace, scale1;
    private Transform parent;
    private int number, stringNumber;
    private List<string> buffCaptions = new List<string>();
    private string enemyName;
    private float timer;
    private int haveChoose = -1, onlyOne;
    private List<Material> oriMaterial;
    void Start()
    {
        parent = gameObject.transform.parent;
        effortTarget = new List<Collider2D>();
        finalTarget = new List<Enemy>();
        oriMaterial = new List<Material>();
        rectTransform = GetComponent<RectTransform>();
        if (number == 0)
        {
            rectTransform = GetComponent<RectTransform>();
            scale1 = new Vector3(rectTransform.localScale.x, rectTransform.localScale.y, rectTransform.localScale.z);
            number++;
        }
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
        if (effortNumber == finalTarget.Count && finalTarget.Count > 0 && isRandom&& Input.GetMouseButtonDown(0))
        {
            endChoose = true;
        }
        if (!choose&&!endChoose)
        {
            for (int i = 0; i < finalTarget.Count; i++)
            {
                finalTarget[i].gameObject.GetComponentInChildren<SpriteRenderer>().material = oriMaterial[i];
            }
            finalTarget.Clear();
            oriMaterial.Clear();
        }
        else if (!choose && endChoose)
        {
            for (int i = 0; i < finalTarget.Count; i++)
            {
                finalTarget[i].gameObject.GetComponentInChildren<SpriteRenderer>().material = oriMaterial[i];
            }
            oriMaterial.Clear();
        }
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
        if (ConditionsOfUseCard() && enemyUse)
        {
            effects?.Invoke();
            Debug.Log(name + "已被使用");
            Captions(enemyName + "使用了" + name, false);
            EnemyEffectOnPlayer();
            EnemyEffectOnSelf();
            EnemyEffectOnSelfAndOtherEnemy();
            BothEnemyEffectOn();
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
            if (ConditionsOfUseCard())
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
                            choose = true;
                            onlyOne = 0;
                            haveChoose = -1;
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
            else
            {
                Captions("当前无法使用", true);
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
            if (ConditionsOfUseCard())
            {
                gameObject.transform.SetParent(parent);
                EffortWhenDragEnd();
                BothEffortWhenDragEnd();
            }
            else
            {
                Captions("当前无法使用", true);
            }
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
            Captions(name, true);
            Destroy(gameObject);
        }
        if (effortOnEnmey)
        {
            List<int> enemiesWhoHaveBeenEfforted = new List<int>();
            Collider2D[] enemiesThatBeenChoose = null;
            if (!customMode)
            {
                enemiesThatBeenChoose = Physics2D.OverlapAreaAll(new Vector2(Camera.main.transform.position.x - (Camera.main.orthographicSize * Camera.main.aspect), Camera.main.transform.position.y + Camera.main.orthographicSize),
                    new Vector2(Camera.main.transform.position.x + (Camera.main.orthographicSize * Camera.main.aspect), Camera.main.transform.position.y - Camera.main.orthographicSize), LayerMask.GetMask(Consts.EnemyLayer));
            }
            else
            {
                if (targetTransform != null)
                {
                    enemiesThatBeenChoose = Physics2D.OverlapAreaAll(new Vector2(targetTransform.position.x + getObjectDistanceInX / 2, targetTransform.position.y + getObjectDistanceInY / 2),
                        new Vector2(targetTransform.position.x - getObjectDistanceInX / 2, targetTransform.position.y - getObjectDistanceInY / 2), LayerMask.GetMask(Consts.EnemyLayer));
                }
                if (theNumberOfTargetPosition != null)
                {
                    enemiesThatBeenChoose = Physics2D.OverlapAreaAll(new Vector2(theNumberOfTargetPosition.x + getObjectDistanceInX / 2, theNumberOfTargetPosition.y + getObjectDistanceInY / 2),
                        new Vector2(theNumberOfTargetPosition.x - getObjectDistanceInX / 2, theNumberOfTargetPosition.y - getObjectDistanceInY / 2), LayerMask.GetMask(Consts.EnemyLayer));
                }
            }
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
                    int n = UnityEngine.Random.Range(0, theNumberOfEffortedEnemies);
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
                Captions(name, true);
                Destroy(gameObject);
            }
            else if (!effortOnMoreEnemies)
            {
                int n = UnityEngine.Random.Range(0, theNumberOfEffortedEnemies);
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
                Captions(name, true);
                Destroy(gameObject);
            }
        }

    }
    private void EffortWhenDragEnd()
    {
        if (effortOnPlayer)
        {
            if (Vector2.Distance(Camera.main.ScreenToWorldPoint(transform.position), GameObject.FindGameObjectWithTag(Consts.PlayerTag).transform.position) < minDistance)
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
                Captions(name, true);
                Destroy(gameObject);
            }
        }
        if (effortOnEnmey)
        {
            List<int> enemiesWhoHaveBeenEfforted = new List<int>();
            Collider2D[] enemiesThatBeenChoose = null;
            if (!customMode)
            {
                enemiesThatBeenChoose = Physics2D.OverlapAreaAll(new Vector2(Camera.main.transform.position.x - (Camera.main.orthographicSize * Camera.main.aspect), Camera.main.transform.position.y + Camera.main.orthographicSize),
                    new Vector2(Camera.main.transform.position.x + (Camera.main.orthographicSize * Camera.main.aspect), Camera.main.transform.position.y - Camera.main.orthographicSize), LayerMask.GetMask(Consts.EnemyLayer));
            }
            else
            {
                if (targetTransform != null)
                {
                    enemiesThatBeenChoose = Physics2D.OverlapAreaAll(new Vector2(targetTransform.position.x + getObjectDistanceInX / 2, targetTransform.position.y + getObjectDistanceInY / 2),
                        new Vector2(targetTransform.position.x - getObjectDistanceInX / 2, targetTransform.position.y - getObjectDistanceInY / 2), LayerMask.GetMask(Consts.EnemyLayer));
                }
                if (theNumberOfTargetPosition != null)
                {
                    enemiesThatBeenChoose = Physics2D.OverlapAreaAll(new Vector2(theNumberOfTargetPosition.x + getObjectDistanceInX / 2, theNumberOfTargetPosition.y + getObjectDistanceInY / 2),
                        new Vector2(theNumberOfTargetPosition.x - getObjectDistanceInX / 2, theNumberOfTargetPosition.y - getObjectDistanceInY / 2), LayerMask.GetMask(Consts.EnemyLayer));
                }
            }
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
                            int n = UnityEngine.Random.Range(0, theNumberOfEffortedEnemies);
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
                Captions(name, true);
                Destroy(gameObject);
            }
        }

    }
    private void GetWhatInCamera()
    {

        if (effortOnPlayer)
        {
            if (!customMode)
            {
                effortTarget.AddRange(Physics2D.OverlapAreaAll(new Vector2(Camera.main.transform.position.x - (Camera.main.orthographicSize * Camera.main.aspect), Camera.main.transform.position.y + Camera.main.orthographicSize),
                        new Vector2(Camera.main.transform.position.x + (Camera.main.orthographicSize * Camera.main.aspect), Camera.main.transform.position.y - Camera.main.orthographicSize), LayerMask.GetMask(Consts.PlayerLayer)));
            }
            else
            {
                if (targetTransform != null)
                {
                    effortTarget.AddRange(Physics2D.OverlapAreaAll(new Vector2(targetTransform.position.x + getObjectDistanceInX / 2, targetTransform.position.y + getObjectDistanceInY / 2),
                        new Vector2(targetTransform.position.x - getObjectDistanceInX / 2, targetTransform.position.y - getObjectDistanceInY / 2), LayerMask.GetMask(Consts.PlayerLayer)));
                }
                if (theNumberOfTargetPosition != null)
                {
                    effortTarget.AddRange(Physics2D.OverlapAreaAll(new Vector2(theNumberOfTargetPosition.x + getObjectDistanceInX / 2, theNumberOfTargetPosition.y + getObjectDistanceInY / 2),
                        new Vector2(theNumberOfTargetPosition.x - getObjectDistanceInX / 2, theNumberOfTargetPosition.y - getObjectDistanceInY / 2), LayerMask.GetMask(Consts.PlayerLayer)));
                }
            }
        }
        else if (effortOnEnmey)
        {
            if (!customMode)
            {
                effortTarget.AddRange(Physics2D.OverlapAreaAll(new Vector2(Camera.main.transform.position.x - (Camera.main.orthographicSize * Camera.main.aspect), Camera.main.transform.position.y + Camera.main.orthographicSize),
                        new Vector2(Camera.main.transform.position.x + (Camera.main.orthographicSize * Camera.main.aspect), Camera.main.transform.position.y - Camera.main.orthographicSize), LayerMask.GetMask(Consts.EnemyLayer)));
            }
            else
            {
                if (targetTransform != null)
                {
                    effortTarget.AddRange(Physics2D.OverlapAreaAll(new Vector2(targetTransform.position.x + getObjectDistanceInX / 2, targetTransform.position.y + getObjectDistanceInY / 2),
                        new Vector2(targetTransform.position.x - getObjectDistanceInX / 2, targetTransform.position.y - getObjectDistanceInY / 2), LayerMask.GetMask(Consts.EnemyLayer)));
                }
                if (theNumberOfTargetPosition != null)
                {
                    effortTarget.AddRange(Physics2D.OverlapAreaAll(new Vector2(theNumberOfTargetPosition.x + getObjectDistanceInX / 2, theNumberOfTargetPosition.y + getObjectDistanceInY / 2),
                        new Vector2(theNumberOfTargetPosition.x - getObjectDistanceInX / 2, theNumberOfTargetPosition.y - getObjectDistanceInY / 2), LayerMask.GetMask(Consts.EnemyLayer)));
                }
            }
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
        }
        if (choose && isRandom)
        {
            if (effortOnPlayer)
            {
                if (onlyOne == 0)
                {
                    Captions("请点击玩家", true);
                    onlyOne = 1;
                }
                if (Input.GetMouseButtonDown(0))
                {
                    Bounds bound = effortTarget[0].bounds;
                    if (bound.Contains(new Vector3(Camera.main.ScreenToWorldPoint(Input.mousePosition).x, Camera.main.ScreenToWorldPoint(Input.mousePosition).y, 0)))
                    {
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
                        Captions(name, true);
                        Destroy(gameObject);
                    }
                    else
                    {
                        choose = false;
                    }
                }
            }
            if (effortOnEnmey)
            {
                if (haveChoose != finalTarget.Count)
                {
                    if (effortNumber == finalTarget.Count)
                    {
                        Captions("请点击卡牌", true);
                    }
                    else
                    {
                        Captions("请选择敌人，已选择" + finalTarget.Count.ToString() + "/" + effortNumber.ToString(), true);
                    }
                    haveChoose = finalTarget.Count;
                }
                if (Input.GetMouseButtonDown(0))
                {
                    bool b = false;
                    if (effortNumber > finalTarget.Count)
                    {
                        for (int i = 0; i < effortTarget.Count; i++)
                        {
                            Bounds bound = effortTarget[i].bounds;
                            if (bound.Contains(new Vector3(Camera.main.ScreenToWorldPoint(Input.mousePosition).x, Camera.main.ScreenToWorldPoint(Input.mousePosition).y, 0)) && effortTarget[i].GetComponent<Enemy>() != null)
                            {
                                b = true;
                                whatHappenWhenBeChoosen?.Invoke();
                                finalTarget.Add(effortTarget[i].GetComponent<Enemy>());
                                SpriteRenderer spriteRenderer = effortTarget[i].gameObject.GetComponentInChildren<SpriteRenderer>();
                                oriMaterial.Add(spriteRenderer.material);
                                spriteRenderer.material = speacialMaterial;
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
        if (endChoose && effortOnEnmey)
        {
            effects?.Invoke();
            for (int i = 0; i < finalTarget.Count; i++)
            {
                finalTarget[i].gameObject.GetComponentInChildren<SpriteRenderer>().material = oriMaterial[i];
                for (int j = 0; j < buffs.Length; j++)
                {
                    finalTarget[i].AddBuff(buffs[j]);
                }
                for (int j = 0; j < buffNames.Length; j++)
                {
                    finalTarget[i].AddBuff(buffNames[j]);
                }
            }
            oriMaterial.Clear();
            Captions(name, true);
            Destroy(gameObject);
        }
    }
    private void BothEffortWhenClickRandomly()
    {
        if (effortOnPlayerAndEnemy)
        {
            effectsPlayer?.Invoke();
            Player player = GameObject.FindGameObjectWithTag(Consts.PlayerTag).GetComponent<Player>();
            for (int i = 0; i < buffsPlayer.Length; i++)
            {
                player.AddBuff(buffsPlayer[i]);
            }
            for (int j = 0; j < buffNamesPlayer.Length; j++)
            {
                player.AddBuff(buffNamesPlayer[j]);
            }
            List<int> enemiesWhoHaveBeenEfforted = new List<int>();
            Collider2D[] enemiesThatBeenChoose = null;
            if (!customMode)
            {
                enemiesThatBeenChoose = Physics2D.OverlapAreaAll(new Vector2(Camera.main.transform.position.x - (Camera.main.orthographicSize * Camera.main.aspect), Camera.main.transform.position.y + Camera.main.orthographicSize),
                    new Vector2(Camera.main.transform.position.x + (Camera.main.orthographicSize * Camera.main.aspect), Camera.main.transform.position.y - Camera.main.orthographicSize), LayerMask.GetMask(Consts.EnemyLayer));
            }
            else
            {
                if (targetTransform != null)
                {
                    enemiesThatBeenChoose = Physics2D.OverlapAreaAll(new Vector2(targetTransform.position.x + getObjectDistanceInX / 2, targetTransform.position.y + getObjectDistanceInY / 2),
                        new Vector2(targetTransform.position.x - getObjectDistanceInX / 2, targetTransform.position.y - getObjectDistanceInY / 2), LayerMask.GetMask(Consts.EnemyLayer));
                }
                if (theNumberOfTargetPosition != null)
                {
                    enemiesThatBeenChoose = Physics2D.OverlapAreaAll(new Vector2(theNumberOfTargetPosition.x + getObjectDistanceInX / 2, theNumberOfTargetPosition.y + getObjectDistanceInY / 2),
                        new Vector2(theNumberOfTargetPosition.x - getObjectDistanceInX / 2, theNumberOfTargetPosition.y - getObjectDistanceInY / 2), LayerMask.GetMask(Consts.EnemyLayer));
                }
            }
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
                    int n = UnityEngine.Random.Range(0, theNumberOfEffortedEnemies);
                    for (int j = 0; j < enemiesWhoHaveBeenEfforted.Count; j++)
                    {
                        if (n == enemiesWhoHaveBeenEfforted[j])
                        {
                            goto a;
                        }
                    }
                    Enemy enemy = enemiesWhoInCameralist[n].GetComponent<Enemy>();
                    for (int j = 0; j < buffsEnemy.Length; j++)
                    {
                        enemy.AddBuff(buffsEnemy[j]);
                    }
                    for (int j = 0; j < buffNamesEnemy.Length; j++)
                    {
                        enemy.AddBuff(buffNamesEnemy[j]);
                    }
                    effectsEnemy?.Invoke();
                    enemiesWhoHaveBeenEfforted.Add(n);
                }
                Captions(name, true);
                Destroy(gameObject);
            }
            else if (!effortOnMoreEnemies)
            {
                int n = UnityEngine.Random.Range(0, theNumberOfEffortedEnemies);
                Enemy enemy = enemiesWhoInCameralist[n].GetComponent<Enemy>();
                for (int j = 0; j < buffsEnemy.Length; j++)
                {
                    enemy.AddBuff(buffsEnemy[j]);
                }
                for (int j = 0; j < buffNamesEnemy.Length; j++)
                {
                    enemy.AddBuff(buffNamesEnemy[j]);
                }
                effectsEnemy?.Invoke();
                Captions(name, true);
                Destroy(gameObject);
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
                for (int i = 0; i < buffsPlayer.Length; i++)
                {
                    player.AddBuff(buffsPlayer[i]);
                }
                for (int j = 0; j < buffNamesPlayer.Length; j++)
                {
                    player.AddBuff(buffNamesPlayer[j]);
                }
            }
            List<int> enemiesWhoHaveBeenEfforted = new List<int>();
            Collider2D[] enemiesThatBeenChoose = null;
            if (!customMode)
            {
                enemiesThatBeenChoose = Physics2D.OverlapAreaAll(new Vector2(Camera.main.transform.position.x - (Camera.main.orthographicSize * Camera.main.aspect), Camera.main.transform.position.y + Camera.main.orthographicSize),
                    new Vector2(Camera.main.transform.position.x + (Camera.main.orthographicSize * Camera.main.aspect), Camera.main.transform.position.y - Camera.main.orthographicSize), LayerMask.GetMask(Consts.EnemyLayer));
            }
            else
            {
                if (targetTransform != null)
                {
                    enemiesThatBeenChoose = Physics2D.OverlapAreaAll(new Vector2(targetTransform.position.x + getObjectDistanceInX / 2, targetTransform.position.y + getObjectDistanceInY / 2),
                        new Vector2(targetTransform.position.x - getObjectDistanceInX / 2, targetTransform.position.y - getObjectDistanceInY / 2), LayerMask.GetMask(Consts.EnemyLayer));
                }
                if (theNumberOfTargetPosition != null)
                {
                    enemiesThatBeenChoose = Physics2D.OverlapAreaAll(new Vector2(theNumberOfTargetPosition.x + getObjectDistanceInX / 2, theNumberOfTargetPosition.y + getObjectDistanceInY / 2),
                        new Vector2(theNumberOfTargetPosition.x - getObjectDistanceInX / 2, theNumberOfTargetPosition.y - getObjectDistanceInY / 2), LayerMask.GetMask(Consts.EnemyLayer));
                }
            }
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
                            int n = UnityEngine.Random.Range(0, theNumberOfEffortedEnemies);
                            for (int j = 0; j < enemiesWhoHaveBeenEfforted.Count; j++)
                            {
                                if (n == enemiesWhoHaveBeenEfforted[j] || n == theNearestNumber)
                                {
                                    goto a;
                                }
                            }
                            Enemy enemy = enemiesWhoInCameralist[n].GetComponent<Enemy>();
                            for (int j = 0; j < buffsEnemy.Length; j++)
                            {
                                enemy.AddBuff(buffsEnemy[j]);
                            }
                            for (int j = 0; j < buffNamesEnemy.Length; j++)
                            {
                                enemy.AddBuff(buffNamesEnemy[j]);
                            }
                            effectsEnemy?.Invoke();
                            enemiesWhoHaveBeenEfforted.Add(n);
                        }
                    }
                    Enemy enemy0 = enemiesWhoInCameralist[theNearestNumber].GetComponent<Enemy>();
                    for (int j = 0; j < buffsEnemy.Length; j++)
                    {
                        enemy0.AddBuff(buffsEnemy[j]);
                    }
                    for (int j = 0; j < buffNamesEnemy.Length; j++)
                    {
                        enemy0.AddBuff(buffNamesEnemy[j]);
                    }
                    effectsEnemy?.Invoke();
                }
                Captions(name, true);
                Destroy(gameObject);
            }
        }

    }
    private void GetBothInCamera()
    {
        if (effortOnPlayerAndEnemy)
        {
            if (!customMode)
            {
                effortTarget.AddRange(Physics2D.OverlapAreaAll(new Vector2(Camera.main.transform.position.x - (Camera.main.orthographicSize * Camera.main.aspect), Camera.main.transform.position.y + Camera.main.orthographicSize),
                        new Vector2(Camera.main.transform.position.x + (Camera.main.orthographicSize * Camera.main.aspect), Camera.main.transform.position.y - Camera.main.orthographicSize), LayerMask.GetMask(Consts.EnemyLayer)));
            }
            else
            {
                if (targetTransform != null)
                {
                    effortTarget.AddRange(Physics2D.OverlapAreaAll(new Vector2(targetTransform.position.x + getObjectDistanceInX / 2, targetTransform.position.y + getObjectDistanceInY / 2),
                        new Vector2(targetTransform.position.x - getObjectDistanceInX / 2, targetTransform.position.y - getObjectDistanceInY / 2), LayerMask.GetMask(Consts.EnemyLayer)));
                }
                if (theNumberOfTargetPosition != null)
                {
                    effortTarget.AddRange(Physics2D.OverlapAreaAll(new Vector2(theNumberOfTargetPosition.x + getObjectDistanceInX / 2, theNumberOfTargetPosition.y + getObjectDistanceInY / 2),
                        new Vector2(theNumberOfTargetPosition.x - getObjectDistanceInX / 2, theNumberOfTargetPosition.y - getObjectDistanceInY / 2), LayerMask.GetMask(Consts.EnemyLayer)));
                }
            }
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
        }
        if (choose && isRandom)
        {
            if (effortOnPlayerAndEnemy)
            {
                if (haveChoose != finalTarget.Count)
                {
                    if (effortNumber == finalTarget.Count)
                    {
                        Captions("请点击卡牌", true);
                    }
                    else
                    {
                        Captions("请选择敌人，已选择" + finalTarget.Count.ToString() + "/" + effortNumber.ToString(), true);
                    }
                    haveChoose = finalTarget.Count;
                }
                if (Input.GetMouseButtonDown(0))
                {
                    bool b = false;
                    if (effortNumber > finalTarget.Count)
                    {
                        for (int i = 0; i < effortTarget.Count; i++)
                        {
                            Bounds bound = effortTarget[i].bounds;
                            if (bound.Contains(new Vector3(Camera.main.ScreenToWorldPoint(Input.mousePosition).x, Camera.main.ScreenToWorldPoint(Input.mousePosition).y, 0)) && effortTarget[i].GetComponent<Enemy>() != null)
                            {
                                if (effortTarget[i].GetComponent<Enemy>() != null)
                                {
                                    b = true;
                                    whatHappenWhenBeChoosen?.Invoke();
                                    finalTarget.Add(effortTarget[i].GetComponent<Enemy>());
                                    SpriteRenderer spriteRenderer = effortTarget[i].gameObject.GetComponentInChildren<SpriteRenderer>();
                                    oriMaterial.Add(spriteRenderer.material);
                                    spriteRenderer.material = speacialMaterial;
                                    effortTarget.RemoveAt(i);
                                }
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
        if (endChoose && effortOnPlayerAndEnemy)
        {
            effectsEnemy?.Invoke();
            for (int i = 0; i < finalTarget.Count; i++)
            {
                finalTarget[i].gameObject.GetComponentInChildren<SpriteRenderer>().material = oriMaterial[i];
                for (int j = 0; j < buffsEnemy.Length; j++)
                {
                    finalTarget[i].AddBuff(buffsEnemy[j]);
                }
                for (int j = 0; j < buffNamesEnemy.Length; j++)
                {
                    finalTarget[i].AddBuff(buffNamesEnemy[j]);
                }
            }
            effectsPlayer?.Invoke();
            Player player = GameObject.FindGameObjectWithTag(Consts.PlayerTag).GetComponent<Player>();
            for (int i = 0; i < buffsPlayer.Length; i++)
            {
                player.AddBuff(buffsPlayer[i]);
            }
            for (int j = 0; j < buffNamesPlayer.Length; j++)
            {
                player.AddBuff(buffNamesPlayer[j]);
            }
            Captions("已默认选择玩家", true);
            oriMaterial.Clear();
            Captions(name, true);
            Destroy(gameObject);
        }
    }
    private void EnemyEffectOnPlayer()
    {
        if (effortOnPlayer)
        {
            Player player = GameObject.FindGameObjectWithTag(Consts.PlayerTag).GetComponent<Player>();
            for (int i = 0; i < buffs.Length; i++)
            {
                player.AddBuff(buffs[i]);
            }
            for (int j = 0; j < buffNames.Length; j++)
            {
                player.AddBuff(buffNames[j]);
            }
        }
    }
    private void EnemyEffectOnSelf()
    {
        if (effortOnOneEnemy && effortOnEnmey)
        {
            Enemy enemy = GetComponentInParent<Enemy>();
            for (int i = 0; i < buffs.Length; i++)
            {
                enemy.AddBuff(buffs[i]);
            }
            for (int j = 0; j < buffNames.Length; j++)
            {
                enemy.AddBuff(buffNames[j]);
            }
        }
    }
    private void EnemyEffectOnSelfAndOtherEnemy()
    {
        if (effortOnMoreEnemies && effortOnEnmey)
        {
            List<int> enemiesWhoHaveBeenEfforted = new List<int>();
            Collider2D[] enemiesThatBeenChoose = null;
            if (!customMode)
            {
                enemiesThatBeenChoose = Physics2D.OverlapAreaAll(new Vector2(Camera.main.transform.position.x - (Camera.main.orthographicSize * Camera.main.aspect), Camera.main.transform.position.y + Camera.main.orthographicSize),
                    new Vector2(Camera.main.transform.position.x + (Camera.main.orthographicSize * Camera.main.aspect), Camera.main.transform.position.y - Camera.main.orthographicSize), LayerMask.GetMask(Consts.EnemyLayer));
            }
            else
            {
                if (targetTransform != null)
                {
                    enemiesThatBeenChoose = Physics2D.OverlapAreaAll(new Vector2(targetTransform.position.x + getObjectDistanceInX / 2, targetTransform.position.y + getObjectDistanceInY / 2),
                        new Vector2(targetTransform.position.x - getObjectDistanceInX / 2, targetTransform.position.y - getObjectDistanceInY / 2), LayerMask.GetMask(Consts.EnemyLayer));
                }
                if (theNumberOfTargetPosition != null)
                {
                    enemiesThatBeenChoose = Physics2D.OverlapAreaAll(new Vector2(theNumberOfTargetPosition.x + getObjectDistanceInX / 2, theNumberOfTargetPosition.y + getObjectDistanceInY / 2),
                        new Vector2(theNumberOfTargetPosition.x - getObjectDistanceInX / 2, theNumberOfTargetPosition.y - getObjectDistanceInY / 2), LayerMask.GetMask(Consts.EnemyLayer));
                }
            }
            Plane[] planes = GeometryUtility.CalculateFrustumPlanes(Camera.main);
            List<GameObject> enemiesWhoInCameralist = new List<GameObject>();
            for (int i = 0; i < enemiesThatBeenChoose.Length; i++)
            {
                Bounds bounds = enemiesThatBeenChoose[i].bounds;
                if (GeometryUtility.TestPlanesAABB(planes, bounds) && gameObject.transform.parent != enemiesThatBeenChoose[i].gameObject)
                {
                    enemiesWhoInCameralist.Add(enemiesThatBeenChoose[i].gameObject);
                }
            }
            int effortNumber = theNumberOfEffortedEnemies;
            if (theNumberOfEffortedEnemies > enemiesWhoInCameralist.Count)
            {
                effortNumber = enemiesWhoInCameralist.Count;
            }
            Enemy enemy = GetComponentInParent<Enemy>();
            for (int i = 0; i < buffs.Length; i++)
            {
                enemy.AddBuff(buffs[i]);
            }
            for (int j = 0; j < buffNames.Length; j++)
            {
                enemy.AddBuff(buffNames[j]);
            }
            List<int> effortList = new List<int>();
            for (int i = 0; i < effortNumber; i++)
            {
            a:
                int n = UnityEngine.Random.Range(0, effortNumber);
                for (int j = 0; j < effortList.Count; j++)
                {
                    if (n == effortList[j])
                    {
                        goto a;
                    }
                }
                Enemy enemy1 = enemiesWhoInCameralist[n].GetComponent<Enemy>();
                for (int i1 = 0; i1 < buffs.Length; i1++)
                {
                    enemy1.AddBuff(buffs[i1]);
                }
                for (int j = 0; j < buffNames.Length; j++)
                {
                    enemy1.AddBuff(buffNames[j]);
                }
            }
        }
    }
    private void BothEnemyEffectOn()
    {
        if (effortOnPlayerAndEnemy)
        {
            Player player = GameObject.FindGameObjectWithTag(Consts.PlayerTag).GetComponent<Player>();
            for (int i = 0; i < buffsPlayer.Length; i++)
            {
                player.AddBuff(buffsPlayer[i]);
            }
            for (int j = 0; j < buffNamesPlayer.Length; j++)
            {
                player.AddBuff(buffNamesPlayer[j]);
            }
            if (effortOnOneEnemy)
            {
                Enemy enemy = GetComponentInParent<Enemy>();
                for (int i = 0; i < buffsEnemy.Length; i++)
                {
                    enemy.AddBuff(buffsEnemy[i]);
                }
                for (int j = 0; j < buffNamesEnemy.Length; j++)
                {
                    enemy.AddBuff(buffNamesEnemy[j]);
                }
            }
            if (effortOnMoreEnemies)
            {
                List<int> enemiesWhoHaveBeenEfforted = new List<int>();
                Collider2D[] enemiesThatBeenChoose = null;
                if (!customMode)
                {
                    enemiesThatBeenChoose = Physics2D.OverlapAreaAll(new Vector2(Camera.main.transform.position.x - (Camera.main.orthographicSize * Camera.main.aspect), Camera.main.transform.position.y + Camera.main.orthographicSize),
                        new Vector2(Camera.main.transform.position.x + (Camera.main.orthographicSize * Camera.main.aspect), Camera.main.transform.position.y - Camera.main.orthographicSize), LayerMask.GetMask(Consts.EnemyLayer));
                }
                else
                {
                    if (targetTransform != null)
                    {
                        enemiesThatBeenChoose = Physics2D.OverlapAreaAll(new Vector2(targetTransform.position.x + getObjectDistanceInX / 2, targetTransform.position.y + getObjectDistanceInY / 2),
                            new Vector2(targetTransform.position.x - getObjectDistanceInX / 2, targetTransform.position.y - getObjectDistanceInY / 2), LayerMask.GetMask(Consts.EnemyLayer));
                    }
                    if (theNumberOfTargetPosition != null)
                    {
                        enemiesThatBeenChoose = Physics2D.OverlapAreaAll(new Vector2(theNumberOfTargetPosition.x + getObjectDistanceInX / 2, theNumberOfTargetPosition.y + getObjectDistanceInY / 2),
                            new Vector2(theNumberOfTargetPosition.x - getObjectDistanceInX / 2, theNumberOfTargetPosition.y - getObjectDistanceInY / 2), LayerMask.GetMask(Consts.EnemyLayer));
                    }
                }
                Plane[] planes = GeometryUtility.CalculateFrustumPlanes(Camera.main);
                List<GameObject> enemiesWhoInCameralist = new List<GameObject>();
                for (int i = 0; i < enemiesThatBeenChoose.Length; i++)
                {
                    Bounds bounds = enemiesThatBeenChoose[i].bounds;
                    if (GeometryUtility.TestPlanesAABB(planes, bounds) && gameObject.transform.parent != enemiesThatBeenChoose[i].gameObject)
                    {
                        enemiesWhoInCameralist.Add(enemiesThatBeenChoose[i].gameObject);
                    }
                }
                int effortNumber = theNumberOfEffortedEnemies;
                if (theNumberOfEffortedEnemies > enemiesWhoInCameralist.Count)
                {
                    effortNumber = enemiesWhoInCameralist.Count;
                }
                Enemy enemy = GetComponentInParent<Enemy>();
                for (int i = 0; i < buffsEnemy.Length; i++)
                {
                    enemy.AddBuff(buffsEnemy[i]);
                }
                for (int j = 0; j < buffNamesEnemy.Length; j++)
                {
                    enemy.AddBuff(buffNamesEnemy[j]);
                }
                List<int> effortList = new List<int>();
                for (int i = 0; i < effortNumber; i++)
                {
                a:
                    int n = UnityEngine.Random.Range(0, effortNumber);
                    for (int j = 0; j < effortList.Count; j++)
                    {
                        if (n == effortList[j])
                        {
                            goto a;
                        }
                    }
                    Enemy enemy1 = enemiesWhoInCameralist[n].GetComponent<Enemy>();
                    for (int i1 = 0; i1 < buffsEnemy.Length; i1++)
                    {
                        enemy1.AddBuff(buffsEnemy[i1]);
                    }
                    for (int j = 0; j < buffNamesEnemy.Length; j++)
                    {
                        enemy1.AddBuff(buffNamesEnemy[j]);
                    }
                }
            }
        }
    }
    private void Captions(string text, bool isPlayer)
    {
        GameObject textObject = new GameObject("playerText", typeof(Text));
        RectTransform rectTransform2 = textObject.GetComponent<RectTransform>();
        Text text1 = textObject.GetComponent<Text>();
        rectTransform2.SetParent(GameObject.Find("textScrollContent").GetComponent<RectTransform>());
        if (isPlayer)
        {
            text1.color = Color.blue;
        }
        else
        {
            text1.color = Color.red;
        }
        text1.text = text;
        Font defaultFont = Resources.GetBuiltinResource<Font>("LegacyRuntime.ttf");
        if (defaultFont != null)
        {
            text1.font = defaultFont;
        }
        text1.fontSize = 21;
        text1.fontStyle = FontStyle.Bold;
        text1.alignment = TextAnchor.MiddleCenter;
        // 自动调整文本框大小以适应内容
        ContentSizeFitter contentSizeFitter = textObject.AddComponent<ContentSizeFitter>();
        contentSizeFitter.horizontalFit = ContentSizeFitter.FitMode.PreferredSize;
        contentSizeFitter.verticalFit = ContentSizeFitter.FitMode.PreferredSize;
        // 确保文本框在Scroll View中正确显示
        rectTransform2.anchorMin = Vector2.zero;
        rectTransform2.anchorMax = Vector2.one;
        rectTransform2.pivot = Vector2.up;
        rectTransform2.sizeDelta = Vector2.zero;
        //rectTransform2.position = rectTransform1.position;
        Destroy(textObject, 1f);
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
    public virtual bool ConditionsOfUseCard()
    {
        return true;
    }
}

