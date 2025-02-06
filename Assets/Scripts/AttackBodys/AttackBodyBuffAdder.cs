using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum User
{
    玩家,
    敌人
}
public enum DUser
{
    玩家,
    敌人,
    双方
}
public class AttackBodyBuffAdder : MonoBehaviour
{
    [Header("使用者")]
    public User user;
    [HideInInspector]
    public List<AttackBuffDict> nextToPlayer = new List<AttackBuffDict>();
    [HideInInspector]
    public List<AttackBuffDict> nextToEnemy = new List<AttackBuffDict>();
    [HideInInspector]
    public Enemy enemy;
    [HideInInspector]
    public Player player;
    void Start()
    {
        if (user == User.敌人)
        {
            player = GameObject.FindGameObjectWithTag(Consts.PlayerTag).GetComponent<Player>();
        }
    }
    private void Update()
    {
        if (user == User.玩家  && GetComponent<AttackBody>().whatBeAttack.Count>0&& GetComponent<AttackBody>().whatBeAttack[0].GetComponent<Enemy>()!=null)
        {
            enemy= GetComponent<AttackBody>().whatBeAttack[0].GetComponent<Enemy>();
        }
    }
    public void AddBuff()
    {
        Invoke("AddBuff1", 0.1f);
    }
    private void AddBuff1()
    {
        if (player != null)
        {
            foreach (var item in nextToPlayer)
            {
                if (item.next.Count > 0)
                {
                    for (int i = 0; i < item.next[0]; i++)
                    {
                        player.AddBuff(item.buffName);
                    }
                    item.next.RemoveAt(0);
                }
            }
            for (int i = 0; i < nextToPlayer.Count; i++)
            {
                if (nextToPlayer[i].next.Count == 0)
                {
                    nextToPlayer.RemoveAt(i);
                    i--;
                }
            }
        }
        if (enemy != null)
        {
            foreach (var item in nextToEnemy)
            {
                if (item.next.Count > 0)
                {
                    for (int i = 0; i < item.next[0]; i++)
                    {
                        enemy.AddBuff(item.buffName);
                    }
                    item.next.RemoveAt(0);
                }
            }
            for (int i = 0; i < nextToEnemy.Count; i++)
            {
                if (nextToEnemy[i].next.Count == 0)
                {
                    nextToEnemy.RemoveAt(i);
                    i--;
                }
            }
        }
    }
}

