using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HunterFeatherAttack : MonoBehaviour
{
    private Player player;
    private float strength;
    public void OnAttack()
    {
        player = GameObject.FindGameObjectWithTag(Consts.PlayerTag).GetComponent<Player>();
        foreach (GameObject attackBody in player.attackBodyObjList)
        {
            if (attackBody.name == "HunterFeatherAttackBody")
            {
                strength = attackBody.GetComponent<AttackBody>().damage;
            }
            if(player.unlockedFeathers.Count > 0) 
            player.unlockedFeathers[0].health += strength / 2;
        }
    }
}