using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class ControllerSprintingState : ControllerState
{
    public ControllerSprintingState(PlayerController controller) : base(controller)
    {
    }

    public override void OnMove(float horizontal)
    {
    }

    public override void OnJump(bool isNoGroundJump = false)
    {
    }

    public override void OnAttack()
    {
    }
}
