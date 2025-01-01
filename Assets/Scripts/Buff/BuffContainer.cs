using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BuffContainer
{
    /// <summary>
    /// 存储buff名称
    /// </summary>
    public static Dictionary<string, Type> buffDictionary = new Dictionary<string, Type>
    {
        {"测试攻击", typeof(TestAttackBuff) },
        {"测试效果", typeof(TestEffectBuff) },
        {"测试装备", typeof(TestEquipmentBuff) },
        {"测试装备羽", typeof(TestEquipmentFeatherBuff) },
        {"测试拔羽", typeof(TestUnlockFeatherBuff) },
        {"测试敌人攻击", typeof(TestEnemyAttackBuff) },
        {"测试敌人效果", typeof(TestEnemyEffectBuff) }
    };


    /// <summary>
    /// 存储一套buff
    /// </summary>
    public static List<Type> testEnemyBuffable = new List<Type>
    {
        buffDictionary["测试敌人攻击"],
        buffDictionary["测试敌人效果"]
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
            Debug.LogError("There's no such a buff called "+buffName);
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

