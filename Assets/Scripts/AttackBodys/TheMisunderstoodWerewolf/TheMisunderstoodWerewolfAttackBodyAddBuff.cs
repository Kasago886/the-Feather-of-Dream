using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TheMisunderstoodWerewolfAttackBodyAddBuff : MonoBehaviour
{
    public string buffName;
    public List<int> number = new List<int>();
    private TheMisunderstoodWerewolfAttackBody theAttackBody;
    private bool late;
    private void Update()
    {
        if(theAttackBody == null)
        {
            theAttackBody = GameObject.Find("TheMisunderstoodWerewolfAttackBody 1(Clone)").GetComponent<TheMisunderstoodWerewolfAttackBody>();
        }
        if (late && theAttackBody != null)
        {
            theAttackBody.next.Add(new AttackBuffDict(buffName, number));
            late = false;
        }
    }
    public void AddBuff()
    {
        if (theAttackBody != null)
        {
            theAttackBody.next.Add(new AttackBuffDict(buffName, number));
        }
        else
        {
            late = true;
        }
    }
}
