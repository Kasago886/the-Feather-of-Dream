using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class testEnemy : Enemy
{
    public override void OnUseAttackCard()
    {
        base.OnUseAttackCard();
        if (attackCardCooldownTimer <= 0)
        {
            attackCardCooldownTimer = attackCardCooldown;
            AddBuff("²âÊÔµÐÈË¹¥»÷");

            player.UnlockFeather(1, 10);
        }
    }

    public override void OnUseEffectCard()
    {
        base.OnUseEffectCard();

        if (effectCardCooldownTimer <= 0)
        {
            effectCardCooldownTimer = effectCardCooldown;
            AddBuff("²âÊÔµÐÈËÐ§¹û");
        }
    }
}
