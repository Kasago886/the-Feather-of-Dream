using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Character : MonoBehaviour
{
    [Header("韧性")]
    public float resilience;
    [Header("力量")]
    public float attack;
    [Header("Buff链表")]
    public List<Type> buffs;
    private List<BuffInterface> buffRunners;
    private int pastValue;
    // Start is called before the first frame update
    void Start()
    {
        //初始化链表
        buffs = new List<Type>();
    }

    // Update is called once per frame
    void Update()
    {
        BuffActive();
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
}
