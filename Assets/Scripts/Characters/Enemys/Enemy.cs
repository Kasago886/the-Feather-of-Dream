using System;
using System.Collections;
using System.Collections.Generic;
using System.Net.Mail;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;

public enum EnemySearchType
{
    distance,horizontal,infinity
}

public enum EnemyStateType
{
    Idle,Chase,Attack,Injury
}

[System.Serializable]
public class EnemyCardWithTimer
{
    public Card card;
    public float cooldown;
    [HideInInspector]public float timer;
}

[RequireComponent(typeof(HpUi2_FollowEnemy))]
public class Enemy : Character
{
    public string enemyName;

    public float runSpeed;
    public float jumpSpeed;
    public float attackCooldown;
    protected float attackCooldownTimer = 0;

    public EnemySearchType searchType;
    public bool wallDetect;

    public float searchDistance;
    public float minDistance;

    public float attackCardUseDistance;
    public float attackCardCooldown;
    protected float attackCardCooldownTimer = 0;

    public float effectCardUseDistance;
    public float effectCardCooldown;
    protected float effectCardCooldownTimer = 0;

    public bool isSingleAttackCardCooldown;
    public bool isSingleEffectCardCooldown;
    public List<EnemyCardWithTimer> attackCardsWithTimer = new();
    public List<EnemyCardWithTimer> effectCardsWithTimer = new();
    public List<Card> attackCards = new();
    public List<Card> effectCards = new();

    [HideInInspector] public Player player;

    //states
    Dictionary<EnemyStateType, EnemyState> stateDict = new();
    EnemyState currentState;

    new protected void Start()
    {
        base.Start();
        player = FindAnyObjectByType<Player>();

        injuryEvent.AddListener(new UnityAction(TransitionToInjury));

        stateDict[EnemyStateType.Idle] = new EnemyIdleState(this);
        stateDict[EnemyStateType.Chase] = new EnemyChaseState(this);
        stateDict[EnemyStateType.Attack] = new EnemyAttackState(this);
        stateDict[EnemyStateType.Injury] = new EnemyInjuryState(this);

        currentState = stateDict[EnemyStateType.Idle];
    }

    /// <summary>
    /// 切换状态
    /// </summary>
    /// <param name="stateType"></param>
    public void StateTransition(EnemyStateType stateType)
    {
        currentState.OnExit();
        currentState = stateDict[stateType];
        currentState.OnEnter();
    }
    /// <summary>
    /// 切换到受伤状态
    /// </summary>
    public void TransitionToInjury()
    {
        StateTransition(EnemyStateType.Injury);
    }

    /// <summary>
    /// 更新AI
    /// </summary>
    public override void AIUpdate()
    {
        base.AIUpdate();

        currentState.OnUpdate();

        if (!isDead)
        {
            if (!(forcebackTimer > 0))
            {
                attackCooldownTimer -= Time.deltaTime;
                attackCardCooldownTimer -= Time.deltaTime;
                effectCardCooldownTimer -= Time.deltaTime;
            }

            if (isSingleAttackCardCooldown)
            {
                foreach(EnemyCardWithTimer ecwt in attackCardsWithTimer)
                {
                    ecwt.timer -= Time.deltaTime;
                }
            }
            if (isSingleEffectCardCooldown)
            {
                foreach (EnemyCardWithTimer ecwt in effectCardsWithTimer)
                {
                    ecwt.timer -= Time.deltaTime;
                }
            }
        }
    }

    #region 状态检查函数
    /// <summary>
    /// 检测玩家是否在视野内
    /// </summary>
    /// <returns></returns>
    public bool CheckPlayerInSight()
    {
        return CheckPlayerInSight(searchType);
    }
    /// <summary>
    /// 检测玩家是否在视野内
    /// </summary>
    /// <returns></returns>
    public bool CheckPlayerInSight(EnemySearchType searchType)
    {
        //检测墙体阻挡
        if (wallDetect)
        {
            RaycastHit2D[] hits = Physics2D.LinecastAll(transform.position, player.transform.position, LayerMask.GetMask(Consts.WallLayer));
            if (hits.Length > 0)
            {
                return false;
            }
        }

        float distance = -1;
        switch (searchType)
        {
            //圆形视野
            case EnemySearchType.distance:
                distance = Vector2.Distance(transform.position, player.transform.position);
                if (distance < searchDistance && distance > minDistance)
                {
                    return true;
                }
                break;

            //水平距离视野
            case EnemySearchType.horizontal:
                distance = Mathf.Abs(transform.position.x - player.transform.position.x);
                if (distance < searchDistance && distance > minDistance)
                {
                    return true;
                }
                break;

            //无穷视野
            case EnemySearchType.infinity:
                distance = Vector2.Distance(transform.position, player.transform.position);
                if (distance > minDistance)
                {
                    return true;
                }
                break;
        }

        return false;
    }

