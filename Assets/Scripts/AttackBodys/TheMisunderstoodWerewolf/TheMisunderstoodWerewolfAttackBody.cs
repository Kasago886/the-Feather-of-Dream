using JetBrains.Annotations;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
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
    private AttackBody attackBody;
    private Player player;
    void Start()
    { 
        attackBody = GetComponentInParent<AttackBody>();
        player = GameObject.FindGameObjectWithTag(Consts.PlayerTag).GetComponent<Player>();
    }
    public void AddBuff()
    {
        Debug.Log("AddBuff"+next.Count);
        Invoke("AddBuff1", 0.1f);
    }
    private void AddBuff1()
    {
        foreach (var item in next)
        {
            for (int i = 0; i < item.next[0]; i++)
            {
                player.AddBuff(item.buffName);
            }
            item.next.Remove(0);
            if (item.next.Count == 0)
            {
                next.Remove(item);
            }
        }
    }
}
