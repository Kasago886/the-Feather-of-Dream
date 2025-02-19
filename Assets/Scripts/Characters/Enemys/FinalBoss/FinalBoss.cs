using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class FinalBoss : Enemy
{
    public bool unattackable = false;
    public bool collisionAttack = false;

    FinalBossSteps finalBossSteps;

    new private void Start()
    {
        base.Start();
        finalBossSteps = GetComponent<FinalBossSteps>();

        unattackable = false;
        collisionAttack = false;
    }

    public override void AIUpdate()
    {

    }

    public void SetUnattackable(bool unattackable)
    {
        this.unattackable = unattackable;
    }

    public override void TakeDamage(float damage, Transform attackTrans = null)
    {
        if (!unattackable)
        {
            base.TakeDamage(damage, attackTrans);
        }
    }

    public override void OnDeath()
    {
        finalBossSteps.NextPhase();
        AddFeather(new DefautFeather(defaultFeatherHealth));
    }

    public void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision != null && collisionAttack)
        {
            if (collision.CompareTag(Consts.PlayerTag))
            {
                player.TakeDamage(30);
            }
        }
    }
}
