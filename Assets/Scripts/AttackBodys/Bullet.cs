using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Events;

[RequireComponent(typeof(CircleCollider2D))]
public class Bullet : MonoBehaviour
{
    public bool isEnemyBullet;
    public float speed;

    public bool autoDestroy = false;
    public float destroyTimer = 10;

    public bool isSplit = false;
    public GameObject splitBullet;
    public int bounceTime;

    public bool ignoreWallCollision;
    public Vector3 destroyCenter;
    public Vector2 destroySize;

    public Vector2 direction;
    public UnityEvent whatHappenWhenHit;
    [HideInInspector] public float damage;

    Rigidbody2D rb;
    Enemy collisionEnemy;

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        rb.velocity = direction.normalized * speed;
    }

    // Update is called once per frame
    void Update()
    {
        if (autoDestroy)
        {
            destroyTimer -= Time.deltaTime;
            if (destroyTimer <= 0)
            {
                Destroy(gameObject);
            }
        }
        if (ignoreWallCollision)
        {
            Vector2 skew = transform.position - destroyCenter;
            if (Mathf.Abs(skew.x) > destroySize.x / 2 || Mathf.Abs(skew.y) > destroySize.y / 2)
            {
                Destroy(gameObject);
            }
        }
    }

    public void AddBuffToTarget(string buffName)
    {
        if (isEnemyBullet)
        {
            Player player = GetComponent<Player>();
            player.AddBuff(buffName);
        }
        else
        {
            collisionEnemy.AddBuff(buffName);
            Debug.Log(buffName);
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision)
        {
            //Debug.Log(collision);
            if (LayerMask.LayerToName(collision.gameObject.layer) == Consts.WallLayer && !ignoreWallCollision)
            {
                bounceTime--;
                if (bounceTime < 0)
                {
                    Destroy(gameObject);
                }
                else if (isSplit)
                {
                    GameObject instance = Instantiate(splitBullet, transform, false);
                    instance.transform.localPosition = Vector3.zero;
                    Bullet b = instance.GetComponent<Bullet>();
                    b.direction = Quaternion.Euler(0,0,Random.Range(-20f,20f)) * direction;
                    b.bounceTime = bounceTime;
                }
            }
            else if (collision.tag == Consts.PlayerTag && isEnemyBullet)
            {
                collision.GetComponent<Player>().TakeDamage(damage);
                whatHappenWhenHit?.Invoke();
                Destroy(gameObject);
            }
            else if (collision.tag == Consts.EnemyTag && !isEnemyBullet)
            {
                collisionEnemy = collision.GetComponent<Enemy>();
                if (!collisionEnemy.isDead)
                {
                    collisionEnemy.TakeDamage(damage);
                    Debug.Log("takedamage:" + collisionEnemy.enemyName);
                    whatHappenWhenHit?.Invoke();
                    Destroy(gameObject);
                }
            }
        }
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.yellow;

        Gizmos.DrawLine(destroyCenter + new Vector3(-destroySize.x / 2, destroySize.y / 2, 0), destroyCenter + new Vector3(destroySize.x / 2, destroySize.y / 2, 0));
        Gizmos.DrawLine(destroyCenter + new Vector3(-destroySize.x / 2, -destroySize.y / 2, 0), destroyCenter + new Vector3(destroySize.x / 2, -destroySize.y / 2, 0));
        Gizmos.DrawLine(destroyCenter + new Vector3(destroySize.x / 2, destroySize.y / 2, 0), destroyCenter + new Vector3(destroySize.x / 2, -destroySize.y / 2, 0));
        Gizmos.DrawLine(destroyCenter + new Vector3(-destroySize.x / 2, destroySize.y / 2, 0), destroyCenter + new Vector3(-destroySize.x / 2, -destroySize.y / 2, 0));
    }
}
