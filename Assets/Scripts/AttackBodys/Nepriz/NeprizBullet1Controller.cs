using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NeprizBullet1Controller : MonoBehaviour
{
    public string buffName;
    public int number;
   public void AddBuff()
    {
        NeprizBullet1.isAddBuff = true;
        if(NeprizBullet1.buffNameAndNumber.ContainsKey(buffName))
        {
            NeprizBullet1.buffNameAndNumber[buffName] += number;
        }
        else
        {
            NeprizBullet1.buffNameAndNumber.Add(buffName, number);
        }
    }
}
