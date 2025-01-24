using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[System.Serializable]
public class Buff
{
    //基础信息
    public string name;
    public string description;
    public Sprite sprite;
    //作用对象
    public Character target = null;
    //持续时间
    public float timer = 0;
    //永久存在
    public bool isPermanent = false;

    /// <summary>
    /// 初始化（某些参数对特定类型的buff无效）
    /// </summary>
    /// <param name="target">作用对象</param>
    /// <param name="timer">持续时间</param>
    /// <param name="isPermanent">是否永久存在</param>
    public virtual void Init(Character target, float timer = 0, bool isPermanent = false)
    {
        this.target = target;
        this.timer = timer;
        this.isPermanent = isPermanent;
        this.name = "无名buff";
        this.description = "没有任何效果";
        this.sprite = Resources.Load<Sprite>("BuffIcon/testIcon");
    }

    public virtual void OnEnter()
    {
    }

    public virtual void OnUpdate()
    {

    }

    public virtual void OnExit()
    {

    }
}

#region 装备buff
/// <summary>
/// 装备buff基类
/// </summary>
public class EquipmentBuff: Buff
{
    public override void Init(Character target, float timer = 0, bool isPermanent = false)
    {
        base.Init(target, timer, isPermanent);

        this.isPermanent = true;
    }
}

public class TestEquipmentBuff: EquipmentBuff
{
    bool isUpdated = false;

    public override void OnEnter()
    {
        base.OnEnter();
        Debug.Log("TestEquipmentBuff is added!");
    }

    public override void OnUpdate()
    {
        base.OnUpdate();
        if (!isUpdated)
        {
            Debug.Log("TestEquipmentBuff is updated succesfully!");
            isUpdated = true;
        }
    }

    public override void OnExit()
    {
        base.OnExit();
        Debug.Log("TestEquipmentBuff is Removed!");
    }
}

#endregion

#region 装备羽buff
/// <summary>
/// 装备羽buff基类
/// </summary>
public class EquipmentFeatherBuff : EquipmentBuff
{
    public EquipmentFeather feather = null;

    public override void OnEnter()
    {
        base.OnEnter();

        //Debug.Log(target);
        //Debug.Log(feather);
        target.AddFeather(feather);
    }

    public override void OnExit()
    {
        base.OnExit();

        target.RemoveFeather(feather);
    }
}
public class TestEquipmentFeatherBuff: EquipmentFeatherBuff
{
    public override void Init(Character target, float timer = 0, bool isPermanent = false)
    {
        base.Init(target, timer, isPermanent);

        this.feather = new TestEquipmentFeather();
    }

    public override void OnEnter()
    {
        base.OnEnter();

        Debug.Log("testEquipmentFeatherBuff added!");
    }

    public override void OnExit()
    {
        base.OnExit();

        Debug.Log("testEquipmentFeatherBuff removed!");
    }
}
#endregion

#region 攻击buff
/// <summary>
/// 攻击buff基类
/// </summary>
public class AttackBuff : Buff
{
    //攻击替身
    public GameObject attackBody = null;

    public override void Init(Character target, float timer = 0, bool isPermanent = false)
    {
        base.Init(target, timer, isPermanent);

        this.isPermanent = false;
    }

    public override void OnEnter()
    {
        base.OnEnter();

        target.AddAttackBody(attackBody);
    }

    public override void OnExit()
    {
        base.OnExit();

        target.RemoveAttackBody(attackBody);
    }
}

public class TestAttackBuff : AttackBuff
{
    public override void Init(Character target, float timer = 0, bool isPermanent = false)
    {
        base.Init(target, timer, isPermanent);

        this.timer = 10;
        this.attackBody = Resources.Load<GameObject>("AttackBodys/testAttackBody");
    }

    public override void OnEnter()
    {
        base.OnEnter();

        Debug.Log("TestAttackBuff added!");
    }

