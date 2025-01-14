using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class NormalListenerState : IListenerState
{
    InputListener listener;
    public NormalListenerState(InputListener listener)
    {
        this.listener = listener;
    }

    public void Update()
    {
        listener.playerController.StateMove(Input.GetAxis("Horizontal"));

        if (Input.GetKeyDown(KeyCode.Space))
        {
            listener.playerController.StateJump();
        }
        if (Input.GetKeyDown(KeyCode.LeftShift))
        {
            listener.playerController.StateSprint();
        }
        if (Input.GetKeyDown(KeyCode.C))
        {
            listener.cardPanelAnimationManager.SwitchValue("appear");
        }
        if (Input.GetKeyDown(KeyCode.Tab))
        {
            listener.equipmentPanelManager.SwitchShow();

            listener.StateTransition(InputListenerState.equipment);
        }
        if (Input.GetKeyDown(KeyCode.Mouse0))
        {
            listener.playerController.StateAttack();
        }
    }
}
