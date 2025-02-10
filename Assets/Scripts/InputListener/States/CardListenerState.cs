using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class CardListenerState : IListenerState
{
    InputListener listener;
    public CardListenerState(InputListener listener)
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
            listener.SwitchCardPanel();

            listener.StateTransition(InputListenerState.normal);
            return;
        }
        if (Input.GetKeyDown(KeyCode.Mouse0))
        {
            listener.playerController.StateAttack();
        }

        if (Input.GetKeyDown(KeyCode.Escape))
        {
            listener.SwitchPausePanel();
        }
    }
}
