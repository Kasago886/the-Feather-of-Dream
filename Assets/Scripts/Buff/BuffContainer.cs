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
        {"艾莉之羽", typeof(EllieEquipmentFeatherBuff) },
        {"疯猎之残羽", typeof(HunterEquipmentFeatherBuff) },
        {"艾莉之剑", typeof(ElliesSwordAttackBuff) },
        {"王子之剑", typeof(PrinceSwordAttackBuff) },
        {"侍卫短剑", typeof(PrinceGuardSwordAttackBuff) },
        {"王子权柄", typeof(PrincePowerEffectBuff) },
        {"猎人预感", typeof(CrazyHunterAttackBuff) },
        {"狩猎", typeof(CrazyHunterAttackBuff1) },
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
        {"忧郁",typeof(Depressed) },//使玩家的获得卡牌的时间间隔增加1秒，使敌人使用攻击牌的间隔增加1秒，持续12秒
        {"崩溃",typeof(Crash) },//使玩家或敌人的力量和韧性降低初始最初力量和韧性的10%，持续13秒
        {"凡庸",typeof(Mediocre) },//使玩家减少一个卡槽，使敌人的效果牌使用时间间隔+3s，持续1分钟，Boss被污染的高级员工2阶段时，持续时间无限，并使对象的攻击可为受击者添加一层"凡庸"，并令自身失去一层"凡庸"
        {"才华",typeof(Talent) },//使对象韧性乘10，持续29.7s
        {"博学",typeof(Erudite) },//使对象力量乘2，持续29.7s
        {"不被认可",typeof(Ignore) },//使敌人的技能时间间隔增加0.05s，最高99层，仅限Boss被污染的高级员工1阶段
        {"瑕疵之作",typeof(ImperfectWork) },//当Boss被污染的高级员工2阶段失去一定量的血量时，使玩家选择一个debuff，持续时间：Boss被污染的高级员工2阶段开始至其被击杀
        {"渴求认可",typeof(CraveRecognition) },//当Boss被污染的高级员工2阶段失去一定量的血量时，"凡庸"按一定比例转化为"渴求认可"，每次受击扣除0.5滴血，持续时间无限
        {"振奋",typeof(UpLifting) },//使对象增加10%的力量，最高不超过1点力量，持续16s
        {"坚韧",typeof(Toughness) },//使对象增加10%的韧性，最高不超过2点韧性，持续16s
        {"神速",typeof(BlazingSpeed) },//使对象加10%速度，持续16s
        {"轻健",typeof(Agile) },//使对象加10%起跳速度，持续16s
        {"坚定",typeof(Steadfast) },//使玩家的获得卡牌的时间间隔增加1秒，使敌人使用攻击牌的间隔增加1秒，持续12秒
        {"卓越",typeof(Superb) },//使玩家增加一个卡槽，使敌人的效果牌使用时间间隔-3s，持续1分钟
        {"萎靡",typeof(Lethargic) },//使对象降低10%的力量，最高不超过1点力量，持续16s
        {"脆弱",typeof(Fragile) },//使对象降低10%的韧性，最高不超过2点韧性，持续16s
        {"迟缓",typeof(Sluggish) },//使对象降低10%速度，持续16s
        {"凝重",typeof(Grave) },//使对象降低10%起跳速度，持续16s
        {"中毒",typeof(PoisonBuff) },//使对象每秒扣除5滴血，持续5s
        {"正义",typeof(JusticeBuff) },//使玩家下一张攻击卡攻击力翻倍
        {"噬血匕首",typeof(HunterFeatherAttackBuff) },
        {"狂猎之枪",typeof(HunterFeatherAttackBuffGun) },
        {"起动",typeof(HunterFeatherEffectBuff) },
        {"猎杀",typeof(HunterFeatherEffectBuff1) },
        {"灼伤",typeof(Scorch) },//使对象每0.1秒扣除0.1滴血，持续6s，buff持续期间如果被施加新的灼伤则持续时间延长6s
        {"烈焰",typeof(Burn) },//触发条件：对象的灼伤抵抗小于5并且对象的灼伤层数大于10，效果：使对象的异常抗性减半，持续时间60s
        {"治愈Ⅰ型",typeof(Heal1) },//在有羽解锁的条件下，每0.1s回复0.2点生命值，持续12秒
        {"治愈Ⅱ型",typeof(Heal2) },//在有羽解锁的条件下，每0.1s回复0.3点生命值，持续30秒，如果受到伤害立即停止
        {"治愈Ⅲ型",typeof(Heal3) },//在有羽解锁的条件下，持续30s，buff结束时回复等同于buff持续期间所受伤害的生命值
        {"随机Buff",typeof(RandomBuff) },
        {"异常抵抗",typeof(AbnormalResistance) },//使对象提高10%的异常抗性，持续16s
        {"异常脆弱",typeof(AbnormalFragility) },//使对象降低10%的异常抗性，持续16s
        {"灼伤抵抗",typeof(BurnResistance) },//使对象提高1点灼伤抵抗，持续16s
        {"灼伤脆弱",typeof(BurnFragility) },//使对象降低1点灼伤抵抗，持续16s
        {"伤痕抵抗",typeof(TraumaResistance) },//使对象提高1点伤痕抗性，持续16s
        {"伤痕脆弱",typeof(TraumaFragility) },//使对象降低1点伤痕抗性，持续16s
        {"裂隙",typeof(Fissure) },//触发条件：对象的伤痕抵抗小于5并且对象的伤痕层数大于13，效果：使对象在n次受击后，受到n*伤痕层数的伤害，持续时间无限至发挥作用
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

