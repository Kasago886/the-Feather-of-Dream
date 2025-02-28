using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Feather
{
    public float health;
    public float maxHealth;
    public float lockTimer = 0;

    public HpUI hpUI = null;

    public virtual void TakeDamage(float damage)
    {
        health -= damage;//需要为负数，以便于character计算伤害，在character中有归零

        if (hpUI != null)
        {
            hpUI.testHp = health;
        }
    }
}

public class DefautFeather : Feather
{
    public DefautFeather(float hp = 100)
    {
        maxHealth = hp;
        health = maxHealth;
    }
}

public class EquipmentFeather : Feather
{
    public Item item = null;
    public EquipmentFeather(float hp = 100, float MaxHealth = 100)
    {
        maxHealth = MaxHealth;
        health = hp;
    }

    public override void TakeDamage(float damage)
    {
        base.TakeDamage(damage);

        Debug.Log(item.itemName+":"+health);
    }
}

public class TestEquipmentFeather : EquipmentFeather
{
    public static float MaxHealth = 100;

    public TestEquipmentFeather(float hp = 100): base(hp)
    {
        maxHealth = MaxHealth;
    }
}

public class EllieEquipmentFeather : EquipmentFeather
{
    public static float MaxHealth = 100;

    public EllieEquipmentFeather(float hp = 100) : base(hp)
    {
        maxHealth = MaxHealth;
    }
}
