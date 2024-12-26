using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Buff
{
    public float timer;
    public bool isPermanent;

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
    public EquipmentBuff()
    {
        isPermanent = true;
    }
}

public class TestEquipmentBuff: EquipmentBuff
{
    bool isUpdated = false;

    public override void OnEnter()
    {
        Debug.Log("TestEquipmentBuff is added!");
    }

    public override void OnUpdate()
    {
        if (!isUpdated)
        {
            Debug.Log("TestEquipmentBuff is updated succesfully!");
            isUpdated = true;
        }
    }

    public override void OnExit()
    {
        Debug.Log("TestEquipmentBuff is Removed!");
    }
}

#endregion

#region 攻击buff
/// <summary>
/// 攻击buff基类
/// </summary>
public class AttackBuff : Buff
{
    public AttackBuff(float t)
    {
        timer = t;
        isPermanent = false;
    }
}

public class TestAttackBuff : AttackBuff
{
    public TestAttackBuff(float t) : base(t)
    {
    }

    public override void OnEnter()
    {
        Debug.Log("TestAttackBuff added!");
    }

    public override void OnUpdate()
    {
        
    }

    public override void OnExit()
    {
        Debug.Log("TestAttackBuff removed!");
    }
}
#endregion

#region 拔羽buff
/// <summary>
/// 拔羽buff基类
/// </summary>
public class UnlockFeatherBuff: Buff
{
    public UnlockFeatherBuff(float t)
    {
        timer = t;
        isPermanent = false;
    }
}

public class TestUnlockFeatherBuff: UnlockFeatherBuff
{
    public TestUnlockFeatherBuff(float t) : base(t)
    {

    }

    public override void OnEnter()
    {

    }

    public override void OnUpdate()
    {

    }

    public override void OnExit()
    {

    }
}
#endregion

#region 效果buff
/// <summary>
/// 效果buff基类
/// </summary>
public class EffectBuff : Buff
{
    public EffectBuff(float t)
    {
        timer = t;
        isPermanent = false;
    }
}

public class TestEffectBuff : EffectBuff
{
    public TestEffectBuff(float t): base(t)
    {
    }

    public override void OnEnter()
    {
        Debug.Log("add TestEffectBuff!");
    }

    public override void OnUpdate()
    {
        
    }

    public override void OnExit()
    {
        Debug.Log("remove TestEffectBuff!");
    }
}
#endregion