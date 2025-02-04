using JetBrains.Annotations;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEditor.Progress;
public class AttackBuffDict
{
    public string buffName;
    public List<int> next=new List<int>();
    public AttackBuffDict(string buffName,List<int> next)
    {
        this.buffName = buffName;
        this.next = next;
    }
}
public class TheMisunderstoodWerewolfAttackBody : MonoBehaviour
{
    public static List<AttackBuffDict> next = new List<AttackBuffDict>();
    private Player player;
    void Start()
    { 
        player = GameObject.FindGameObjectWithTag(Consts.PlayerTag).GetComponent<Player>();
    }
    public void AddBuff()
    {
        Invoke("AddBuff1", 0.1f);
    }
    private void AddBuff1()
    {
        if (player != null)
        {
            foreach (var item in next)
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
            for (int i = 0;i < next.Count;i++)
            {
                if (next[i].next.Count == 0)
                {
                    next.RemoveAt(i);
                    i--;
                }              
            }
        }
    }
}
