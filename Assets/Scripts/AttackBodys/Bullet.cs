using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

[RequireComponent(typeof(CircleCollider2D))]
public class Bullet : MonoBehaviour
{
    public bool isEnemyBullet;
    public float speed;
    public int bounceTime;

    public bool autoDestroy = false;
    public float destroyTimer = 10;

    public bool isSplit = false;
    public GameObject splitBullet;

    public bool ignoreWallCollision;
    public Vector3 destroyCenter;
    public Vector2 destroySize;

    public Vector2 direction;
    public UnityEvent whatHappenWhenHit;
    [HideInInspector] public float damage;

    Rigidbody2D rb;

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
                collision.GetComponent<Enemy>().TakeDamage(damage);
                whatHappenWhenHit?.Invoke();
                Destroy(gameObject);
            }
        }
    }
}
