using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ControllerPauseState : ControllerState
{
    public ControllerPauseState(PlayerController controller) : base(controller)
    {
    }

    public override void OnMove(float horizontal)
    {
        if (Mathf.Abs(controller.HorizontalToSpeed(horizontal)) < Mathf.Abs(controller.GetComponent<Rigidbody2D>().velocity.x))
        {
            base.OnMove(horizontal);
        }
    }

    public override void OnJump()
    {
    }

    public override void OnSprint()
    {
    }

    public override void OnAttack()
    {
    }
}
