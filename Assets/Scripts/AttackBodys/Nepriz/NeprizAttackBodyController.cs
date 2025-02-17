using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEngine.RuleTile.TilingRuleOutput;

public class NeprizAttackBodyController : MonoBehaviour
{
    public void AddAttackBody1()
    {
        if (GameObject.FindGameObjectWithTag(Consts.PlayerTag) != null)
        {
            Instantiate(Resources.Load<GameObject>("AttackBodys/Nepriz/Nepriz1Follwer"),new Vector3(GameObject.FindGameObjectWithTag(Consts.PlayerTag).transform.position.x, Camera.main.transform.position.y + Camera.main.orthographicSize + 10,0),Quaternion.identity);
        }
    }
}
