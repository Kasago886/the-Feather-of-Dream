using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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
        if (isleft)
        {
            attackCenter.x = -attackCenter.x;
        }

        if (attackType == AttackType.Melee)
        {
            Vector3 center = attackCenter;
            center = center + transform.position;

            //≈–∂œµ–Œ“
            if (isEnemy)
            {
                Collider2D[] players = Physics2D.OverlapBoxAll(center, attackRegion, 0f, LayerMask.GetMask(Consts.PlayerLayer));
                foreach (Collider2D player in players)
                {
                    //Debug.Log(enemy);
                    player.GetComponent<Character>().TakeDamage(damage + addDamage);
                }
            }
            else
            {
                Collider2D[] enemys = Physics2D.OverlapBoxAll(center, attackRegion, 0f, LayerMask.GetMask(Consts.EnemyLayer));
                foreach (Collider2D enemy in enemys)
                {
                    //Debug.Log(enemy);
                    enemy.GetComponent<Character>().TakeDamage(damage + addDamage);
                }
            }
        }
    }

    // Update is called once per frame
    void Update()
    {
    }

    public void Destroy()
    {
        Destroy(gameObject);
    }

    private void OnDrawGizmos()
    {
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
            Vector3 center = attackCenter;

            Gizmos.DrawWireCube(transform.position + center, attackRegion);
        }
    }
}
