using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class Character : MonoBehaviour
{
    [Header("韧性")]
    public float resilience;
    [Header("最大韧性")]
    [Tooltip("如果最大韧性未填写，或者小于韧性则上面所填的初始韧性就为最大韧性")]
    public float resilienceMax;
    private float previousResilioence;
    [Header("力量")]
    public float attack;
    [Header("Buff链表")]
    public List<Buff> buffList = new();
    private int pastValue;
    [Header("受伤特效")]
    public UnityEvent injurySpecialEffect;
    [Header("回复特效")]
    public UnityEvent healSpecialEffect;
    [Header("韧性归零")]
    public UnityEvent resilienceEqualZero;
    // Start is called before the first frame update
    protected void Start()
    {
        //最大韧性赋值
        if (resilience >= resilienceMax)
        {
            resilienceMax = resilience;
        }
        previousResilioence = resilience;
    }

    // Update is called once per frame
    protected void Update()
    {
        BuffUpdate();
        ResilienceController();
    }

    /// <summary>
    /// 添加Buff
    /// </summary>
    /// <param name="buff"></param>
    public void AddBuff(string buffName)
    {
        Buff buff = BuffContainer.GetBuffInstance(buffName) as Buff;
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

    /// <summary>
    /// 这个方法是对韧性不同情况的判定，支持覆写
    /// </summary>
    protected virtual void ResilienceController()
    {
        if (resilience > resilienceMax)
        {
            resilience = resilienceMax;
        }
        if (resilience < previousResilioence)
        {
            injurySpecialEffect?.Invoke();
        }
        if(resilience > previousResilioence)
        {
            healSpecialEffect?.Invoke();
        }
        if (resilience == 0)
        {
            resilienceEqualZero?.Invoke();
        }
        previousResilioence = resilience;
    }
}
