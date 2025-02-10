using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovingPlatform : MonoBehaviour
{
    public Transform[] points;
    public float maxSpeed;

    bool back = false;
    float distance;

    // Start is called before the first frame update
    void Start()
    {
        transform.position = points[0].position;
        distance = Vector2.Distance(points[0].position, points[1].position);
    }

    // Update is called once per frame
    void Update()
    {
        Transform target;
        if (back)
        {
            target = points[0];
        }
        else
        {
            target = points[1];
        }

        float distanceToTarget = Vector2.Distance(transform.position, target.position);
        float ratio = 1 - (Mathf.Abs(distanceToTarget - (distance / 2)) / (distance / 2));
        if (ratio <= 0.1f)
        {
            ratio = 0.1f;
        }

        Vector3 direction = (target.position - transform.position).normalized;
        transform.position += direction * ratio * maxSpeed * Time.deltaTime;

        if (distanceToTarget < 0.01f)
        {
            back = !back;
        }

        //Debug.Log(ratio);
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.yellow;
        if (points.Length >= 2)
        {
            Gizmos.DrawLine(points[0].position, points[1].position);
        }
    }
}
