using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TheMisunderstoodWerewolfAttackBodyAddBuff : MonoBehaviour
{
    public string buffName;
    public int[] number;
   public void AddBuff()
    {
        List<int> list = new List<int>();
        for (int i = 0; i < number.Length; i++)
        {
            list.Add(number[i]);
        }
        if (TheMisunderstoodWerewolfAttackBody.instance != null)
        {
            TheMisunderstoodWerewolfAttackBody.instance.next.Add(buffName, list);
        }
    }
}
