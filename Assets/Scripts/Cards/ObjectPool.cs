using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectPool<T> where T : Component
{
    private Stack<T> pool = new Stack<T>();
    private Stack<T> otherPool = new Stack<T>();
    private GameObject prefob;
    private List<Transform> container = new List<Transform>();
    /// <summary>
    /// 创建ObjectPool
    /// </summary>
    /// <param name="prefob">
    /// 预制件
    /// </param>
    /// <param name="container">
    /// 实例生成的位置
    /// </param>
    public ObjectPool(GameObject prefob, Transform[] container)
    {
        this.prefob = prefob;
        foreach (Transform t in container)
        {
            this.container.Add(t);
        }
        for (int i = 0; i < this.container.Count; i++)
        {
            T obj = CreateNewGameObject();
            ReturnToPool(obj, this.container[i]);
        }
    }
    /// <summary>
    /// 使用该方法从对象池中获取对象
    /// </summary>
    /// <param name="container">
    /// 卡牌所对应的位置
    /// </param>
    /// <returns></returns>
    public T GetFromPool(Transform container)
    {
        if (pool.Count > 0)
        {
            for (int i = 0; i < this.container.Count; i++)
            {
                if (pool.Count > 0)
                {
                    T obj = pool.Pop();
                    if (obj.gameObject.transform.parent == container)
                    {
                        obj.gameObject.SetActive(true);
                        ReturnToPool();
                        return obj;
                    }
                    else
                    {
                        otherPool.Push(obj);
                    }
                }
                else
                {
                    ReturnToPool();
                    return null;
                }
            }
            ReturnToPool();
            return null;
        }
        return null;
    }
    private void ReturnToPool(T obj, Transform container)
    {
        obj.gameObject.SetActive(false);
        obj.transform.parent = container;
        pool.Push(obj);
    }
    /// <summary>
    /// 归还所使用的对象添加新的对象
    /// </summary>
    /// <param name="obj">
    /// 对象
    /// </param>
    public void ReturnToPool(T obj)
    {
        obj.gameObject.SetActive(false);
        int number = 0;
        for (int i = 0; i < this.container.Count; i++)
        {
            if (obj.gameObject.transform.parent != this.container[i])
            {
                number++;
            }
        }
        if (number == this.container.Count)
        {
            this.container.Add(obj.gameObject.transform.parent);
        }
        pool.Push(obj);
    }
    private void ReturnToPool()
    {
        while (otherPool.Count > 0)
        {
            T other = otherPool.Pop();
            pool.Push(other);
        }
    }
    private T CreateNewGameObject()
    {
        GameObject gameObject = new GameObject();
        gameObject.SetActive(false);
        return gameObject.GetComponent<T>();
    }
    /// <summary>
    /// 清空对象池
    /// </summary>
    public void ClearPool()
    {
        for (int i = 0; i < this.pool.Count; i++)
        {
            pool.Pop();
        }
        container.Clear();
    }
}
