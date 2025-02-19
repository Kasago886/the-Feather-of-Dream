using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Events;

[Serializable]
public struct FlagCondition
{
    public FlagType flagType;
    public bool opposite;
}
public class PlayerDistanceDetector : MonoBehaviour
{
    public float distance;
    public UnityEvent enterEvent;
    public UnityEvent exitEvent;
    [Header("仅执行一次（勾选后仅执行enterEvent）")]
    public bool onceTrriger;
    [Header("需要Flag或Condition（勾选后仅执行enterEvent）")]
    [Header("Flag为false时执行，且执行后为true")]
    public bool flagNeed;
    public FlagType flagType;

    [Header("Condition为true时执行,执行后不改变值\n勾选oppositie选项则Condition为false时执行")]
    public bool conditionsNeed;
    public FlagCondition[] conditions;
    [Header("按标签检测，勾选则忽略玩家")]
    public bool tagDetect;
    public string targetTag;
    [Header("开始时若目标不在范围内则自我摧毁")]
    public bool startDetect;

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

        if (startDetect)
        {
            bool startEnter = false;
            if (tagDetect)
            {
                foreach (GameObject go in targets)
                {
                    if (EnterDetect(go.transform))
                    {
                        startEnter = true;
                    }
                }
            }
            else
            {
                if (EnterDetect(player))
                {
                    startEnter = true;
                }
            }
            if (!startEnter)
            {
                Destroy(gameObject);
            }
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (tagDetect)
        {
            foreach (GameObject go in targets)
            {
                if (EnterDetect(go.transform))
                {
                    EnterFunc(go.transform);
                }
                if (ExitDetect(go.transform))
                {
                    ExitFunc(go.transform);
                }
            }
        }
        else
        {
            //Debug.Log("detecting player");
            if (EnterDetect(player.transform))
            {
                //Debug.Log("player enter");
                EnterFunc(player.transform);
            }
            if (ExitDetect(player.transform))
            {
                ExitFunc(player.transform);
            }
        }
    }

    void EnterFunc(Transform targetTrans)
    {
        enteredObj.Add(targetTrans);
        isTrriggered = true;

        if (flagNeed)
        {
            ArchiveManager.CheckFlag(flagType, true);
        }

        enterEvent?.Invoke();
    }
    void ExitFunc(Transform targetTrans)
    {
        enteredObj.Remove(targetTrans);

        if (enteredObj.Count == 0)
        {
            exitEvent?.Invoke();
        }
    }

    bool EnterDetect(Transform targetTrans)
    {
        if (!enteredObj.Contains(targetTrans)
            && Vector2.Distance(transform.position, targetTrans.position) <= distance 
            && !(flagNeed && ArchiveManager.CheckFlag(flagType))
            && !(conditionsNeed && !CheckConditions(conditions))
            && !(onceTrriger && isTrriggered))
        {
            return true;
        }
        return false;
    }
    bool ExitDetect(Transform targetTrans)
    {
        if (enteredObj.Contains(targetTrans)
            && Vector2.Distance(transform.position, targetTrans.position) > distance
            && !flagNeed
            && !conditionsNeed
            && !onceTrriger)
        {
            return true;
        }
        return false;
    }

    bool CheckConditions(FlagCondition[] conditions)
    {
        bool allSatisfied = true;
        foreach(FlagCondition condition in conditions)
        {
            /// CheckFlag  | opposite  | satisfied
            /// true         false       true
            /// true         true        false
            /// false        false       false
            /// false        true        true
            FlagType flagType = condition.flagType;
            bool opposite = condition.opposite;

            //Debug.Log(flagType);
            //Debug.Log("checkflagtype="+ArchiveManager.CheckFlag(flagType));
            //Debug.Log("opposite="+opposite);
            if (ArchiveManager.CheckFlag(flagType) == opposite)
            {
                //Debug.Log("allsatisfied=false");
                allSatisfied = false;
                break;
            }
        }
        return allSatisfied;
    }

    public void Destroy()
    {
        Destroy(gameObject);
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
