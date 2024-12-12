using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEngine.RuleTile.TilingRuleOutput;

public class ControllerState
{
    public PlayerController controller;
    public ControllerState(PlayerController controller) {  this.controller = controller; }

    public virtual void OnMove(float horizontal)
    {
        controller.OnMove(horizontal);
    }

    public virtual void OnJump()
    {
        controller.OnJump();
    }

    public virtual void OnSprint()
    {
        controller.OnSprint();
    }
}
