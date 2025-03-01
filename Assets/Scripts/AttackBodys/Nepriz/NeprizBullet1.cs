using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class NeprizBullet1 : MonoBehaviour
{
    public List<BuffNameAndNumber> buffNameAndNumber = new List<BuffNameAndNumber>();
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if ( collision.gameObject.tag == Consts.PlayerTag)
        {
            Debug.Log("Collider is Player");
            Player player = collision.gameObject.GetComponent<Player>();
            foreach (var buff in buffNameAndNumber)
            {
                for (int i = 0; i < buff.number.Count; i++)
                {
                    player.AddBuff(buff.name);
                }
            }
            if(FindAnyObjectByType<NeprizAttackBodyController>() != null)
            {
                Debug.Log("!null");
            }
            FindAnyObjectByType<NeprizBullet1Controller>().b = false;
            FindAnyObjectByType<NeprizBullet1Controller>().b1 = false;
            if (FindAnyObjectByType<Nepriz2>() != null) 
            {
                Nepriz2 nepriz2 = FindAnyObjectByType<Nepriz2>();
                if (nepriz2.mediocreNumber.Count > 0)
                {
                    player.AddBuff("·²Ó¹");
                    nepriz2.buffList[nepriz2.mediocreNumber[0]].timer = 0;
                }
            }
            Destroy(GameObject.Find(gameObject.name));
        }
    }
}