    /// <summary>
    /// 检测是否可使用攻击卡
    /// </summary>
    /// <returns></returns>
    public bool CheckPlayerInAttackCardDistance()
    {
        if (Vector2.Distance(transform.position, player.transform.position) < attackCardUseDistance && attackCardCooldownTimer <= 0)
        {
            return true;
        }
        return false;
    }

    /// <summary>
    /// 检测是否可使用效果卡
    /// </summary>
    /// <returns></returns>
    public bool CheckPlayerInEffectCardDistance()
    {
        if (Vector2.Distance(transform.position, player.transform.position) < effectCardUseDistance && effectCardCooldownTimer <= 0)
        {
            return true;
        }
        return false;
    }

    /// <summary>
    /// 检查是否可攻击
    /// </summary>
    /// <returns></returns>
    public bool CheckPlayerInAttackRegion()
    {
        foreach (GameObject attackBodyObj in attackBodyObjList)
        {
            AttackBody attackBody = attackBodyObj.GetComponent<AttackBody>();
            if (attackBody.GetTargetsInAttackRegion(transform.position, spriteRenderer.flipX).Length > 0)
            {
                return true;
            }
        }

        return false;
    }

    #endregion

    /// <summary>
    /// 移动
    /// </summary>
    /// <param name="horizontalMove"></param>
    public virtual void OnMove(float horizontalMove)
    {
        float speed = runSpeed * horizontalMove;
        rb.velocity = new Vector2(speed, rb.velocity.y);
        //Debug.Log(rb.velocity);

        if (speed > 0)
        {
            spriteRenderer.flipX = false;
        }
        else if (speed < 0)
        {
            spriteRenderer.flipX = true;
        }
    }

    /// <summary>
    /// 攻击
    /// </summary>
    public override void OnAttack()
    {
        if (attackCooldownTimer <= 0)
        {
            attackCooldownTimer = attackCooldown;

            base.OnAttack();
        }
    }

    /// <summary>
    /// 使用攻击卡
    /// </summary>
    public virtual void OnUseAttackCard()
    {
        if (attackCards.Count > 0 && attackCardCooldownTimer <= 0)
        {
            //单独cd
            if (isSingleAttackCardCooldown)
            {
                //找出cd到了的卡牌信息实例
                List<EnemyCardWithTimer> availableECWT = new List<EnemyCardWithTimer>();
                foreach(EnemyCardWithTimer ecwt in attackCardsWithTimer)
                {
                    if (ecwt.timer <= 0)
                    {
                        availableECWT.Add(ecwt);
                    }
                }

                //抽卡
                if (availableECWT.Count > 0)
                {
                    int index = UnityEngine.Random.Range(0, availableECWT.Count);
                    Card card = availableECWT[index].card;
                    card.EnemyHasEffectOnPlayer(enemyName);

                    //冷却时间
                    availableECWT[index].timer = availableECWT[index].cooldown;
                    attackCardCooldownTimer = attackCardCooldown;
                }
            }
            //无单独cd
            else if (attackCards.Count > 0)
            {
                //Random.Range(a,b)不含右值
                int index = UnityEngine.Random.Range(0, attackCards.Count);
                Card card = attackCards[index];
                card.EnemyHasEffectOnPlayer(enemyName);

                attackCardCooldownTimer = attackCardCooldown;
            }
        }
        
    }

    /// <summary>
    /// 使用效果卡
    /// </summary>
    public virtual void OnUseEffectCard()
    {
        if (effectCards.Count > 0 && effectCardCooldownTimer <= 0)
        {
            //单独cd
            if (isSingleEffectCardCooldown)
            {
                //找出cd到了的卡牌信息实例
                List<EnemyCardWithTimer> availableECWT = new List<EnemyCardWithTimer>();
                foreach (EnemyCardWithTimer ecwt in effectCardsWithTimer)
                {
                    if (ecwt.timer <= 0)
                    {
                        availableECWT.Add(ecwt);
                    }
                }

                //抽卡
                if (availableECWT.Count > 0)
                {
                    int index = UnityEngine.Random.Range(0, availableECWT.Count);
                    Card card = availableECWT[index].card;
                    card.EnemyHasEffectOnPlayer(enemyName);

                    //冷却时间
                    availableECWT[index].timer = availableECWT[index].cooldown;
                    effectCardCooldownTimer = effectCardCooldown;
                }
            }
            //无单独cd
            else if (effectCards.Count > 0)
            {
                int index = UnityEngine.Random.Range(0, effectCards.Count);
                Card card = effectCards[index];
                card.EnemyHasEffectOnPlayer(enemyName);

                effectCardCooldownTimer = effectCardCooldown;
            }
        }
    }

