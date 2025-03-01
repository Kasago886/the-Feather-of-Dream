using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NeprizBullet : MonoBehaviour
{
    public bool effectOnPlayer;
    public bool effectOnEnemy;
    public float damage;
    public List<string> buffs;
    private void Start()
    {
        Destroy(gameObject,10f);
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(effectOnPlayer && (collision.gameObject.tag==Consts.PlayerTag||collision.GetComponent<Player>()!=null))
        {
            Player player = collision.gameObject.GetComponent<Player>();
            player.TakeDamage(damage);
            for (int i = 0; i < buffs.Count; i++)
            {
                player.AddBuff(buffs[i]);
            }
            Destroy(gameObject);
        }
        if (effectOnEnemy&&collision.gameObject.tag == Consts.EnemyTag)
        {
            Enemy enemy = collision.gameObject.GetComponent<Enemy>();
            enemy.TakeDamage(damage);
            for (int i = 0; i < buffs.Count; i++)
            {
                enemy.AddBuff(buffs[i]);
            }
            Destroy(gameObject);
        }
    }
}
