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

    public virtual void OnJump(bool isNoGroundJump = false)
    {

        //Debug.Log("isNoGroundJump:" + isNoGroundJump);
        controller.OnJump(isNoGroundJump);
    }

    public virtual void OnSprint()
    {
        controller.OnSprint();
    }

    public virtual void OnAttack()
    {
        controller.OnAttack();
    }

    public virtual void OnEnter()
    {

    }

    public virtual void OnExit()
    {

    }
}
