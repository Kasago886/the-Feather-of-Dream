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
}
