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
        {"修补空虚", typeof(TinWoodmanEffectBuff) },//立即扣除2.5%单个解锁羽的血量，每2秒增加1层力量，buff持续6秒，6秒内如若单个解锁羽受到超过20点生命值，则全体单个解锁羽一共扣除40点生命值
        {"被奴役者", typeof(EnslavedDwarfsAttackBuff) },
        {"麻木", typeof(EnslavedDwarfsEffectBuff) },//在有羽解锁的条件下，立即回复总血量的5%，提高总血量5%的血量上限，玩家和敌人通用
        {"矮人短剑", typeof(DwarfsAttackBuff) },
        {"合力", typeof(DwarfsEffectBuff) },
        {"利爪", typeof(TheMisunderstoodWerewolfBuff) },
        {"伤痕", typeof(Trauma) },//在有羽解锁的条件下，受到不低于1点伤害后扣除1滴血，并有1/3的概率解除该buff，玩家和敌人通用
        {"惊惶", typeof(Terrified) },//在有羽解锁的条件下，立即受到1点伤害，并回复1滴血，玩家和敌人通用
        {"忧郁",typeof(Depressed) }//使玩家的获得卡牌的时间间隔增加1秒，使敌人使用攻击牌的间隔增加1秒，持续12秒
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

