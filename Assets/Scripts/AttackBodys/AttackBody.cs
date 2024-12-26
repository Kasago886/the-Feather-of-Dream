using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum AttackType
{
    Melee,Ranged
}

public class AttackBody : MonoBehaviour
{
    public float damage;

    public AttackType attackType;

    public Vector2 attackCenter;
    public Vector2 attackRegion;

    // Start is called before the first frame update
    void Start()
    {
        if (attackType == AttackType.Melee)
        {
            Vector3 center = attackCenter;
            center = center + transform.position;

            Collider2D[] enemys = Physics2D.OverlapBoxAll(center, attackRegion,0f,LayerMask.GetMask(Consts.EnemyLayer));
            foreach (Collider2D enemy in enemys)
            {
                enemy.GetComponent<Character>().TakeDamage(damage);
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

    private void OnDrawGizmosSelected()
    {
        if (attackType == AttackType.Melee)
        {
            Vector3 center = attackCenter;

            Gizmos.color = Color.yellow;
            Gizmos.DrawWireCube(transform.position + center, attackRegion);
        }
    }
}
