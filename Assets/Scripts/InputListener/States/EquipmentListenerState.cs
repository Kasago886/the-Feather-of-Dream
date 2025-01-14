using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class EquipmentListenerState : IListenerState
{
    InputListener listener;
    public EquipmentListenerState(InputListener listener)
    {
        this.listener = listener;
    }

    public void Update()
    {
        if (Input.GetKeyDown(KeyCode.Tab) || Input.GetKeyDown(KeyCode.Escape))
        {
            listener.equipmentPanelManager.SwitchShow();

            listener.StateTransition(InputListenerState.normal);
        }
    }
}
