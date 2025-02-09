using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class BoxMapListenerState : IListenerState
{
    InputListener listener;

    public BoxMapListenerState(InputListener listener)
    {
        this.listener = listener;
    }

    public void Update()
    {

        if (Input.GetKeyDown(KeyCode.Escape))
        {
            listener.SwitchPausePanel();
        }
    }
}
