using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CrazyHunter : Enemy
{
    public bool attackWay;
    public override void OnMove(float horizontalMove)
    {
        base.OnMove(horizontalMove);
        
        switch (searchType)
        {
            //圆形视野
            case EnemySearchType.distance:
                if (Vector2.Distance(transform.position, player.transform.position) < searchDistance/2)
                {
                  attackWay = true;
                }
                else
                {
                    attackWay=false;
                }
                break;

            //水平距离视野
            case EnemySearchType.horizontal:
                if (Mathf.Abs(transform.position.x - player.transform.position.x) < searchDistance/2)
                {
                    attackWay = true;
                }
                else
                {
                    attackWay = false;
                }
                break;
        }
    }
}
