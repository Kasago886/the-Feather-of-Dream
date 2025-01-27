using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyInjuryState : EnemyState
{
    public Enemy enemy;

    public EnemyInjuryState(Enemy enemy)
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
        if (enemy.forcebackTimer <= 0)
        {
            enemy.StateTransition(EnemyStateType.Idle);
            return;
        }
    }
}
