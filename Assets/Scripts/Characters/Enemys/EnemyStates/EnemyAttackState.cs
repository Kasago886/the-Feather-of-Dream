using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyAttackState : EnemyState
{
    public Enemy enemy;

    public EnemyAttackState(Enemy enemy)
    {
        this.enemy = enemy;
    }

    public void OnEnter()
    {
    }

    public void OnExit()
    {
    }

    public void OnUpdate()
    {
        if (!enemy.CheckPlayerInAttackRegion())
        {
            enemy.StateTransition(EnemyStateType.Idle);
            return;
        }

        enemy.OnAttack();
    }
}
