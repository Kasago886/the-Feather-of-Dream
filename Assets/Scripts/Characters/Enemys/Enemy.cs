using System;
using System.Collections;
using System.Collections.Generic;
using System.Net.Mail;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using static UnityEngine.EventSystems.EventTrigger;

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

[System.Serializable]
public class EnemyCardLine
{
    public List<EnemyCardWithTimer> cards;
    public float cooldown;
    [HideInInspector] public float timer;
}

[RequireComponent(typeof(HpUi2_FollowEnemy))]
public class Enemy : Character
{
    public string enemyName;

    public float runSpeed;
    public float jumpSpeed;
    public float attackCooldown;
    [HideInInspector] public float attackCooldownTimer = 0;

    public EnemySearchType searchType;
    public bool wallDetect;
    public bool keepDistanceWhenNotArmed;

    public float searchDistance;
    public float minDistance;

    public float attackCardUseDistance;
    public float effectCardUseDistance;

    public List<EnemyCardLine> attackCardLineList = new();
    public List<EnemyCardLine> effectCardLineList = new();

    public DropItem[] dropItems;
    public int exp;

    public FlagType flag;
    public bool refreshable;
    public Item encyclopedia;

    [HideInInspector] public Player player;

    EnemyUIScroll enemyUIScroll;
    InputListener inputListener;
    EquipmentPanelManager equipmentPanelManager;

    //states
    protected Dictionary<EnemyStateType, EnemyState> stateDict = new();
    protected EnemyState currentState;

    new protected void Start()
    {
        base.Start();

        //flag
        if (ArchiveManager.CheckFlag(flag) && !refreshable)
        {
            deathEvent?.Invoke();
            gameObject.SetActive(false);
            return;
        }

        player = FindAnyObjectByType<Player>();
        enemyUIScroll = FindAnyObjectByType<EnemyUIScroll>();
        inputListener = FindAnyObjectByType<InputListener>();
        equipmentPanelManager = FindAnyObjectByType<EquipmentPanelManager>();

        injuryEvent.AddListener(new UnityAction(TransitionToInjury));

        stateDict[EnemyStateType.Idle] = new EnemyIdleState(this);
        if (keepDistanceWhenNotArmed)
        {
            stateDict[EnemyStateType.Chase] = new KeepDistanceWhenNotArmedChaseState(this);
        }
        else
        {
            stateDict[EnemyStateType.Chase] = new EnemyChaseState(this);
        }
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

        if (!isDead)
        {
            currentState.OnUpdate();

            if (!(forcebackTimer > 0))
            {
                attackCooldownTimer -= Time.deltaTime;
            }

            //遍历line
            foreach (EnemyCardLine cardLine in attackCardLineList)
            {
                cardLine.timer -= Time.deltaTime;
                foreach (EnemyCardWithTimer ectw in cardLine.cards)
                {
                    ectw.timer -= Time.deltaTime;
                }
            }
            foreach (EnemyCardLine cardLine in effectCardLineList)
            {
                cardLine.timer -= Time.deltaTime;
                foreach (EnemyCardWithTimer ectw in cardLine.cards)
                {
                    ectw.timer -= Time.deltaTime;
                }
            }
        }
    }

