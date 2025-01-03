using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public enum AttackType
{
    Melee,Ranged
}

public class AttackBody : MonoBehaviour
{
    public bool isEnemy;
    public float damage;

    public AttackType attackType;

    public Vector2 attackCenter;
    public Vector2 attackRegion;

    [HideInInspector]public bool isleft;
    [HideInInspector] public float addDamage = 0;

    // Start is called before the first frame update
    void Start()
    {
        //攻击
        Collider2D[] targets = GetTargetsInAttackRegion(transform.position,isleft);
        foreach (Collider2D target in targets)
        {
            target.GetComponent<Character>().TakeDamage(damage + addDamage, transform);
        }
    }

    // Update is called once per frame
    void Update()
    {
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

        return null;
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
    }
}
