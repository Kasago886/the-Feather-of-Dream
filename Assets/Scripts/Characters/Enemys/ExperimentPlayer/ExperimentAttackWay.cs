using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class ExperimentAttackWay : MonoBehaviour
{
    private Player player;
    private Rigidbody2D rb;
    private float timer;
    private float timer2;
    public int attackWay;
    Vector2 dirction;
    GameObject childAttack;
    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        player = FindAnyObjectByType<Player>();
        Init();
        childAttack = Resources.Load<GameObject>("AttackBodys/ExperimentPlayer/ExperimentPlayerChildAttack");
    }
    private void Update()
    {
        if (timer > 0)
        {
            timer -= Time.deltaTime;
        }
        if (timer2 > 0)
        {
            timer2 -= Time.deltaTime;
        }
        if (attackWay == 1)
        {
            rb.gravityScale = 0;
            if (timer <-1)
            {
                timer = 0.15f;
            }
            if (timer > 0)
            {
                gameObject.transform.position += new Vector3(0, Time.deltaTime * 40, 0);
            }
            if (timer < 0 && timer > -1 && timer2 < -1)
            {
                timer2 = 0.9f;
                timer = -2;
            }
            if (timer2 > 0&&timer2<=0.6f)
            {
                if (dirction == new Vector2(0, 0))
                {
                    dirction = player.transform.position-new Vector3(0,10,0) - gameObject.transform.position;
                }
                rb.AddForce(dirction.normalized, ForceMode2D.Impulse);

                if (Vector3.Distance(player.transform.position, gameObject.transform.position) < 2)
                {
                    if (player.unlockedFeathers.Count != 0)
                    {
                        player.unlockedFeathers[0].health -= 50;
                        Init();
                    }

                }
            }
            if (timer2 <= 0 && timer2 > -1)
            {
                Init();
            }
        }
        if (attackWay == 2)
        {
            Instantiate<GameObject>(childAttack,new Vector3(-3.55f,8.42f,0),Quaternion.identity);
            Instantiate<GameObject>(childAttack,new Vector3(-11.98f,8.94f,0),Quaternion.identity);
            attackWay = -1;
        }
    }
    private void Init()
    {
        timer2 = -2;
        timer = -2;
        attackWay = -1;
        rb.gravityScale = 1;
        rb.velocity = new Vector2(0, 0);
        dirction = new Vector2(0, 0);
    }
}