    /// <summary>
    /// 更新UI
    /// </summary>
    public override void UIUpdate()
    {
        bool showUI = false;

        //被拔羽时
        if (unlockedFeathers.Count > 0)
        {
            //Debug.Log(enemyName);
            showUI = true;
        }
        //使用卡牌且在视野内时
        if (inputListener.CompareStateType(InputListenerState.card))
        {
            if (CheckPlayerInSight())
            {
                showUI = true;
            }
        }

        //死亡时取消UI
        if (isDead)
        {
            showUI = false;
        }
        
        if (showUI)
        {
            enemyUIScroll.AddEnemyUI(this);

            //加入图鉴
            if (encyclopedia != null && equipmentPanelManager != null)
            {
                equipmentPanelManager.AddEncyclopedia(encyclopedia);
            }
        }
        else
        {
            enemyUIScroll.RemoveEnemyUI(this);
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
        return CheckPlayerInSight(searchType, searchDistance, 0);
    }
    public bool CheckPlayerInSight(EnemySearchType searchType, float maxDistance, float minDistance)
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
                if (distance < maxDistance && distance > minDistance)
                {
                    return true;
                }
                break;

            //水平距离视野
            case EnemySearchType.horizontal:
                distance = Mathf.Abs(transform.position.x - player.transform.position.x);
                if (distance < maxDistance && distance > minDistance)
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
    /// 检测是否在攻击卡范围内
    /// </summary>
    /// <returns></returns>
    public bool CheckPlayerInAttackCardDistance()
    {
        if (CheckPlayerInSight(EnemySearchType.distance,attackCardUseDistance,0))
        {
            return true;
        }
        return false;
    }

    /// <summary>
    /// 检测是否可使用效果卡范围内
    /// </summary>
    /// <returns></returns>
    public bool CheckPlayerInEffectCardDistance()
    {
        if (CheckPlayerInSight(EnemySearchType.distance, effectCardUseDistance, 0))
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
                //Debug.Log("In AttackRegion" + attackBody.gameObject);
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

        //动画
        if (animator != null)
        {
            animator.SetFloat(Consts.SpeedAnimatorArgument, Mathf.Abs(horizontalMove));
        }
    }
    /// <summary>
    /// 相对于玩家移动
    /// </summary>
    /// <param name="forward">forward = 1为靠近，forward = -1为远离</param>
    public virtual void MoveRelateToPlayer(float forward)
    {
        if (player.transform.position.x > transform.position.x)
        {
            OnMove(forward);
        }
        else if (player.transform.position.x < transform.position.x)
        {
            OnMove(-forward);
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
        //遍历line
        foreach (EnemyCardLine cardLine in attackCardLineList)
        {
            //找到cd到了且不为空的line
            if (cardLine.timer <= 0 && cardLine.cards.Count > 0)
            {
                //找出cd到了的ecwt
                List<EnemyCardWithTimer> availableECWT = new List<EnemyCardWithTimer>();
                foreach (EnemyCardWithTimer ecwt in cardLine.cards)
                {
                    if (ecwt.timer <= 0)
                    {
                        availableECWT.Add(ecwt);
                    }
                }

                //随机抽卡
                if (availableECWT.Count > 0)
                {
                    int index = UnityEngine.Random.Range(0, availableECWT.Count);
                    EnemyCardWithTimer ecwt = availableECWT[index];
                    Card card = ecwt.card;
                    card.EnemyHasEffectOnPlayer(enemyName);

                    //冷却时间
                    ecwt.timer = ecwt.cooldown;
                    cardLine.timer = cardLine.cooldown;
                }
            }
        }
        
    }

    /// <summary>
    /// 使用效果卡
    /// </summary>
    public virtual void OnUseEffectCard()
    {
        //遍历line
        foreach (EnemyCardLine cardLine in effectCardLineList)
        {
            //找到cd到了且不为空的line
            if (cardLine.timer <= 0 && cardLine.cards.Count > 0)
            {
                //找出cd到了的ecwt
                List<EnemyCardWithTimer> availableECWT = new List<EnemyCardWithTimer>();
                foreach (EnemyCardWithTimer ecwt in cardLine.cards)
                {
                    if (ecwt.timer <= 0)
                    {
                        availableECWT.Add(ecwt);
                    }
                }

                //随机抽卡
                if (availableECWT.Count > 0)
                {
                    int index = UnityEngine.Random.Range(0, availableECWT.Count);
                    EnemyCardWithTimer ecwt = availableECWT[index];
                    Card card = ecwt.card;
                    card.EnemyHasEffectOnPlayer(enemyName);

                    //冷却时间
                    ecwt.timer = ecwt.cooldown;
                    cardLine.timer = cardLine.cooldown;
                }
            }
        }
    }

    /// <summary>
    /// 死亡
    /// </summary>
    public override void OnDeath()
    {
        base.OnDeath();

        //掉落物品
        foreach (DropItem dropItem in dropItems)
        {
            if (dropItem != null)
            {
                GameObject instance = Instantiate(dropItem.gameObject);
                instance.transform.position = transform.position;
            }
        }

        //经验
        player.AddExp(exp);

        //flag
        if (!refreshable)
        {
            ArchiveManager.CheckFlag(flag, true, true);
        }
    }


    /// <summary>
    /// 展示血条
    /// </summary>
    /// <param name="feather"></param>
    public override void ShowUnlockFeather(Feather feather)
    {
        base.ShowUnlockFeather(feather);
        //Debug.Log("hpscroll:" + hpScroll);
        if (hpScroll != null)
        {
            if (!hpScroll.gameObject.IsDestroyed())
            {
                hpScroll.ClearAllContent();

                foreach (var item in unlockedFeathers)
                {
                    //Debug.Log("hpscroll:" +enemyName);
                    HpUI hpUI = hpScroll.AddHp();
                    hpUI.testTime = item.lockTimer;
                    hpUI.testHp = item.health;
                    hpUI.testHpMax = item.maxHealth;
                    item.hpUI = hpUI;
                    hpUI.targetFeather = item;
                }
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
