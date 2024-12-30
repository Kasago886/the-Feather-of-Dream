using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BuffContainer
{
    //存储buff名称
    public static Dictionary<string, Type> buffDictionary = new Dictionary<string, Type>
    {
        {"测试攻击", typeof(TestAttackBuff) },
        {"测试效果", typeof(TestEffectBuff) },
        {"测试装备", typeof(TestEquipmentBuff) },
        {"测试装备羽", typeof(TestEquipmentFeatherBuff) },
        {"测试拔羽", typeof(TestUnlockFeatherBuff) }
    };

    /// <summary>
    /// 获取buff新实例
    /// </summary>
    /// <param name="buffName"></param>
    /// <returns></returns>
    public static object GetBuffInstance(string buffName)
    {
        if (buffDictionary.ContainsKey(buffName)) 
        { 
            return Activator.CreateInstance(buffDictionary[buffName]); 
        }
        else 
        { 
            return null; 
        }
    }
    /// <summary>
    /// 获取buff新实例
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <returns></returns>
    public static T GetBuffInstance<T>()
    {
        return Activator.CreateInstance<T>();
    }

    /// <summary>
    /// 获取buff类型
    /// </summary>
    /// <param name="buffName"></param>
    /// <returns></returns>
    public static Type GetBuffType(string buffName)
    {
        if (buffDictionary.ContainsKey(buffName))
        {
            return buffDictionary[buffName];
        }
        else
        {
            return null;
        }
    }

}

