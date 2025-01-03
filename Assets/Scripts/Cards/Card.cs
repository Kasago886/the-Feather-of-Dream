using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
[RequireComponent(typeof(EventTrigger))]
public class Card : MonoBehaviour
{
    //作用对象
    public bool effortOnPlayer;//作用于玩家
    public bool effortOnEnmey;//作用于敌方
    public bool effortOnOneEnemy;//作用于一个敌方
    public bool effortOnMoreEnemies;//作用于多个敌方
    public int theNumberOfEffortedEnemies;//作用的敌方个数
    //作用方式
    public bool click;
    public bool dragOnCharactor;
    public float minDistance;//最小距离
    public UnityEvent whatHappenOnDrag;
    public UnityEvent whatHappenWhenMouseEnter;
    public UnityEvent whatHappenWhenMouseExit;
    public UnityEvent effects;//卡牌效果
    public Buff[] buffs;
    public string[] buffNames ;
    // Start is called before the first frame update
    void Start()
    {

    }
    // Update is called once per frame
    void Update()
    {

    }
    /// <summary>
    /// 这是敌人作用于玩家的方法，你只需要在编写敌人Ai时引用该方法即可
    /// </summary>
    public void EnemyHasEffectOnPlayer()
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
    }
    /// <summary>
    /// 当玩家鼠标进入该物体中的时候执行该函数
    /// </summary>
    public void PointerEnter()
    {

        whatHappenWhenMouseEnter?.Invoke();

    }
    /// <summary>
    /// 当玩家鼠标离开该物体中的时候执行该函数
    /// </summary>
    public void PointerExit()
    {

        whatHappenWhenMouseExit?.Invoke();

    }
    /// <summary>
    /// 当玩家点击该物体的时候执行该函数
    /// </summary>
    public void PointerClick()
    {
        if (click)
        {
            EffortWhenClick();
        }
    }
    /// <summary>
    /// 当玩家拖拽该物品的时候执行该函数
    /// </summary>
    public void Drag()
    {
        transform.position = Input.mousePosition;

        whatHappenOnDrag?.Invoke();

    }
    /// <summary>
    /// 当玩家拖拽结束的时候执行该函数
    /// </summary>
    public void EndDrag()
    {
        if (dragOnCharactor)
        {
            EffortWhenDragEnd();
        }
    }
    private void EffortWhenClick()
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
            Destroy(gameObject);
        }
        if (effortOnEnmey)
        {
            List<int> enemiesWhoHaveBeenEfforted = new List<int>();
            Collider2D[] enemiesThatBeenChoose = Physics2D.OverlapAreaAll(new Vector2(Camera.main.transform.position.x - (Camera.main.orthographicSize * Camera.main.aspect), Camera.main.transform.position.y + Camera.main.orthographicSize),
                new Vector2(Camera.main.transform.position.x + (Camera.main.orthographicSize * Camera.main.aspect), Camera.main.transform.position.y - Camera.main.orthographicSize),LayerMask.GetMask(Consts.EnemyLayer));
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
                Destroy(gameObject);
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
                Destroy(gameObject);
            }
        }
        
    }
    private void EffortWhenDragEnd()
    {
        if (effortOnPlayer)
        {
            if (Vector2.Distance(transform.position, GameObject.FindGameObjectWithTag(Consts.PlayerTag).transform.position) < minDistance)
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
                Destroy(gameObject);
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
                if (Vector2.Distance(transform.position, enemiesWhoInCameralist[i].transform.position) < minDistance)
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
                    for (int j = 0;j < buffNames.Length; j++)
                    {
                        enemy0.AddBuff(buffNames[j]);
                    }
                    effects?.Invoke();
                }
                Destroy(gameObject);
            }
        }
      
    }
    private void OnDrawGizmos()
    {
        if (dragOnCharactor&&minDistance>0)
        {
            for (int j = 0; j < 360; j++)
            {
                Gizmos.color = Color.red;
                Gizmos.DrawLine(new Vector3(transform.position.x + minDistance * Mathf.Cos((j * Mathf.PI) / 180), transform.position.y + minDistance * Mathf.Sin((j * Mathf.PI) / 180), 0), new Vector3(transform.position.x + minDistance * Mathf.Cos(((j + 1) * Mathf.PI) / 180), transform.position.y + minDistance * Mathf.Sin(((j + 1) * Mathf.PI) / 180), 0));
            }
        }
    }
}