    private void OnDrawGizmosSelected()
    {
        int side = 20;
        float angle = 360f / side;

        Gizmos.color = Color.yellow;

        switch (searchType)
        {
            //圆形视野
            case EnemySearchType.distance:
                for (int i = 0; i < side; i++)
                {
                    //最远
                    Vector2 from = transform.position + Quaternion.Euler(0, 0, angle * i) * Vector2.right * searchDistance;
                    Vector2 to = transform.position + Quaternion.Euler(0,0,angle * (i+1)) * Vector2.right * searchDistance;
                    //最近
                    Vector2 from2 = transform.position + Quaternion.Euler(0, 0, angle * i) * Vector2.right * minDistance;
                    Vector2 to2 = transform.position + Quaternion.Euler(0, 0, angle * (i + 1)) * Vector2.right * minDistance;

                    //圆圈
                    Gizmos.DrawLine(from,to);
                    Gizmos.DrawLine(from2, to2);

                    //阴影
                    Gizmos.DrawLine(from, from2);
                }

                break;

            //水平距离视野
            case EnemySearchType.horizontal:
                //最远
                Gizmos.DrawLine(new Vector2(transform.position.x - searchDistance, transform.position.y + 50), new Vector2(transform.position.x - searchDistance, transform.position.y - 50));
                Gizmos.DrawLine(new Vector2(transform.position.x + searchDistance, transform.position.y + 50), new Vector2(transform.position.x + searchDistance, transform.position.y - 50));
                //最近
                Gizmos.DrawLine(new Vector2(transform.position.x - minDistance, transform.position.y + 10), new Vector2(transform.position.x - minDistance, transform.position.y - 10));
                Gizmos.DrawLine(new Vector2(transform.position.x + minDistance, transform.position.y + 10), new Vector2(transform.position.x + minDistance, transform.position.y - 10));

                //连线
                Gizmos.DrawLine(new Vector2(transform.position.x + searchDistance, transform.position.y), new Vector2(transform.position.x + minDistance, transform.position.y));
                Gizmos.DrawLine(new Vector2(transform.position.x - searchDistance, transform.position.y), new Vector2(transform.position.x - minDistance, transform.position.y));

                break;

            //无穷视野
            case EnemySearchType.infinity:
                for (int i = 0; i < side; i++)
                {
                    //最近
                    Vector2 from2 = transform.position + Quaternion.Euler(0, 0, angle * i) * Vector2.right * minDistance;
                    Vector2 to2 = transform.position + Quaternion.Euler(0, 0, angle * (i + 1)) * Vector2.right * minDistance;

                    //圆圈
                    Gizmos.DrawLine(from2, to2);
                }
                //连线
                player = FindAnyObjectByType<Player>();
                Vector3 direction = player.transform.position - transform.position;
                if (direction.sqrMagnitude > minDistance * minDistance)
                {
                    Gizmos.DrawRay(transform.position + direction.normalized * minDistance, direction - direction.normalized * minDistance);
                }
                break;
        }

        //攻击卡使用范围
        Gizmos.color = Color.red;
        for (int i = 0; i < side; i++)
        {
            Vector2 from = transform.position + Quaternion.Euler(0, 0, angle * i) * Vector2.right * attackCardUseDistance;
            Vector2 to = transform.position + Quaternion.Euler(0, 0, angle * (i + 1)) * Vector2.right * attackCardUseDistance;

            Gizmos.DrawLine(from, to);
        }

        //效果卡使用范围
        Gizmos.color = Color.green;
        for (int i = 0; i < side; i++)
        {
            Vector2 from = transform.position + Quaternion.Euler(0, 0, angle * i) * Vector2.right * effectCardUseDistance;
            Vector2 to = transform.position + Quaternion.Euler(0, 0, angle * (i + 1)) * Vector2.right * effectCardUseDistance;

            Gizmos.DrawLine(from, to);
        }
    }
}
