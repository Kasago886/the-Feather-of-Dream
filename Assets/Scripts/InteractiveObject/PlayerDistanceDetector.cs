using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class PlayerDistanceDetector : MonoBehaviour
{
    public float distance;
    public UnityEvent enterEvent;
    public UnityEvent exitEvent;
    [Header("仅执行一次（勾选后仅执行enterEvent）")]
    public bool onceTrriger;
    [Header("需要Flag（勾选后仅执行enterEvent）")]
    public bool flagOnly;
    public FlagType flagType;

    bool isEntered = false;
    bool isTrriggered = false;
    Transform player;
    // Start is called before the first frame update
    void Start()
    {
        player = GameObject.FindGameObjectWithTag(Consts.PlayerTag).transform;
    }

    // Update is called once per frame
    void Update()
    {
        if (!isEntered && Vector2.Distance(transform.position, player.position) <= distance && !(flagOnly && ArchiveManager.CheckFlag(flagType)) && !(onceTrriger && isTrriggered))
        {
            isEntered = true;
            isTrriggered = true;

            if (flagOnly)
            {
                ArchiveManager.CheckFlag(flagType,true);
            }

            enterEvent?.Invoke();
        }
        else if (isEntered && Vector2.Distance(transform.position, player.position) > distance && !flagOnly && !onceTrriger)
        {
            isEntered = false;

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
