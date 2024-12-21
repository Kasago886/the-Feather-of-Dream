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
    public List<Type> buffs;
    private List<BuffInterface> buffRunners;
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
        //初始化链表
        buffs = new List<Type>();
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
        BuffActive();
        ResilienceController();
    }
    /// <summary>
    /// 这个方法的作用是：实例化获得的buff,并调用其中的函数
    /// </summary>
    void BuffActive()
    {
        if (buffs.Count > pastValue)
        {
            for (int i = pastValue; i < buffs.Count; i++)
            {
                var buff = buffs[i];
                if (typeof(BuffInterface).IsAssignableFrom(buff))
                {
                    BuffInterface buffRunner=(BuffInterface)Activator.CreateInstance(buff);
                    buffRunners.Add(buffRunner);
                    buffRunner.Initialize();
                }
            }
        }
       for (int i = 0; i < buffRunners.Count; i++)
        {
            if (buffRunners[i] != null)
            {
                buffRunners[i].Update();
            }
            else
            {
                buffRunners.RemoveAt(i);
                buffs.RemoveAt(i);
            }
        }
       pastValue=buffs.Count;
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
