using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TheMisunderstoodWerewolfCardSkill : Card
{
    private Enemy enemy;
    private float health;
    private void Start()
    {
        if (gameObject.GetComponentInParent<Enemy>() != null)
        {
            enemy = GetComponentInParent<Enemy>();
        }
    }
    public override bool ConditionsOfUseCard()
    {
        if (health > enemy.unlockedFeathers[0].maxHealth / 2 && enemy.unlockedFeathers[0].health< enemy.unlockedFeathers[0].maxHealth / 2)
        {
            return true;
        }
        health = enemy.unlockedFeathers[0].health;
        return false;
    }
}
