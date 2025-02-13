using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HunterFeatherBullet : MonoBehaviour
{
    private Enemy Enemy;
    public void OnHit()
    {
        Collider2D myCollider = GetComponent<Collider2D>();
        Bounds bounds = myCollider.bounds;

        // 检测矩形区域内的所有2D碰撞体
        Collider2D[] colliders = Physics2D.OverlapBoxAll(bounds.center,bounds.size,0f);

        foreach (var hitCollider in colliders)
        {
            Debug.Log("7");
            if (hitCollider.gameObject != gameObject && hitCollider.gameObject.GetComponent<Enemy>())
            {
                Enemy = hitCollider.GetComponent<Enemy>();
                Debug.Log("6");
            }
            if (Enemy != null)
            {
                Enemy.AddBuff("中毒");
            }
        }
    }
}
