using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class FinalBoss : Enemy
{
    public bool unattackable = false;
    public bool collisionAttack = false;
    public bool enemyAI = false;

    FinalBossSteps finalBossSteps;

    new private void Start()
    {
        base.Start();
        finalBossSteps = GetComponent<FinalBossSteps>();

        unattackable = false;
        collisionAttack = false;
        enemyAI = false;
    }

    public override void AIUpdate()
    {
        if (enemyAI)
        {
            base.AIUpdate();
        }
    }

    public void SetUnattackable(bool unattackable)
    {
        this.unattackable = unattackable;
    }

    public override void TakeDamage(float damage, Transform attackTrans = null)
    {
        Debug.Log("unattackable:" + unattackable);
        if (!unattackable)
        {
            base.TakeDamage(damage, attackTrans);
        }
    }

    public override void OnDeath()
    {
        AddFeather(new DefautFeather(defaultFeatherHealth));
        finalBossSteps.NextPhase();
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
