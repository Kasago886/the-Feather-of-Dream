using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyIdleState : EnemyState
{
    public Enemy enemy;

    public EnemyIdleState(Enemy enemy)
    {
        this.enemy = enemy;
    }

    public void OnEnter()
    {
        enemy.OnMove(0);
    }

    public void OnExit()
    {
    }

    public void OnUpdate()
    {
        //¹¥»÷
        if (enemy.CheckPlayerInAttackRegion())
        {
            enemy.StateTransition(EnemyStateType.Attack);
            return;
        }

        //×·»÷
        if (enemy.CheckPlayerInSight())
        {
            enemy.StateTransition(EnemyStateType.Chase);
            return;
        }

        //¹¥»÷¿¨
        if (enemy.CheckPlayerInAttackCardDistance())
        {
            enemy.OnUseAttackCard();
        }

        //Ð§¹û¿¨
        if (enemy.CheckPlayerInEffectCardDistance())
        {
            enemy.OnUseEffectCard();
        }
    }
}
