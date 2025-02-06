using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UIElements;

public enum AttackType
{
    Melee, Gun, Child
}

public class AttackBody : MonoBehaviour
{
    public bool isEnemy;
    public bool immediateAttack;
    public float damage;

    public AttackType attackType;

    public Vector2 attackCenter;
    public Vector2 attackRegion;

    public GameObject bullet;
    public bool isAiming;

    public UnityEvent whatHappenWhenAttack;
    [HideInInspector]
    public List<GameObject> whatBeAttack;

    [HideInInspector]public bool isleft;
    [HideInInspector] public float addDamage = 0;

    Vector3 center;

    // Start is called before the first frame update
    void Start()
    {
        //攻击中心的绝对位置
        center = attackCenter;
        if (isleft)
        {
            center.x = -attackCenter.x;
            transform.localScale = new Vector3(-transform.localScale.x,transform.localScale.y,transform.localScale.z);
        }
        center = center + transform.position;

        if (immediateAttack)
        {
            OnAttack();
        }
    }

    // Update is called once per frame
    void Update()
    {
    }

    public void OnAttack()
    {
        //攻击
        switch (attackType)
        {
            //近战
            case AttackType.Melee:
                Collider2D[] targets = GetTargetsInAttackRegion(transform.position, isleft);
                foreach (Collider2D target in targets)
                {
                    target.GetComponent<Character>().TakeDamage(damage + addDamage, transform);
                    whatHappenWhenAttack?.Invoke();
                    whatBeAttack.Add(target.gameObject);
                }
                Invoke("ClearTarget", 0.2f);
                break;

            //发射子弹
            case AttackType.Gun:
                GameObject bulletObj = Instantiate(bullet);
                bulletObj.transform.position = center;

                Bullet b = bulletObj.GetComponent<Bullet>();
                b.damage = damage + addDamage;

                //是否瞄准
                if (isAiming)
                {
                    Vector2 direction = GetTargetDirection(transform.position, isleft);
                    b.direction = direction;
                }
                else
                {
                    if (isleft)
                    {
                        b.direction = Vector2.left;
                    }
                    else
                    {
                        b.direction = Vector2.right;
                    }
                }

                break;
        }
    }

    /// <summary>
    /// 获取攻击范围内的目标
    /// </summary>
    /// <param name="position">attackBody位置</param>
    /// <param name="isleft">面朝方向</param>
    /// <returns></returns>
    public Collider2D[] GetTargetsInAttackRegion(Vector3 position, bool isleft)
    {
        Vector3 center = attackCenter;
        if (isleft)
        {
            center.x = -attackCenter.x;
        }
        center = center + position;

        //有限视距
        if (attackType == AttackType.Melee)
        {

            //判断敌我
            if (isEnemy)
            {
                Collider2D[] players = Physics2D.OverlapBoxAll(center, attackRegion, 0f, LayerMask.GetMask(Consts.PlayerLayer));
                return players;
            }
            else
            {
                Collider2D[] enemys = Physics2D.OverlapBoxAll(center, attackRegion, 0f, LayerMask.GetMask(Consts.EnemyLayer));
                return enemys;
            }
        }
        //无限视距
        else
        {
            if (isEnemy)
            {
                Collider2D player = GameObject.FindGameObjectWithTag(Consts.PlayerTag).GetComponent<Collider2D>();

                return new Collider2D[] { player };
            }
            else
            {
                List<Collider2D> list = new();
                GameObject[] enemys = GameObject.FindGameObjectsWithTag(Consts.EnemyTag);
                foreach (GameObject enemy in enemys)
                {
                    list.Add(enemy.GetComponent<Collider2D>());
                }

                return list.ToArray();
            }
        }
    }

    /// <summary>
    /// 计算目标方向
    /// </summary>
    /// <param name="position"></param>
    /// <param name="isleft"></param>
    /// <returns></returns>
    Vector2 GetTargetDirection(Vector3 position, bool isleft)
    {
        Vector3 center = attackCenter;
        if (isleft)
        {
            center.x = -attackCenter.x;
        }
        center = center + position;

        Collider2D[] targets = GetTargetsInAttackRegion(position, isleft);
        if (targets.Length == 0)
        {
            return Vector2.zero;
        }
        else
        {
            //找到最近的目标
            Collider2D target = targets[0];
            foreach(Collider2D collider in targets)
            {
                if (Vector2.Distance(center, collider.transform.position) < Vector2.Distance(center, target.transform.position))
                {
                    target = collider;
                }
            }

            return target.transform.position - center;
        }
    }

    public void Destroy()
    {
        Destroy(gameObject);
    }

    private void OnDrawGizmos()
    {
        Vector3 center = attackCenter;
        if (isleft)
        {
            center.x = -attackCenter.x;
        }
        center = center + transform.position;

        if (isEnemy)
        {
            Gizmos.color = Color.red;
        }
        else
        {
            Gizmos.color = Color.yellow;
        }

        if (attackType == AttackType.Melee)
        {
            Gizmos.DrawWireCube(center, attackRegion);
        }
        else if (attackType == AttackType.Gun)
        {
            Gizmos.DrawCube(center, new Vector3(0.1f,0.1f));

            if (isAiming)
            {
                Gizmos.DrawRay(center, GetTargetDirection(transform.position, isleft));
            }
            else
            {
                if(isleft)
                {
                    Gizmos.DrawRay(center, Vector3.left * 5);
                }
                else
                {
                    Gizmos.DrawRay(center, Vector3.right * 5);
                }
            }
        }
    }
    private void ClearTarget()
    {
        if(whatBeAttack!=null)
        {
            whatBeAttack.Clear();
        }
    }
}
