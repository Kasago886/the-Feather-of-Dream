using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NeprizBullet1Controller : MonoBehaviour
{
    public bool b,b1;
    private void Update()
    {
        if (GameObject.Find("NeprizAttackBody(Clone)") != null)
        {
            if (b1)
            {
                AttackBody attackBody = GameObject.Find("NeprizAttackBody(Clone)").GetComponent<AttackBody>();
                attackBody.bullet = Resources.Load<GameObject>("AttackBodys/Nepriz/NeprizBullet 2");
            }
            if (b)
            {
                AttackBody attackBody = GameObject.Find("NeprizAttackBody(Clone)").GetComponent<AttackBody>();
                attackBody.bullet = Resources.Load<GameObject>("AttackBodys/Nepriz/NeprizBullet 1");
            }
            if(!b&&!b1) 
            {
                AttackBody attackBody = GameObject.Find("NeprizAttackBody(Clone)").GetComponent<AttackBody>();
                attackBody.bullet = Resources.Load<GameObject>("AttackBodys/Nepriz/NeprizBullet");
            }
        }
    }
    public void AddBuff()
    {
        Debug.Log("ADD Buff");
        b = true;
    }
    public void AddBuff1()
    {
        Debug.Log("ADD Buff");
        b1 = true;
    }
}
