using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Dwarfs : Enemy
{
    public int DwarfsNumber;
    public override void AIUpdate()
    {
        base.AIUpdate();
        DwarfsNumber = 0;
        Collider2D[] DwarfsList = Physics2D.OverlapAreaAll(new Vector2(transform.position.x - 5, transform.position.y - 5), new Vector2(transform.position.x + 5, transform.position.y + 5), LayerMask.GetMask(Consts.EnemyLayer));
        foreach (Collider2D collider in DwarfsList)
        {
            if (collider.GetComponent<Dwarfs>())
            {
                DwarfsNumber++;
            }
        }
    }
}
