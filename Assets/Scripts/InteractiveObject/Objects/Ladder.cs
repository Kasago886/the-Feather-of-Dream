using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Events;

public class Ladder : InteractiveObject 
{
    Player p;
    PlayerController controller;
    protected override void Start()
    {
        base.Start();
        
        p = player.GetComponent<Player>();
        controller = p.GetComponent<PlayerController>();
    }
    public override void Interact()
    {
        base.Interact();

        if (p.rb.velocity.y < 7)
        {
            p.rb.velocity = new Vector2(p.rb.velocity.x, 7);

            controller.animator.SetBool(Consts.IsClimbAnimatorArgument, true);
            controller.animator.Play("Climb");
        }

        if (Input.GetKeyDown(KeyCode.Space))
        {
            controller.animator.SetBool(Consts.IsClimbAnimatorArgument, false);
            controller.animator.Play("Idle");

            //Debug.Log("statejump");
            controller.StateJump(true);
        }
    }

    protected override void OnTriggerExit2D(Collider2D collision)
    {
        base.OnTriggerExit2D(collision);

        if (!gameObject.IsDestroyed())
        {
            controller.animator.SetBool(Consts.IsClimbAnimatorArgument, false);
        }
    }
}
