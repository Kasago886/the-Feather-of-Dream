using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class DeathListenerState : IListenerState
{
    InputListener listener;

    public DeathListenerState(InputListener listener)
    {
        this.listener = listener;
    }

    public void Update()
    {
    }
}
