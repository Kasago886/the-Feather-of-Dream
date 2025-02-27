using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class BulletsRain : MonoBehaviour
{
    public GameObject bullet;
    public float damage;
    public Vector2 leftDownPos;
    public Vector2 size;
    public int column;
    public int row;
    public Vector2 speed;
    public float coolDown;
    public RainType rainType;

    float move = 0;
    float timer = 0;
    public enum RainType
    {
        pause,LtoR,RtoL,BothToBoth
    }
    // Start is called before the first frame update
    void Start()
    {
        if (size.x < 0)
        {
            size = new Vector2(0, size.y);
        }
        if (size.y < 0)
        {
            size = new Vector2(size.x, 0);
        }
        if (column == 0)
        {
            column = 1;
        }
        if (row == 0)
        {
            row = 1;
        }
        if (speed.x < 0)
        {
            speed = new Vector2(-speed.x, speed.y);
        }
        if (speed.y < 0)
        {
            speed = new Vector2(speed.x, -speed.y);
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (rainType == RainType.pause)
        {
            move = 0;
        }

        timer -= Time.deltaTime;
        if (timer < 0)
        {
            timer += coolDown;

            float xd = size.x / column;
            Vector2 movingSpeed;
            switch (rainType)
            {
                case RainType.pause:
                    move = 0;
                    break;
                case RainType.LtoR:
                    movingSpeed = new Vector2(speed.x, -speed.y);
                    for (int i = 0; i < column; i++)
                    {
                        GameObject bulletObj = Instantiate(bullet);
                        bulletObj.transform.position = leftDownPos + new Vector2(i * xd + move, size.y);

                        Bullet b = bulletObj.GetComponent<Bullet>();
                        b.damage = damage;
                        b.speed = movingSpeed.magnitude;

                        Vector2 direction = movingSpeed.normalized;
                        b.direction = direction;
                    }
                    move += speed.x * coolDown;
                    while (move > xd && xd > 0)
                    {
                        move -= xd;
                    }

                    break;
                case RainType.RtoL:
                    movingSpeed = new Vector2(-speed.x, -speed.y);
                    for (int i = 0; i < column; i++)
                    {
                        GameObject bulletObj = Instantiate(bullet);
                        bulletObj.transform.position = leftDownPos + new Vector2(i * xd + move, size.y);

                        Bullet b = bulletObj.GetComponent<Bullet>();
                        b.damage = damage;
                        b.speed = movingSpeed.magnitude;

                        Vector2 direction = movingSpeed.normalized;
                        b.direction = direction;
                    }
                    move -= speed.x * coolDown;
                    while (move < 0 && xd > 0)
                    {
                        move += xd;
                    }

                    break;
                case RainType.BothToBoth:
                    GameObject bullet1 = Instantiate(bullet);
                    GameObject bullet2 = Instantiate(bullet);
                    bullet1.transform.position = leftDownPos + new Vector2(move, size.y);
                    bullet2.transform.position = leftDownPos + new Vector2(size.x - move, size.y);

                    Bullet b1 = bullet1.GetComponent<Bullet>();
                    Bullet b2 = bullet2.GetComponent<Bullet>();
                    b1.damage = damage;
                    b2.damage = damage;
                    b1.speed = new Vector2(speed.x, -speed.y).magnitude;
                    b2.speed = new Vector2(-speed.x, -speed.y).magnitude;

                    Vector2 direction1 = new Vector2(speed.x, -speed.y).normalized;
                    b1.direction = direction1;
                    Vector2 direction2 = new Vector2(-speed.x, -speed.y).normalized;
                    b2.direction = direction2;

                    move += speed.x * coolDown;
                    while (move > size.x && size.x > 0)
                    {
                        move -= size.x;
                    }

                    break;
            }
        }
    }

    private void OnDrawGizmosSelected()
    {
        if (size.x < 0)
        {
            size = new Vector2(0, size.y);
        }
        if (size.y < 0)
        {
            size = new Vector2(size.x, 0);
        }
        if (column == 0)
        {
            column = 1;
        }
        if (row == 0)
        {
            row = 1;
        }
        if (speed.x < 0)
        {
            speed = new Vector2(-speed.x, speed.y);
        }
        if (speed.y < 0)
        {
            speed = new Vector2(speed.x, -speed.y);
        }

        Gizmos.color = Color.white;

        float xd = size.x / column;
        for (int i = 0; i < column; i++)
        {
            Gizmos.DrawLine(leftDownPos + new Vector2(i* xd + move, 0), leftDownPos + new Vector2(i * xd + move, size.y));
        }

        float yd = size.y / row;
        for (int i = 0; i < row; i++)
        {
            Gizmos.DrawLine(leftDownPos + new Vector2(0, i*yd), leftDownPos + new Vector2(size.x, i*yd));
        }
    }
}
