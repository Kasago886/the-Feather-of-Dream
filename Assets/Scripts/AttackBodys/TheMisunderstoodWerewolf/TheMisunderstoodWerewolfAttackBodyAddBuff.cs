using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TheMisunderstoodWerewolfAttackBodyAddBuff : MonoBehaviour
{
    public string buffName;
    public List<int> number=new List<int>();
    private bool late;
    private void Update()
    {
        if (late && GameObject.Find("TheMisunderstoodWerewolfAttackBody 1(Clone)") != null)
        {
            TheMisunderstoodWerewolfAttackBody.next.Add(new AttackBuffDict(buffName,number));
            late = false;
        }
    }
    public void AddBuff()
    {
        if (GameObject.Find("TheMisunderstoodWerewolfAttackBody 1(Clone)") != null)
        {
            TheMisunderstoodWerewolfAttackBody.next.Add(new AttackBuffDict(buffName, number));
        }
        else
        {
            late = true;
        }
    }
}
