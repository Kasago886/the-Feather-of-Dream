using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class testDamageToEnemy : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag(Consts.EnemyTag))
        {
            Enemy enemy = collision.GetComponent<Enemy>();
            if (enemy != null)
            {
                enemy.UnlockFeather(enemy.feathers.Count, 100);
                enemy.TakeDamage(1000);
            }
        }
    }
}
