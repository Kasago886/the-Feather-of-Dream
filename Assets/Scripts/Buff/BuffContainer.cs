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
        {"测试敌人效果", typeof(TestEnemyEffectBuff) },
        {"拔羽5s", typeof(UnlockFeather5sBuff) },
        {"拔羽10s", typeof(UnlockFeather10sBuff) },
        {"拔羽15s", typeof(UnlockFeather15sBuff) },
        {"拔羽20s", typeof(UnlockFeather20sBuff) },
        {"王子之剑", typeof(PrinceSwordAttackBuff) },
        {"侍卫短剑", typeof(PrinceGuardSwordAttackBuff) },
        {"王子权柄", typeof(PrincePowerEffectBuff) },
        {"猎人预感", typeof(CrazyHunterAttackBuff) },
        {"狂暴", typeof(CrazyHunterEffectBuff) },
        {"破损引擎", typeof(TinWoodmanAttackBuff) },
        {"修补空虚", typeof(TinWoodmanEffectBuff) },
        {"被奴役者", typeof(EnslavedDwarfsAttackBuff) },
        {"麻木", typeof(EnslavedDwarfsEffectBuff) }
    };

    /*
    /// <summary>
    /// 存储一套buff
    /// </summary>
    public static List<Type> testEnemyBuffable = new List<Type>
    {
        buffDictionary["测试敌人攻击"],
        buffDictionary["测试敌人效果"]
    };
    */

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

