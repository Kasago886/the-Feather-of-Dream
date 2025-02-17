using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class NeprizBullet1 : MonoBehaviour
{
    public static bool isAddBuff;
    public static Dictionary<string, int> buffNameAndNumber=new Dictionary<string, int>();
   public void AddBuff()
    {
        if (isAddBuff&& GameObject.FindGameObjectWithTag(Consts.PlayerTag) != null)
        {
            Player player=GameObject.FindGameObjectWithTag(Consts.PlayerTag).GetComponent<Player>();
            foreach (var item in buffNameAndNumber)
            {
                for(int i = 0; i < item.Value; i++)
                {
                    player.AddBuff(item.Key);
                }
                buffNameAndNumber.Remove(item.Key);
            }
            isAddBuff=false;
        }
    }
}
