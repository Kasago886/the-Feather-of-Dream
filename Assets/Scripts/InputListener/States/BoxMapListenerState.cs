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
        if (Input.GetKeyDown(KeyCode.Tab))
        {
            if (listener.equipmentPanelManager != null)
            {
                listener.equipmentPanelManager.SwitchShow();

                if (listener.equipmentPanelManager.isShow)
                {
                    listener.StateTransition(InputListenerState.equipment);
                    return;
                }
            }
        }
        if (Input.GetKeyDown(KeyCode.A))
        {
            listener.playerIn21.Move(Vector3.left);
        }
        if (Input.GetKeyDown(KeyCode.D))
        {
            listener.playerIn21.Move(Vector3.right);
        }
        if (Input.GetKeyDown(KeyCode.S))
        {
            listener.playerIn21.Move(Vector3.down);
        }
        if (Input.GetKeyDown(KeyCode.W))
        {
            listener.playerIn21.Move(Vector3.up);
        }
    }
}
