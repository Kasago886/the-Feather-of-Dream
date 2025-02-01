using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading;
using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.TextCore.Text;

public class Character : MonoBehaviour
{
    //血条
    public Scroll hpScroll;

    [Header("是否拥有初始羽")]
    public bool isDefaultFeather;
    public int defaultFeatherNum;
    public float defaultFeatherHealth = 100;
    [Header("韧性")]
    public float tenacity;
    [Header("力量")]
    public float strength;

    public bool injuryForceback;
    public float forcebackForce;
    public float forcebackDuration;
    public float forcebackTimer = 0;
    protected Transform beAttackedTrans;

    [Header("攻击替身")]
    public List<GameObject> attackBodyObjList;
    public AudioClip attackSound;

    public UnityEvent injuryEvent;
    public AudioClip injurySound;

    public UnityEvent healEvent;

    public UnityEvent deathEvent;
    public AudioClip deathSound;

    public List<Buff> buffList = new();
    public List<Feather> feathers = new();
    public List<Feather> unlockedFeathers = new();

    public bool isDead = false;

    public SpriteRenderer spriteRenderer;
    public Rigidbody2D rb;
    public AudioSource effectAudioSource;

    public Animator animator;

    // Start is called before the first frame update
    protected void Start()
    {
        spriteRenderer = GetComponentInChildren<SpriteRenderer>();
        rb = GetComponent<Rigidbody2D>();
        effectAudioSource = GameObject.Find("EffectSound").GetComponent<AudioSource>();
        animator = GetComponent<Animator>();

        //初始羽
        if (isDefaultFeather)
        {
            for (int i = 0; i < defaultFeatherNum; i++)
            {
                AddFeather(new DefautFeather(defaultFeatherHealth));
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
            AIUpdate();
            ForcebackUpdate();
        }
    }

    /// <summary>
    /// 攻击
    /// </summary>
    public virtual void OnAttack()
    {
        if (attackSound != null && attackBodyObjList.Count > 0)
        {
            effectAudioSource.PlayOneShot(attackSound);
        }

        foreach (GameObject obj in attackBodyObjList)
        {
            GameObject instance = Instantiate(obj, transform.position, Quaternion.identity);
            AttackBody attackBody = instance.GetComponent<AttackBody>();

            //方向
            if (spriteRenderer.flipX)
            {
                instance.GetComponent<AttackBody>().isleft = true;
            }

            //额外伤害
            float addDamage = 0;
            //力量是按基础伤害进行百分比增伤
            addDamage += attackBody.damage * strength / 100;

            //Debug.Log(addDamage);

            attackBody.addDamage = addDamage;

        }
    }

    /// <summary>
    /// 受伤
    /// </summary>
    /// <param name="damage"></param>
    public virtual void TakeDamage(float damage, Transform attackTrans = null)
    {
        if (!isDead)
        {
            //受伤事件
            if (unlockedFeathers.Count > 0 && damage > 0)
            {
                //音效
                if (injurySound != null)
                {
                    effectAudioSource.PlayOneShot(injurySound);
                }

                //击退
                if (injuryForceback)
                {
                    beAttackedTrans = attackTrans;
                    forcebackTimer = forcebackDuration;
                }

                injuryEvent?.Invoke();

                //减伤
                /// damage = damage * 2^(-tenacity / 100)
                /// tenacity | ratio
                ///  10        0.933
                ///  20        0.871
                ///  30        0.812
                ///  40        0.758
                ///  50        0.707
                /// 100        0.500
                /// 200        0.250
                /// 300        0.125

                //Debug.Log(damage);
                damage = damage * Mathf.Pow(2, -tenacity / 100);
            }
            //吃伤
            while (unlockedFeathers.Count > 0 && damage > 0)
            {
                Feather feather = unlockedFeathers[0];
                feather.TakeDamage(damage);

                //Debug.Log(damage);
                //Debug.Log(feather.health);

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
                OnDeath();
            }
        }
    }

    /// <summary>
    /// 死亡
    /// </summary>
    public virtual void OnDeath()
    {
        isDead = true;

        //音效
        if (deathSound != null)
        {
            effectAudioSource.PlayOneShot(deathSound);
        }

        //动画
        if (animator != null)
        {
            animator.SetBool(Consts.IsDeadAnimatorArgument, true);
        }

        deathEvent?.Invoke();
    }

    /// <summary>
    /// 受击击退
    /// </summary>
    /// <param name="attackPosition">攻击来源</param>
    public void OnForceback(Vector3 attackPosition)
    {
        Vector2 direction = transform.position - attackPosition;
        //Debug.Log(direction);
        if (direction.x > 0)
        {
            rb.AddForce(new Vector2(forcebackForce, rb.velocity.y), ForceMode2D.Impulse);
        }
        else if (direction.x < 0)
        {
            rb.AddForce(new Vector2(-forcebackForce, rb.velocity.y), ForceMode2D.Impulse);
        }
        else
        {
            float speed = -forcebackForce;
            if (spriteRenderer.flipX)
            {
                speed = -speed;
            }
            rb.AddForce(new Vector2(speed, rb.velocity.y), ForceMode2D.Impulse);
        }
    }
    public void OnForceback(Transform attackTrans)
    {
        if (attackTrans == null)
        {
            OnForceback(Vector3.zero);
        }
        else
        {
            OnForceback(attackTrans.position);
        }
    }

    /// <summary>
    /// 更新击退
    /// </summary>
    public void ForcebackUpdate()
    {
        if (forcebackTimer > 0)
        {
            OnForceback(beAttackedTrans);

            forcebackTimer -= Time.deltaTime;
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
    /// 增加羽
    /// </summary>
    /// <param name="feather"></param>
    public void AddFeather(Feather feather)
    {
        if (feather != null)
        {
            feathers.Add(feather);
            //Debug.Log(feather);
        }
    }

    /// <summary>
    /// 拔羽
    /// </summary>
    /// <param name="num"></param>
    public void UnlockFeather(int num, float time)
    {
        //Debug.Log(num.ToString()+" "+time.ToString());

        int count = 0;
        int i = feathers.Count - 1;
        while (i >= 0 && count < num)
        {
            Feather feather = feathers[i];
            unlockedFeathers.Add(feather);
            feather.lockTimer = time;

            //Debug.Log(feather);
            
            feathers.RemoveAt(i);

            ShowUnlockFeather(feather);

            count++;
            i--;
        }
    }

    /// <summary>
    /// 展示血条
    /// </summary>
    /// <param name="feather"></param>
    public virtual void ShowUnlockFeather(Feather feather)
    {
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
    /// 移除羽
    /// </summary>
    /// <param name="feather"></param>
    public void RemoveFeather(Feather feather)
    {
        if (feathers.Contains(feather))
        {
            feathers.Remove(feather);
        }
        else if (unlockedFeathers.Contains(feather))
        {
            unlockedFeathers.Remove(feather);
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
        buff.Init(this);
        buff.name = buffName;
        AddBuff(buff);
    }
    public void AddBuff(Buff buff)
    {
        buffList.Add(buff);
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
                    RemoveBuff(buff);
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
        //移除最旧的该类型buff
        for (int i = 0; i < buffList.Count; i++)
        {
            Buff buff = buffList[i];

            if (buffType.IsInstanceOfType(buff))
            {
                RemoveBuff(buff);
                break;
            }
        }
    }
    public void RemoveBuff(Buff buff)
    {
        buff.OnExit();
        buffList.Remove(buff);
    }

    /// <summary>
    /// 从列表中获取特定类型的buff
    /// </summary>
    /// <param name="type"></param>
    /// <returns></returns>
    public Buff GetBuffOfType(Type type)
    {
        foreach (Buff buff in buffList)
        {
            if (type.IsInstanceOfType(buff))
            {
                return buff;
            }
        }
        return null;
    }
    #endregion

    /// <summary>
    /// 更新AI
    /// </summary>
    public virtual void AIUpdate()
    {

    }
}
