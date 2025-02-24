using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExperimentAttack : MonoBehaviour
{
    public float timer;
    public int bulletIndex;
    private void Update()
    {
        if (timer >= 0)
        {
            timer -= Time.deltaTime;
        }
        else
        {
            if (bulletIndex == 1)
            {
                gameObject.GetComponent<Rigidbody2D>().velocity = -gameObject.GetComponent<Rigidbody2D>().velocity;
                bulletIndex = 10;
            }
        }
    }
}
