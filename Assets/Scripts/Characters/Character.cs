using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEditor;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.TextCore.Text;

public class Character : MonoBehaviour
{
    [Header("是否拥有初始羽")]
    public bool isDefaultFeather;
    public int defaultFeatherNum;
    public float defaultFeatherHealth = 100;
    [Header("韧性")]
    public float tenacity;
    [Header("力量")]
    public float strength;

    [Header("攻击替身")]
    public List<GameObject> attackBodyObjList;

    [Header("受伤事件")]
    public UnityEvent injuryEvent;
    [Header("治疗事件")]
    public UnityEvent healEvent;
    [Header("死亡事件")]
    public UnityEvent deathEvent;

    public List<Buff> buffList = new();
    public List<Feather> feathers = new();
    public List<Feather> unlockedFeathers = new();

    public bool isDead = false;

    SpriteRenderer spriteRenderer;

    // Start is called before the first frame update
    protected void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();

        //默认羽
        if (isDefaultFeather)
        {
            for (int i = 0; i < defaultFeatherNum; i++)
            {
                feathers.Add(new DefautFeather(defaultFeatherHealth));
            }
        }
    }

    // Update is called once per frame
    protected void Update()
    {
        if (!isDead)
        {
            BuffUpdate();
            FeatherUpdate();
        }
    }

    /// <summary>
    /// 攻击
    /// </summary>
    public void OnAttack()
    {
        foreach (GameObject obj in attackBodyObjList)
        {
            GameObject instance = Instantiate(obj, transform.position, Quaternion.identity);
            if (spriteRenderer.flipX)
            {
                instance.GetComponent<SpriteRenderer>().flipX = true;
                instance.GetComponent<AttackBody>().isleft = true;
            }
        }
    }

    /// <summary>
    /// 受伤
    /// </summary>
    /// <param name="damage"></param>
    public void TakeDamage(float damage)
    {
        //受伤事件
        if (unlockedFeathers.Count > 0 && damage > 0)
        {
            injuryEvent?.Invoke();
        }
        //吃伤
        while (unlockedFeathers.Count > 0 && damage > 0)
        {
            Feather feather = unlockedFeathers[0];
            feather.health -= damage;

            Debug.Log(feather.health);

            if (feather.health <= 0)
            {
                damage = -feather.health;
                unlockedFeathers.RemoveAt(0);
            }
            else
            {
                damage = 0;
            }
        }
        //检查是否失去所有羽毛
        //Debug.Log("unlock feathers:"+unlockedFeathers.Count.ToString() + "\nfeathers:" + feathers.Count.ToString());
        if (unlockedFeathers.Count <= 0 && feathers.Count <= 0)
        {
            isDead = true;

            deathEvent?.Invoke();
        }
    }

    #region attackBody
    /// <summary>
    /// 添加攻击替身
    /// </summary>
    /// <param name="obj"></param>
    public void AddAttackBody(GameObject obj)
    {
        if (obj != null)
        {
            attackBodyObjList.Add(obj);
        }
    }

    /// <summary>
    /// 去除攻击替身
    /// </summary>
    /// <param name="obj"></param>
    public void RemoveAttackBody(GameObject obj)
    {
        if (obj != null)
        {
            attackBodyObjList.Remove(obj);
        }
    }
    #endregion

    #region feather
    /// <summary>
    /// 拔羽
    /// </summary>
    /// <param name="num"></param>
    public void UnlockFeather(int num, float time)
    {
        Debug.Log(num.ToString()+" "+time.ToString());

        int count = 0;
        int i = feathers.Count - 1;
        while (i >= 0 && count < num)
        {
            Feather feather = feathers[i];
            unlockedFeathers.Add(feather);
            feather.lockTimer = time;

            //Debug.Log(feather);
            
            feathers.RemoveAt(i);

            count++;
            i--;
        }
    }

    /// <summary>
    /// 更新羽
    /// </summary>
    public void FeatherUpdate()
    {
        for (int i = unlockedFeathers.Count - 1; i >= 0; i--)
        {
            Feather unlockedFeather = unlockedFeathers[i];
            unlockedFeather.lockTimer -= Time.deltaTime;

            if (unlockedFeather.lockTimer < 0)
            {
                feathers.Add(unlockedFeather);
                unlockedFeathers.RemoveAt(i);
            }
        }
    }

    /// <summary>
    /// 拔羽10秒（调试）
    /// </summary>
    /// <param name="num"></param>
    public void DebugUnlockFeather(int num)
    {
        UnlockFeather(num, 10);
    }

    #endregion

    #region buff
    /// <summary>
    /// 添加Buff
    /// </summary>
    /// <param name="buff"></param>
    public void AddBuff(string buffName)
    {
        Buff buff = BuffContainer.GetBuffInstance(buffName) as Buff;
        buffList.Add(buff);

        buff.Init(this);
        buff.OnEnter();
    }

    /// <summary>
    /// buff更新
    /// </summary>
    public void BuffUpdate()
    {
        //此处不能用foreach，因为循环中要修改buffList
        for (int i = buffList.Count - 1; i >= 0; i--)
        {
            Buff buff = buffList[i];

            //更新
            buff.OnUpdate();
            
            if (!buff.isPermanent)
            {
                //减少倒计时
                buff.timer -= Time.deltaTime;
                if (buff.timer <= 0)
                {
                    //移除buff
                    buff.OnExit();
                    buffList.Remove(buff);
                }
            }
        }
    }

    /// <summary>
    /// 从列表中移除一个buff
    /// </summary>
    /// <param name="buffName"></param>
    public void RemoveBuff(string buffName)
    {
        Type buffType = BuffContainer.GetBuffType(buffName);
        for (int i = buffList.Count - 1; i >= 0; i--)
        {
            Buff buff = buffList[i];

            if (buffType.IsInstanceOfType(buff))
            {
                buff.OnExit();
                buffList.Remove(buff);
                break;
            }
        }
    }
    #endregion
}
