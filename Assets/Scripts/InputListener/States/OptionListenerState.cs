using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class OptionListenerState : IListenerState
{
    InputListener listener;
    public UnityEvent escapeEvent = new();

    public OptionListenerState(InputListener listener)
    {
        this.listener = listener;
    }

    public void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            escapeEvent?.Invoke();
        }
    }
}
