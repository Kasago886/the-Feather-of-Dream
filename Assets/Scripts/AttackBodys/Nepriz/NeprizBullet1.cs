using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class NeprizBullet1 : MonoBehaviour
{
    public List<BuffNameAndNumber> buffNameAndNumber=new List<BuffNameAndNumber>();
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag == Consts.PlayerTag)
        {
            Player player= collision.gameObject.GetComponent<Player>();
            foreach (var buff in buffNameAndNumber)
            {
               for (int i = 0; i < buff.number.Count; i++)
                {
                    player.AddBuff(buff.name);
                }
            }
            Destroy(GameObject.Find("NeprizBullet 1(Clone)"));
            if (GameObject.Find("NeprizAttackBody(Clone)") != null)
            {
                AttackBody attackBody = GameObject.Find("NeprizAttackBody(Clone)").GetComponent<AttackBody>();
                attackBody.bullet = Resources.Load<GameObject>("AttackBodys/Nepriz/NeprizBullet.prefab");
            }
        }
    }
}
