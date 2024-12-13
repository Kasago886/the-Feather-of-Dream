using System.Collections;
using System.Collections.Generic;
using UnityEngine;
/// <summary>
/// 这是所有buff需要继承的接口，你可以把它继承到buff的基类上
/// </summary>
public interface BuffInterface
{
    /// <summary>
    /// 这是初始化对象的方法
    /// </summary>
    void Initialize();
    /// <summary>
    /// 这是帧执行的方法
    /// </summary>
    void Update();
}
