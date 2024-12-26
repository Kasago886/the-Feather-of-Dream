using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.TextCore.Text;

public class Character : MonoBehaviour
{
    public bool isDefaultFeather;
    public int defaultFeatherNum;
    [Header("韧性")]
    public float tenacity;
    [Header("力量")]
    public float strength;

    [Header("攻击替身")]
    public List<GameObject> attackBodyObjList;

    [Header("受伤事件")]
    public UnityEvent injuryEvent;
    [Header("治疗事件")]
    public UnityEvent healEvent;
    [Header("死亡事件")]
    public UnityEvent deathEvent;

    public List<Buff> buffList = new();
    public List<Feather> feathers = new();

    // Start is called before the first frame update
    protected void Start()
    {
        if (isDefaultFeather)
        {
            defaultFeatherNum = EditorGUILayout.IntField("默认羽数", defaultFeatherNum);

            feathers = new();
            for (int i = 0; i < defaultFeatherNum; i++)
            {
                feathers.Add(new DefautFeather());
            }
        }
    }

    // Update is called once per frame
    protected void Update()
    {
        BuffUpdate();
    }

    /// <summary>
    /// 攻击
    /// </summary>
    public void OnAttack()
    {
        foreach (GameObject obj in attackBodyObjList)
        {
            GameObject instance = Instantiate(obj, transform.position, Quaternion.identity);
            instance.transform.localRotation = Quaternion.Euler(0, 180, 0);
        }
    }

    /// <summary>
    /// 受伤
    /// </summary>
    /// <param name="damage"></param>
    public void TakeDamage(float damage)
    {

    }

    #region buff
    /// <summary>
    /// 添加Buff
    /// </summary>
    /// <param name="buff"></param>
    public void AddBuff(string buffName)
    {
        Buff buff = BuffContainer.GetBuffInstance(buffName) as Buff;
        buffList.Add(buff);
        buff.OnEnter();
    }

    /// <summary>
    /// buff更新
    /// </summary>
    public void BuffUpdate()
    {
        //此处不能用foreach，因为循环中要修改buffList
        for (int i = buffList.Count - 1; i >= 0; i--)
        {
            Buff buff = buffList[i];

            //更新
            buff.OnUpdate();
            
            if (!buff.isPermanent)
            {
                //减少倒计时
                buff.timer -= Time.deltaTime;
                if (buff.timer <= 0)
                {
                    //移除buff
                    buff.OnExit();
                    buffList.Remove(buff);
                }
            }
        }
    }

    /// <summary>
    /// 从列表中移除一个buff
    /// </summary>
    /// <param name="buffName"></param>
    public void RemoveBuff(string buffName)
    {
        Type buffType = BuffContainer.GetBuffType(buffName);
        for (int i = buffList.Count - 1; i >= 0; i--)
        {
            Buff buff = buffList[i];

            if (buffType.IsInstanceOfType(buff))
            {
                buff.OnExit();
                buffList.Remove(buff);
                break;
            }
        }
    }
    #endregion
}
