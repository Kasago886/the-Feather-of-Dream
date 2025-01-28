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

    public virtual void OnUpdate()
    {
        ///各个范围由外向内依次是：
        ///                  视野外            Idle
        ///视野内            最小距离外        接近
        ///最小距离内        攻击范围外        有攻击替身则接近，无攻击替身则静止
        ///攻击范围内                          静止

        //视野外
        if (!enemy.CheckPlayerInSight())
        {
            //静止
            enemy.StateTransition(EnemyStateType.Idle);
            return;
        }
        //视野内

        //最小距离外
        if (!enemy.CheckPlayerInSight(enemy.searchType, enemy.minDistance, 0))
        {
            //接近
            enemy.MoveRelateToPlayer(1);
        }
        //最小距离内，攻击范围外
        else if(!enemy.CheckPlayerInAttackRegion())
        {
            //有攻击替身则接近，无攻击替身则静止
            if (enemy.attackBodyObjList.Count > 0)
            {
                enemy.MoveRelateToPlayer(1);
            }
            else
            {
                enemy.MoveRelateToPlayer(0);
            }
        }
        //攻击范围内静止
        else
        {
            enemy.MoveRelateToPlayer(0);
        }

        //攻击范围内攻击
        if (enemy.CheckPlayerInAttackRegion())
        {
            enemy.StateTransition(EnemyStateType.Attack);
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


public class KeepDistanceWhenNotArmedChaseState : EnemyChaseState
{
    public KeepDistanceWhenNotArmedChaseState(Enemy enemy) : base(enemy)
    {
    }

    public override void OnUpdate()
    {
        ///各个范围由外向内依次是：
        ///                  视野外            Idle
        ///视野内            攻击卡使用范围外  接近
        ///攻击卡使用范围内  最小距离外        有攻击替身则接近，无攻击替身则静止
        ///最小距离内        攻击范围外        有攻击替身则接近，无攻击替身则远离
        ///攻击范围内                          有攻击替身则静止，无攻击替身则远离

        //视野外
        if (!enemy.CheckPlayerInSight())
        {
            //静止
            enemy.StateTransition(EnemyStateType.Idle);
            return;
        }
        //视野内

        //攻击卡使用范围外
        if (!enemy.CheckPlayerInAttackCardDistance())
        {
            //接近
            enemy.MoveRelateToPlayer(1);
        }
        //攻击卡使用范围内
        else
        {
            //最小距离外
            if (!enemy.CheckPlayerInSight(enemy.searchType, enemy.minDistance, 0))
            {
                //有攻击替身则接近，无攻击替身则静止
                if (enemy.attackBodyObjList.Count > 0)
                {
                    enemy.MoveRelateToPlayer(1);
                }
                else
                {
                    enemy.MoveRelateToPlayer(0);
                }
            }
            //最小距离内
            else
            {
                //攻击范围外
                if (!enemy.CheckPlayerInAttackRegion())
                {
                    //有攻击替身则接近，无攻击替身则远离
                    if (enemy.attackBodyObjList.Count > 0)
                    {
                        enemy.MoveRelateToPlayer(1);
                    }
                    else
                    {
                        enemy.MoveRelateToPlayer(-1);
                    }
                }
                //攻击范围内
                else
                {
                    //有攻击替身则静止，无攻击替身则远离
                    if (enemy.attackBodyObjList.Count <= 0)
                    {
                        enemy.MoveRelateToPlayer(-1);
                    }
                }
            }
        }

        //攻击范围内攻击
        if (enemy.CheckPlayerInAttackRegion())
        {
            enemy.StateTransition(EnemyStateType.Attack);
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