    public override void OnExit()
    {
        base.OnExit();

        Debug.Log("TestAttackBuff removed!");
    }
}

public class TestEnemyAttackBuff : AttackBuff
{
    public override void Init(Character target, float timer = 0, bool isPermanent = false)
    {
        base.Init(target, timer, isPermanent);

        this.timer = 10;
        this.attackBody = Resources.Load<GameObject>("AttackBodys/testEnemyAttackBody");
    }
}

public class PrinceSwordAttackBuff : AttackBuff
{
    public override void Init(Character target, float timer = 0, bool isPermanent = false)
    {
        base.Init(target, timer, isPermanent);

        this.timer = 5;
        this.attackBody = Resources.Load<GameObject>("AttackBodys/PrinceSwordAttackBody");
        //Debug.Log(attackBody);
    }
}
#endregion

#region 拔羽buff
/// <summary>
/// 拔羽buff基类
/// </summary>
public class UnlockFeatherBuff: Buff
{
    //拔羽数
    public int unlockFeatherNum = 0;

    public override void Init(Character target, float timer = 0, bool isPermanent = false)
    {
        base.Init(target, timer, isPermanent);

        this.isPermanent = false;
    }

    public override void OnEnter()
    {
        base.OnEnter();

        target.UnlockFeather(unlockFeatherNum, timer);
    }
}

public class TestUnlockFeatherBuff: UnlockFeatherBuff
{
    public override void Init(Character target, float timer = 0, bool isPermanent = false)
    {
        base.Init(target, timer, isPermanent);

        this.timer = 10;
        this.unlockFeatherNum = 1;
    }
}

public class UnlockFeather5sBuff : UnlockFeatherBuff
{
    public override void Init(Character target, float timer = 0, bool isPermanent = false)
    {
        base.Init(target, timer, isPermanent);

        this.timer = 5;
        this.unlockFeatherNum = 1;
    }
}

public class UnlockFeather10sBuff : UnlockFeatherBuff
{
    public override void Init(Character target, float timer = 0, bool isPermanent = false)
    {
        base.Init(target, timer, isPermanent);

        this.timer = 10;
        this.unlockFeatherNum = 1;
    }
}

public class UnlockFeather15sBuff : UnlockFeatherBuff
{
    public override void Init(Character target, float timer = 0, bool isPermanent = false)
    {
        base.Init(target, timer, isPermanent);

        this.timer = 15;
        this.unlockFeatherNum = 1;
    }
}

public class UnlockFeather20sBuff : UnlockFeatherBuff
{
    public override void Init(Character target, float timer = 0, bool isPermanent = false)
    {
        base.Init(target, timer, isPermanent);

        this.timer = 20;
        this.unlockFeatherNum = 1;
    }
}

#endregion

#region 效果buff
/// <summary>
/// 效果buff基类
/// </summary>
public class EffectBuff : Buff
{
    public override void Init(Character target, float timer = 0, bool isPermanent = false)
    {
        base.Init(target, timer, isPermanent);

        this.isPermanent = false;
    }
}

public class TestEffectBuff : EffectBuff
{
    public override void OnEnter()
    {
        base.OnEnter();
        Debug.Log("add TestEffectBuff!");
    }

    public override void OnUpdate()
    {
        base.OnUpdate();
    }

    public override void OnExit()
    {
        base.OnExit();
        Debug.Log("remove TestEffectBuff!");
    }
}

public class TestEnemyEffectBuff : EffectBuff
{
    public override void Init(Character target, float timer = 0, bool isPermanent = false)
    {
        base.Init(target, timer, isPermanent);

        this.timer = 5;
    }

    public override void OnEnter()
    {
        base.OnEnter();
        //Debug.Log("add TestEnemyEffectBuff!");
    }

    public override void OnUpdate()
    {
        base.OnUpdate();
    }

    public override void OnExit()
    {
        base.OnExit();
        //Debug.Log("remove TestEnemyEffectBuff!");
    }
}
#endregion