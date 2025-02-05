using System.Collections;
using System.Collections.Generic;
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
        }

        if (Input.GetKeyDown(KeyCode.Space))
        {
            //Debug.Log("statejump");
            controller.StateJump(true);
        }
    }
}
