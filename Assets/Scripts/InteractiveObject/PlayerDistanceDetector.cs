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
    [Header("按标签检测，勾选则忽略玩家")]
    public bool tagDetect;
    public string targetTag;

    List<Transform> enteredObj = new();
    bool isTrriggered = false;
    Transform player;
    GameObject[] targets;
    // Start is called before the first frame update
    void Start()
    {
        player = GameObject.FindGameObjectWithTag(Consts.PlayerTag).transform;

        if (tagDetect)
        {
            targets = GameObject.FindGameObjectsWithTag(targetTag);
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (tagDetect)
        {
            foreach (GameObject go in targets)
            {
                Detect(go.transform);
            }
        }
        else
        {
            Detect(player);
        }
    }

    void Detect(Transform targetTrans)
    {
        if (!enteredObj.Contains(targetTrans) && Vector2.Distance(transform.position, targetTrans.position) <= distance && !(flagOnly && ArchiveManager.CheckFlag(flagType)) && !(onceTrriger && isTrriggered))
        {
            enteredObj.Add(targetTrans);
            isTrriggered = true;

            if (flagOnly)
            {
                ArchiveManager.CheckFlag(flagType, true);
            }

            enterEvent?.Invoke();
        }
        else if (enteredObj.Contains(targetTrans) && Vector2.Distance(transform.position, targetTrans.position) > distance && !flagOnly && !onceTrriger)
        {
            enteredObj.Remove(targetTrans);

            if (enteredObj.Count == 0 )
            {
                exitEvent?.Invoke();
            }
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
