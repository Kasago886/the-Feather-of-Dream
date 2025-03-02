using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BCard : Card
{
    public override bool ConditionsOfUseCard()
    {
        if (Vector2.Distance(transform.parent.transform.position, GameObject.FindGameObjectWithTag(Consts.PlayerTag).transform.position)<50)
        {
            return true;
        }
        return false;
    }
}
