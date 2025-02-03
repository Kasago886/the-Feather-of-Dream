using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TheMisunderstoodWerewolfAttackBodyAddBuff : MonoBehaviour
{
    public string buffName;
    public int[] number;
    private bool late;
    private void Update()
    {
        if (late && GameObject.Find("TheMisunderstoodWerewolfAttackBody 1(Clone)") != null)
        {
            List<int> list = new List<int>();
            for (int i = 0; i < number.Length; i++)
            {
                list.Add(number[i]);
            }
            Debug.Log("AddBuff1113");
            TheMisunderstoodWerewolfAttackBody.next.Add(new AttackBuffDict(buffName,list));
            late = false;
        }
    }
    public void AddBuff()
    {
        List<int> list = new List<int>();
        for (int i = 0; i < number.Length; i++)
        {
            list.Add(number[i]);
        }
        if (GameObject.Find("TheMisunderstoodWerewolfAttackBody 1(Clone)") != null)
        {
            Debug.Log("AddBuff1112");
            TheMisunderstoodWerewolfAttackBody.next.Add(new AttackBuffDict(buffName, list));
        }
        else
        {
            late = true;
        }
    }
}
