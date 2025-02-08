using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class PlayerDistanceDetector : MonoBehaviour
{
    public float distance;
    public UnityEvent enterEvent;
    public UnityEvent exitEvent;

    bool isEntered = false;
    Transform player;
    // Start is called before the first frame update
    void Start()
    {
        player = GameObject.FindGameObjectWithTag(Consts.PlayerTag).transform;
    }

    // Update is called once per frame
    void Update()
    {
        if (!isEntered && Vector2.Distance(transform.position, player.position) <= distance)
        {
            enterEvent?.Invoke();
        }
        else if (isEntered && Vector2.Distance(transform.position, player.position) > distance)
        {
            exitEvent?.Invoke();
        }
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.cyan;

        float r = distance;
        for (int i = 0; i < 360; i++)
        {
            Gizmos.DrawLine(new Vector3(transform.position.x + r * Mathf.Cos((i * Mathf.PI) / 180), transform.position.y + r * Mathf.Sin((i * Mathf.PI) / 180), 0), new Vector3(transform.position.x + r * Mathf.Cos(((i + 1) * Mathf.PI) / 180), transform.position.y + r * Mathf.Sin(((i + 1) * Mathf.PI) / 180), 0));
        }
    }
}
