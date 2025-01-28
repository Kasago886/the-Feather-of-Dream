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
        ///各个范围由外向内依次是：
        ///                  视野外            静止
        ///视野内            攻击范围外        Chase
        ///攻击范围内                          Attack
        
        //攻击
        if (enemy.CheckPlayerInAttackRegion())
        {
            enemy.StateTransition(EnemyStateType.Attack);
            return;
        }

        //追击
        if (enemy.CheckPlayerInSight())
        {
            enemy.StateTransition(EnemyStateType.Chase);
            return;
        }

        //攻击卡
        if (enemy.CheckPlayerInAttackCardDistance())
        {
            enemy.OnUseAttackCard();
        }

        //效果卡
        if (enemy.CheckPlayerInEffectCardDistance())
        {
            enemy.OnUseEffectCard();
        }
    }
}
