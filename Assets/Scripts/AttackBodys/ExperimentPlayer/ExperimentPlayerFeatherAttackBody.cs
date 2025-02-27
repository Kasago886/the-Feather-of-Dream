using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExperimentPlayerFeatherAttackBody : MonoBehaviour
{
  public void OnHit()
    {
        Player player = GameObject.Find("Player").GetComponent<Player>();
        if (player.buffList.Exists(buff => buff.name == "รฮิด"))
        {
            player.RemoveBuff("รฮิด");
            if (player.unlockedFeathers.Count != 0)
            {
                player.unlockedFeathers[0].health += player.unlockedFeathers[0].maxHealth/10;
                if (player.unlockedFeathers[0].health > player.unlockedFeathers[0].maxHealth)
                {
                    player.unlockedFeathers[0].health = player.unlockedFeathers[0].maxHealth;
                }
            }
        }
    }
}
