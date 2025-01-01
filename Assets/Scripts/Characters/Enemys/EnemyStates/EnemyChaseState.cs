using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyChaseState : EnemyState
{
    public Enemy enemy;

    public EnemyChaseState(Enemy enemy)
    {
        this.enemy = enemy;
    }

    public void OnEnter()
    {
    }

    public void OnExit()
    {
        enemy.OnMove(0);
    }

    public void OnUpdate()
    {
        //×·»÷×´Ì¬
        if (!enemy.CheckPlayerInSight())
        {
            enemy.StateTransition(EnemyStateType.Idle);
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

        //¹¥»÷
        if (enemy.CheckPlayerInAttackRegion())
        {
            enemy.StateTransition(EnemyStateType.Attack);
            return;
        }

        //×·»÷
        if (enemy.player.transform.position.x > enemy.transform.position.x)
        {
            enemy.OnMove(1);
        }
        else if (enemy.player.transform.position.x < enemy.transform.position.x)
        {
            enemy.OnMove(-1);
        }
    }
}
