using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NeprizBullet1Controller : MonoBehaviour
{
    private bool b;
    private void Update()
    {
        if (GameObject.Find("NeprizAttackBody(Clone)") != null)
        {
            AttackBody attackBody = GameObject.Find("NeprizAttackBody(Clone)").GetComponent<AttackBody>();
            attackBody.bullet = Resources.Load<GameObject>("AttackBodys/Nepriz/NeprizBullet 1.prefab");
            b =false;
        }
    }
    public void AddBuff()
    {
        if (GameObject.Find("NeprizAttackBody(Clone)") != null)
        {
            AttackBody attackBody=GameObject.Find("NeprizAttackBody(Clone)").GetComponent<AttackBody>();
            attackBody.bullet = Resources.Load<GameObject>("AttackBodys/Nepriz/NeprizBullet 1.prefab");
        }
    }
}
