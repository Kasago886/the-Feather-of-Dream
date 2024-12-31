using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum EnemySearchType
{
    distance,horizontal,infinity
}

public enum EnemyStateType
{
    Idle,Chase,Attack,Injury
}

public class Enemy : Character
{
    public float runSpeed;
    public float jumpSpeed;

    public EnemySearchType searchType;
    public bool wallDetect;

    public float searchDistance;

    public float attackCardUseDistance;
    public float attackCardCooldown;

    public float effectCardUseDistance;
    public float effectCardCooldown;

    [HideInInspector] public Player player;
    public Rigidbody2D rb;

    //states
    Dictionary<EnemyStateType, EnemyState> stateDict = new();
    EnemyState currentState;

    new protected void Start()
    {
        base.Start();
        player = FindAnyObjectByType<Player>();
        rb = GetComponent<Rigidbody2D>();

        stateDict[EnemyStateType.Idle] = new EnemyIdleState(this);
        stateDict[EnemyStateType.Chase] = new EnemyChaseState(this);

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
    /// 更新AI
    /// </summary>
    public override void AIUpdate()
    {
        base.AIUpdate();

        currentState.OnUpdate();
    }

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

        switch (searchType)
        {
            //圆形视野
            case EnemySearchType.distance:
                if (Vector2.Distance(transform.position, player.transform.position) < searchDistance)
                {
                    return true;
                }
                break;

            //水平距离视野
            case EnemySearchType.horizontal:
                if (Mathf.Abs(transform.position.x - player.transform.position.x) < searchDistance)
                {
                    return true;
                }
                break;

            //无穷视野
            case EnemySearchType.infinity:
                return true;
        }

        return false;
    }

    /// <summary>
    /// 移动
    /// </summary>
    /// <param name="horizontalMove"></param>
    public void OnMove(float horizontalMove)
    {
        float speed = runSpeed * horizontalMove;
        rb.velocity = new Vector2(speed, rb.velocity.y);
        //Debug.Log(rb.velocity);
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.yellow;

        switch (searchType)
        {
            //圆形视野
            case EnemySearchType.distance:
                int side = 20;
                float angle = 360f / side;
                for (int i = 0; i < side; i++)
                {
                    Vector2 from = transform.position + Quaternion.Euler(0, 0, angle * i) * Vector2.right * searchDistance;
                    Vector2 to = transform.position + Quaternion.Euler(0,0,angle * (i+1)) * Vector2.right * searchDistance;

                    Gizmos.DrawLine(from,to);
                }

                break;

            //水平距离视野
            case EnemySearchType.horizontal:
                Gizmos.DrawLine(new Vector2(transform.position.x - searchDistance, transform.position.y + 50), new Vector2(transform.position.x - searchDistance, transform.position.y - 50));
                Gizmos.DrawLine(new Vector2(transform.position.x + searchDistance, transform.position.y + 50), new Vector2(transform.position.x + searchDistance, transform.position.y - 50));
                Gizmos.DrawLine(new Vector2(transform.position.x + searchDistance, transform.position.y), new Vector2(transform.position.x - searchDistance, transform.position.y));

                break;

            //无穷视野
            case EnemySearchType.infinity:
                player = FindAnyObjectByType<Player>();
                Gizmos.DrawLine(transform.position,player.transform.position);
                break;
        }
    }
}
