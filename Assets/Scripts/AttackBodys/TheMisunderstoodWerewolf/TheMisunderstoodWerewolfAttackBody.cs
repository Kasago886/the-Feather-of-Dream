using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TheMisunderstoodWerewolfAttackBody : MonoBehaviour
{
    public static TheMisunderstoodWerewolfAttackBody instance;
    public Dictionary<string, List<int>> next;
    private AttackBody attackBody;
    private Player player;
    void Start()
    {
        next = new Dictionary<string, List<int>>();
        attackBody = GetComponentInParent<AttackBody>();
        player = GameObject.FindGameObjectWithTag(Consts.PlayerTag).GetComponent<Player>();
    }

    // Update is called once per frame
    void Update()
    {
        if (attackBody != null && attackBody.immediateAttack && player != null && next.Count > 0)
        {
            foreach (var item in next)
            {
                for (int i = 0; i < item.Value[0]; i++)
                {
                    player.AddBuff(item.Key);
                }
                item.Value.Remove(0);
                if(item.Value.Count == 0)
                {
                    next.Remove(item.Key);
                }
            }
        }
    }
}
