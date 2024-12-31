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
        if (enemy.CheckPlayerInSight())
        {
            enemy.StateTransition(EnemyStateType.Chase);
        }
    }
}
