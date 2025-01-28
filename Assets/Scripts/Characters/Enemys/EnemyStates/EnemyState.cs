using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface EnemyState
{
    public abstract void OnEnter();

    public abstract void OnExit();

    public abstract void OnUpdate();
}
